using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Model
{
    public class PSRM : PSR
    {
        public PSRM()
        {
            this.Type = type.M;
            this.IPpsr = new IPAddress(Encoding.UTF8.GetBytes("10.49.24.240"));
        }

    }
}
