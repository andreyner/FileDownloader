using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using PSR_File_Downloader.Model.Connects;
using System.Net.NetworkInformation;
using System.Threading;

namespace PSR_File_Downloader.Actions
{
   public  class PSRAction:IPSR
    {

       public event Action<int> prbarIncrement;
       public  event Action<double> prbarmax;
       public  event Action<string> prbartext;
       public PSRAction()
       {
           
       }
        public void SetLoginPassword(ref Wagon wagon)
        {
            if (wagon.psr.connect is WI_FI)
            {

                wagon.psr.IPadres = "psr" + wagon.number + ".mshome.net";
                wagon.psr.Login = "ftppsr";
                wagon.psr.Password = "ftppsr";
                return;
            }
            if (wagon.psr.connect is Ethernet && (wagon.psr is PSRL || wagon.psr is PSRLE))
            {
                wagon.psr.IPadres = "10.49.24.240";
                wagon.psr.Login = "ftppsr";
                wagon.psr.Password = "ftppsr";
                return;
            }
#warning тут ошибка
            if (wagon.psr.connect is Ethernet && (wagon.psr is PSRM))
            {
                wagon.psr.IPadres = "192.168.105.181";
                wagon.psr.Login = "";
                wagon.psr.Password = "";
                return;
            }
            throw new Exception("Неизвестная модель ПСР");
        }


        public void ChekConnect(PSR psr)
        {
            Ping ping = new Ping();

            if(ping.Send(psr.IPadres, 3000).Status!=IPStatus.Success)
            {
                throw new Exception("Не удалось установить связь с ПСР");
            }
             
        }

        public void Download(List<Files> files, Wagon wagon,string path)
        {
            prbarmax(files.Sum(f=>f.size));
            foreach (var file in files)
            {

              
                // Создаем объект FtpWebRequest
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/"+file.Name);
                // устанавливаем метод на загрузку файлов
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // если требуется логин и пароль, устанавливаем их
                //request.Credentials = new NetworkCredential("login", "password");
                //request.EnableSsl = true; // если используется ssl
              
                // получаем ответ от сервера в виде объекта FtpWebResponse
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {

                    // получаем поток ответа
                    Stream responseStream = response.GetResponseStream();
                    // сохраняем файл в дисковой системе
                    // создаем поток для сохранения файла

                    using (FileStream fs = new FileStream(path + @"\" + file.Name, FileMode.Create))
                    {
                        //Буфер для считываемых данных
                        byte[] buffer = new byte[4096];
                        int size = 0;
                        prbartext(file.Name);
                        while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            prbarIncrement(size);
                            fs.Write(buffer, 0, size);

                        }
                    }

                }
            }
        }

        public List<Files> GetListFilesFromPSR(Wagon wagon, bool twoWeek)
        {
         
           SetLoginPassword(ref wagon);
           if (wagon.psr.connect is WI_FI)
           {
               CreatWi_FiNetwor();
           }
           Existnetwor(wagon.psr);
           ChekConnect(wagon.psr);


           FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + wagon.psr.IPadres + "/");
           request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
           FtpWebResponse response = (FtpWebResponse)request.GetResponse();
           Stream responseStream = response.GetResponseStream();
           StreamReader reader = new StreamReader(responseStream);
           string file = "";
           List<Files> fileinfo = new List<Files>();
         
           while ((file = reader.ReadLine()) != null)
           {
               string[] f = file.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
               fileinfo.Add(new Files
               {
                   Name = f[3],
                   size = Convert.ToInt32(f[2]),
                   DateChange = DateTime.Parse(f[0]+" "+f[1])
               });


           }

           return fileinfo;
           
        }

        public string GetIP(PSR psr, Connect connect)
        {

            return "";
        }

#warning тут ошибка
        public bool Existnetwor(PSR psr)
        {
            if (psr.connect is Ethernet)
            {
                string Host = System.Net.Dns.GetHostName();
                IPAddress[] IP = System.Net.Dns.GetHostEntry(Host).AddressList;
                if (!IP.Select(ip => ip.ToString().Contains("192.168.105.") && !ip.ToString().Contains("10.49.24.240")).Contains(true))
                {
                    throw new Exception("Установите нужный IP адрес на компьютере,например 10.49.24.230");
                
                }
                return true;
            }
            if (psr.connect is WI_FI)
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(@"cmd.exe");
                // Следующая команды означает, что нужно перенаправить стандартынй вывод
                // на Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // не создавать окно CMD
                procStartInfo.CreateNoWindow = true;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                // Получение текста в виде кодировки 866 win
                procStartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                //запуск CMD
                proc.StartInfo = procStartInfo;
                proc.Start();
                // System.Threading.Thread.Sleep(5000);
                //чтение результата
                string result = proc.StandardOutput.ReadToEnd();


                if (!(result.Contains("Запущено") && result.Contains("psr_soft")))
                {
                    throw new Exception( "Не создана сеть WI-FI для ПСР" );

                }
                return true;
  
            }
            return true;
           
        }


        public void CreatWi_FiNetwor()
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(@"cmd.exe");
            procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.RedirectStandardInput = true;
            System.Diagnostics.Process proc = System.Diagnostics.Process.Start(procStartInfo);
            proc.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=psr_soft key=349T1074");//349T1074pi
            proc.StandardInput.WriteLine("netsh wlan start hostednetwork");
            System.Threading.Thread.Sleep(2000);
        }
    }
}
