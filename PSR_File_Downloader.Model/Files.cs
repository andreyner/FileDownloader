using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Model
{
    public class Files
    {
        /// <summary>
        /// Исходное имя файла
        /// </summary>
     public string NameSource
        {

            get
            {
                return namefinaly;
            }
            set
            {
                ///релизовать новое имя
            }
        }
     private string namefinaly;
        /// <summary>
        /// Дата изменения файла
        /// </summary>
     public DateTime DateChange { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
     public int size { get; set; }
        /// <summary>
        /// Конечное имя файла
        /// </summary>
     public string NameFinaly { get; set; }

    }
}
