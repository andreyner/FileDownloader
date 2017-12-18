using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSR_File_Downloader.Model;
using System.Windows;
using System.Threading;
namespace PSR_File_Downloader.IAction
{
    public interface IFile
    {
        event Action<int> prbarIncrement;
        event Action<double> prbarmax;
        event Action<string> prbartext;

        event Action<int> prbarIncrementOneFile;
        event Action<double> prbarmaxOneFile;
        event Action<int> prbarvalueOneFile;
        /// <summary>
        /// Загрузка файла с ПСР
        /// </summary>
        /// <param name="goalfiles"> Список файлов,которые нужно загрузить</param>
        /// <param name="twoWeek"> загрузка/не загрузка за последние 2 недели</param>
        /// <param name="psr"> ПСР</param>
        /// <param name="path">место для скачивания</param>
        /// <param name="token">признак отмены скачивания</param>
        void DownloaderFiles(List<Files> goalfiles, Wagon wagon, string path, CancellationTokenSource token);
        /// <summary>
        /// Получить список незагруженных файлов
        /// </summary>
        /// <param name="pathrepositoryOnComputer"> путь к хранилищу файлов на компьютере</param>
        /// <param name="requaredFile">Файлы которые необходимо было загрузить</param>
        /// <returns></returns>
        IEnumerable<Files> NotDownload(string pathrepositoryOnComputer, List<Files> requaredFile);
        /// <summary>
        /// Получить список фалов на ПСР
        /// </summary>
        /// <param name="wagon"> Вагон</param>
        /// <param name="twoWeek"> список за две недели или нет</param>
        /// <returns></returns>
        List<Files> GetListFilesFromPSR(Wagon wagon, bool twoWeek);

    }
}
