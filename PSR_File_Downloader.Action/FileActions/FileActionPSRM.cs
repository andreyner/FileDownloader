using PSR_File_Downloader.Actions.Helper;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using PSR_File_Downloader.Model.Connects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Actions.PSRActions
{
    public  class FileActionPSRM : FilesAction 
    {
        public FileActionPSRM(IPSR psrAcrion)
        {
            this.psr = psrAcrion;
            culture = new CultureInfo("en-Us");
        }
        private CultureInfo culture;
        public override List<Files> GetListFilesFromPSR(Model.Wagon wagon, bool twoWeek)
        {
            psr.SetLoginPassword(ref wagon);
            psr.Existnetwor(wagon.psr);
            psr.ChekConnect(wagon.psr);

            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string file = "";
                    allfiles = new List<Files>();

                    while ((file = reader.ReadLine()) != null)
                    {
                        try
                        {   
                            string[] f = file.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (f.Length == 9)
                            {
                                int year = 0;
                                year = DateTime.Now.Year;
                               
                                DateTime date = DateTime.ParseExact(String.Format("{0}:{1} {2}:{3}:{4}", f[7].Split(':')[0], f[7].Split(':')[1], f[6], f[5], year), "HH:mm d:MMM:yyyy", culture);
                                if (date.Month > DateTime.Now.Month)
                                { year--; date = DateTime.ParseExact(String.Format("{0}:{1} {2}:{3}:{4}", f[7].Split(':')[0], f[7].Split(':')[1], f[6], f[5], year), "HH:mm d:MMM:yyyy", culture); }
                                FtpWebRequest requestdate = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/" + f[8]);
                                requestdate.Method = WebRequestMethods.Ftp.GetFileSize;
                                using (FtpWebResponse responsedate = (FtpWebResponse)requestdate.GetResponse())
                                {  
                                   
                                    allfiles.Add(new Files
                                    {
                                        Name = f[8],
                                        size = Convert.ToInt32(responsedate.ContentLength),
                                        DateChange = date

                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
               
                throw new Exception("Не удалось получить список файлов на ПСР!");

            
            }

            return allfiles.Where(dat =>
            {
                Regex name = new Regex(@".\.dat$");
                if (name.IsMatch(dat.Name)) return true;
                else return false;
            }).Where(date =>
            {
                if (!twoWeek)
                {
                    return true;
                }
                else
                {
                    if (DateTime.Now.Subtract(date.DateChange) <= TimeSpan.FromDays(14))
                    {
                        return true;
                    }
                    else { return false; }
                }
            }).ToList<Files>(); 
        }
    }
}
