using PSR_File_Downloader.Actions.Helper;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Actions.FileActions
{
    public class FileActionPSRL : FilesAction 
    {
        public FileActionPSRL(IPSR psrAcrion)
        {
            this.psr = psrAcrion;
            culture = new CultureInfo("en-Us");
        }
        private CultureInfo culture;
        public override List<Files> GetListFilesFromPSR(Wagon wagon, bool twoWeek)
        {
            psr.SetLoginPassword(ref wagon);
            psr.Existnetwor(wagon.psr);
            psr.ChekConnect(wagon.psr);

            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(wagon.psr.Login, wagon.psr.Password);
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
                            if (f.Length == 9 && f[7].Contains(":"))
                            {


                                        int year = DateTime.Now.Year;
                                        DateTime date = DateTime.ParseExact(String.Format("{0}:{1} {2}:{3}:{4}", f[7].Split(':')[0], f[7].Split(':')[1], f[6], f[5], year), "HH:mm d:MMM:yyyy", culture);
                                        if (date.Month > DateTime.Now.Month)
                                        { year--; date = DateTime.ParseExact(String.Format("{0}:{1} {2}:{3}:{4}", f[7].Split(':')[0], f[7].Split(':')[1], f[6], f[5], year), "HH:mm d:MMM:yyyy", culture); }
                                 
                                
                                    allfiles.Add(new Files
                                    {
                                        Name = f[8],
                                        size = Convert.ToInt32(f[4]),
                                        DateChange = date
                                    });
                                
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

            return (allfiles.Where(dat =>
            {
                Regex name = new Regex(@".\.dat$");
                if (name.IsMatch(dat.Name)) return true;
                else return false;
            }).Union(allfiles.Where(gz =>
            {
                Regex name = new Regex(@".\.gz$");
                if (name.IsMatch(gz.Name))
                {
                    return true;
                }
                else return false;
            }).Select(gz =>
            {
                return new Files
                {
                    Name = gz.Name.Replace(".gz", ""),
                    DateChange = gz.DateChange,
                    size = gz.size

                };
            }))).Distinct(new FileComparer()).Where(date =>
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
