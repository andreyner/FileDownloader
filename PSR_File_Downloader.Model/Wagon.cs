using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Model
{
    public class Wagon
    {
        public Wagon(string number,PSR psr)
        {
            this.number = number;
            this.psr = psr;
        }
        /// <summary>
        /// номер вагона
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// ПСР вагона
        /// </summary>
        public  PSR psr {get; set;}

    }
}
