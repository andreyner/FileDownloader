using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSR_File_Downloader.Actions.Helper
{
    public class FileComparer:IEqualityComparer<Files>
    {


        public bool Equals(Files x, Files y)
        {
            if (x.Name == y.Name)
            { 
                return true;
            }
            else return false;
        }

        public int GetHashCode(Files obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
