using PSR_File_Downloader.Model;
using PSR_File_Downloader.Model.Connects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSR_File_Downloader.IAction
{
   public interface IPSR
   {
       event Action<int> prbarIncrement;
       event Action<double> prbarmax;
       event Action<string> prbartext;
       /// <summary>
       /// Установить нужный логин/пароль для ПСР
       /// </summary>
       /// <param name="psr"></param>
       void SetLoginPassword(ref Wagon wagon);
       /// <summary>
       /// Проверить соединение с ПСР
       /// </summary>
       /// <param name="psr"></param>
      void ChekConnect(PSR psr);
       /// <summary>
       /// Загрузка файла с ПСР
       /// </summary>
       /// <param name="file"> Нужный файл</param>
       /// <param name="twoWeek"> загрузка/не загрузка за последние 2 недели</param>
       /// <param name="psr"> ПСР</param>
       /// <returns></returns>
       void Download(List<Files> files, Wagon wagon, string path);
       /// <summary>
       /// Получить список файлов с ПСР
       /// </summary>
       /// <param name="psr"></param>
       /// <returns></returns>
       List<Files> GetListFilesFromPSR(Wagon wagon, bool twoWeek);
       /// <summary>
       /// Получить IP адресс ПСР
       /// </summary>
       /// <param name="psr"> ПСР</param>
       /// <param name="connect"> соедение</param>
       /// <returns> IP адрес</returns>
       string GetIP(PSR psr, Connect connect);
       /// <summary>
       /// Проверка существования сети на компьютере
       /// </summary>
       /// <param name="psr"></param>
       /// <returns></returns>
       bool Existnetwor(PSR psr);
       /// <summary>
       /// Создаёт wi-fi сеть на компьютере
       /// </summary>
       /// <returns></returns>
       void CreatWi_FiNetwor();
 
    }
}
