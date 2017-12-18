using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using System.Windows;
using System.IO;
using System.IO.Compression;
using System.Net;
using SevenZip;
using System.Threading;
using System.Text.RegularExpressions;
using PSR_File_Downloader.Actions.Helper;
using PSR_File_Downloader.Model.Connects;
using SevenZipExtractor;

namespace PSR_File_Downloader.Actions
{
    abstract public class FilesAction : IFile
    {

        protected IPSR psr;
        protected List<Files> allfiles;
        public event Action<int> prbarIncrement;
        public event Action<double> prbarmax;
        public event Action<string> prbartext;

        public event Action<int> prbarIncrementOneFile;
        public event Action<double> prbarmaxOneFile;
        public event Action<int> prbarvalueOneFile;
        public void DownloaderFiles(List<Files> goalfiles, Wagon wagon, string path, CancellationTokenSource token)
        {   
            Regex nameanyf = new Regex(@".+\.");
            Regex namedatf = new Regex(@".+\.dat$");

            prbarmax(goalfiles.Where(file =>
                {
                    if (allfiles.Any(f => f.Name == file.Name + ".gz"))
                    {
                        return true;
                    }
                    if (allfiles.Any(f => f.Name == file.Name))
                    {
                        return true;
                    }
                    return false;
                }
                ).
                Select( file =>
                {
                    return new Files()
                    {
                        size = allfiles.Where(f => f.Name == file.Name).Single().size,
                        Name = file.Name,
                        DateChange=file.DateChange
                    };  
                }
                ).Sum(f=>f.size));
            
            foreach (var file in goalfiles)
            {
                prbarvalueOneFile(0);
                if (token.Token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    if (allfiles.Any(gz => nameanyf.Match(file.Name).Value + "dat.gz" == gz.Name))
                    {
                       
                        DownloaderFile(new Files { Name = nameanyf.Match(file.Name).Value + "dat.gz" }, wagon, path, token);
                        Decompress(new FileInfo(path + "\\" + file.Name + ".gz"));
                        File.SetLastWriteTime(path + "\\" + file.Name, file.DateChange);
                        File.Delete(Path.Combine(path,file.Name + ".gz"));

                    }
                    else
                    {
                        try
                        {
                            if (allfiles.Any(dat => nameanyf.Match(file.Name).Value + "dat" == dat.Name))
                            {
                                DownloaderFile(new Files { Name = nameanyf.Match(file.Name).Value + "dat" }, wagon, path, token);
                                File.SetLastWriteTime(path + "\\" + nameanyf.Match(file.Name).Value + "dat", file.DateChange);
                            }
                        }
                        catch { }

                    }
                }

                catch
                {
                    try
                    {
                        if (allfiles.Any(dat => nameanyf.Match(file.Name).Value + "dat" == dat.Name))
                        {
                            DownloaderFile(new Files { Name = nameanyf.Match(file.Name).Value + "dat" }, wagon, path, token);
                            File.SetLastWriteTime(path + "\\" + nameanyf.Match(file.Name).Value + "dat", file.DateChange);
                        }
                    }
                    catch { }
                }

            }
            token.Cancel();

        }
        int sizeprbarOnefile;
        private void DownloaderFile(Files file, Wagon wagon, string path, CancellationTokenSource token)
        {
            // Создаем объект FtpWebRequest
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/" + file.Name);
            // устанавливаем метод на загрузку файлов
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // если требуется логин и пароль, устанавливаем их
            request.Credentials = new NetworkCredential("ftppsr", "ftppsr");
            //request.EnableSsl = true; // если используется ssl

            // получаем ответ от сервера в виде объекта FtpWebResponse
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {

                // получаем поток ответа
                Stream responseStream = response.GetResponseStream();
                // сохраняем файл в дисковой системе
                // создаем поток для сохранения файла
                
                using (FileStream fs = new FileStream(path + @"\" + file.Name + ".bad", FileMode.Create))
                {
                    //Буфер для считываемых данных
                    byte[] buffer = new byte[2048];
                    int size = 0;
                    prbartext(file.Name.Replace("dat.gz", "dat"));
                 //   prbarmaxOneFile(allfiles.Where(files => files.Name == file.Name).Single().size);
                    while (( size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (token.Token.IsCancellationRequested)
                        {
                            break;
                        }
                     //   prbarIncrementOneFile(size);
                        prbarIncrement(size);
                      //  sizeprbarOnefile += size;
                        fs.Write(buffer, 0, size);


                    }
                }
                if (token.Token.IsCancellationRequested)
                {
                    File.Delete(path + @"\" + file.Name + ".bad");
                    return;
                }
                if (File.Exists(path + @"\" + file.Name))
                {
                    File.Delete(path + @"\" + file.Name);
                }
                File.Move(path + @"\" + file.Name + ".bad", path + @"\" + file.Name);
            }
        }

        abstract public List<Files> GetListFilesFromPSR(Wagon wagon, bool twoWeek);

        public IEnumerable<Files> NotDownload(string path, List<Files> requaredFile)
        {

            foreach (var file in requaredFile)
            {
                if (!File.Exists(path + @"\" + file.Name))
                {
                    yield return file;
                }
            }
        }

        private void Decompress(FileInfo fileToDecompress)
        {

            string zipPath = fileToDecompress.FullName;
            string newFile = fileToDecompress.FullName.Replace(".dat.gz",".dat");
            
                // throw new Exception();
                string currentFileName = zipPath;
                string newFileName = newFile;

                string currentArchitecture = IntPtr.Size == 4 ? "x86" : "x64"; // magic check
                using (ArchiveFile archiveFile = new ArchiveFile(zipPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, currentArchitecture, "7z.dll")))
                {
                    foreach (Entry entry in archiveFile.Entries)
                    {
                        Console.WriteLine(entry.FileName);

                        // extract to file
                        entry.Extract(newFile);

                        // extract to stream
                        MemoryStream memoryStream = new MemoryStream();
                        entry.Extract(memoryStream);
                    }
                }



            
        }
    }
}
