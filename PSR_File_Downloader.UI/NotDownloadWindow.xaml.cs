using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PSR_File_Downloader.UI
{
    /// <summary>
    /// Логика взаимодействия для NotDownloadWindow.xaml
    /// </summary>
    public partial class NotDownloadWindow : Window
    {
        public NotDownloadWindow(List<Files> files)
        {
         
            InitializeComponent();
            this.DataGFile.ItemsSource= files;
        }
    }
}
