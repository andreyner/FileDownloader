using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSR_File_Downloader.UI;
using System.Windows;
using System.Net;
using System.IO;

namespace PSR_File_Downloader.Action
{
   public  class PSRAction:IPSR
    {


   
     
       public PSRAction(Window window)
       {
           this.window = window;
       }
    
        public void SetLoginPassword(ref PSR psr)
        {
            throw new NotImplementedException();
        }

    
        public void ChekConnect(PSR psr)
        {
            throw new NotImplementedException();
        }
    
        public List<Files> Download(Files file, bool twoWeek, PSR psr)
        {

            throw new NotImplementedException();
        }
    
        public List<Files> GetListFilesFromPSR(PSR psr)
        {
            throw new NotImplementedException();
        }

        public Window window
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
