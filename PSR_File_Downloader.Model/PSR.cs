using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Model
{
    abstract public class PSR
    {

       public  PSR()
       {
          
       }

      public  Connect connect { get; set; }
      /// <summary>
      /// Логин для устройства
      /// </summary>
      public string Login { get; set; }
      /// <summary>
      /// Пароль для устройства
      /// </summary>
      public string Password { get; set; }
      /// <summary>
      /// IP адрес устройства
      /// </summary>
      public string IPadres { get; set; }

    }
}
