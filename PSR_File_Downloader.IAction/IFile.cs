using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSR_File_Downloader.Model;
using System.Windows;
namespace PSR_File_Downloader.IAction
{
    public interface IFile
    {
        /// <summary>
        /// UI
        /// </summary>
        Window window { get; set; }
        /// <summary>
        /// Фильтр для нужных файлов
        /// </summary>
        /// <returns></returns>
        List<Files> FilesFilter();
        /// <summary>
        /// Загрузить выбранный список фалов
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        List<Files> DownloaderFile(List<Files> files);
        /// <summary>
        /// Получить список незагруженных файлов
        /// </summary>
        /// <param name="pathrepositoryOnComputer"></param>
        /// <param name="requaredFile"></param>
        /// <returns></returns>
        List<Files> NotDownload(string pathrepositoryOnComputer, List<Files> requaredFile);

    }
}
