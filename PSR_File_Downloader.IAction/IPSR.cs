using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSR_File_Downloader.IAction
{
   public interface IPSR
   {  
       /// <summary>
       /// UI 
       /// </summary>
       public Window window { get; set; }
       /// <summary>
       /// Установить нудный логин/пароль для ПСР
       /// </summary>
       /// <param name="psr"></param>
       void SetLoginPassword(ref PSR psr);
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
       List<Files> Download(Files file, bool twoWeek, PSR psr);
       /// <summary>
       /// Получить список файлов с ПСР
       /// </summary>
       /// <param name="psr"></param>
       /// <returns></returns>
       List<Files> GetListFilesFromPSR( PSR psr);
    }
}
