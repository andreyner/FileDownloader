using System;
using System.Collections.Generic;
using System.ComponentModel;
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
     [DisplayName("Имя файла")]
     public string Name { get; set; }
        /// <summary>
        /// Дата изменения файла
        /// </summary>
     [DisplayName("Дата изменения")]
     public DateTime DateChange { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
     [DisplayName("Размер")]
     public int size { get; set; }


    }
}
