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
using System.Text.RegularExpressions;
using PSR_File_Downloader.Actions.Helper;

namespace PSR_File_Downloader.Actions
{
   public  class PSRAction:IPSR
    {


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
            if (wagon.psr.connect is Ethernet && (wagon.psr is PSRM))
            {
                wagon.psr.IPadres = "10.49.24.240";
                wagon.psr.Login = "";
                wagon.psr.Password = "";
                return;
            }
            throw new Exception("Неизвестная модель ПСР");
        }


        public void ChekConnect(PSR psr)
        {
            Ping ping = new Ping();

            if (ping.Send(psr.IPadres, 3000).Status != IPStatus.Success)
            {
                throw new Exception("Не удалось установить связь с ПСР");
            }
             
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
                if (!IP.Select(ip => ip.ToString().Contains("10.49.24.") && !ip.ToString().Contains("10.49.24.240")).Contains(true))
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
