using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Model
{
   public class PSR
    {

       public  PSR()
       {
           //this.IPpsr = new IPAddress(Encoding.UTF8.GetBytes("10.49.24.240"));
       }
       public enum type
       {
           none,
           M,
           LE,
           L
      }
       /// <summary>
       /// Тип ПСР
       /// </summary>
      public type Type { get; set; }
       /// <summary>
       /// IP адрес устройства
       /// </summary>
      public IPAddress IPpsr;
       /// <summary>
       /// Логин для устройства
       /// </summary>
      public string Login { get; set; }
       /// <summary>
       /// Пароль для устройства
       /// </summary>
      public string Password { get; set; }

    }
}
