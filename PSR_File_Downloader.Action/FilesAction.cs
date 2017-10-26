using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using System.Windows;
namespace PSR_File_Downloader.Action
{
    public class FilesAction:IFile
    {
        public FilesAction(Window window)
        {
            this.window = window;
        }
        public List<Files> FilesFilter()
        {
            throw new NotImplementedException();
        }

        public List<Files> DownloaderFile(List<PSR_File_Downloader.Model.Files> files)
        {
            throw new NotImplementedException();
        }

        public List<Files> NotDownload(string pathrepositoryOnComputer, List<PSR_File_Downloader.Model.Files> requaredFile)
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
