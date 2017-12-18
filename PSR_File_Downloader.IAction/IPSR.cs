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
