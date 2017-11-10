using PSR_File_Downloader.Actions;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using PSR_File_Downloader.Model.Connects;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSR_File_Downloader.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPSR psractions;
        IFile file;
        PSR psr;
        Connect connect;
        Wagon wagon;
        bool twoweek;
        string pathfinaly;
        DownloadWindow dowmloadwindow;
        public  MainWindow()
        {
          
            InitializeComponent();
            psractions = new PSRAction();
            dowmloadwindow = new DownloadWindow();
            psractions.prbarIncrement += dowmloadwindow.PrBarIncremnt;
            psractions.prbarmax += dowmloadwindow.PrBarMax;
            psractions.prbartext += dowmloadwindow.PrBarTxt;

            file = new FilesAction(this);

            twoweek = true;
        }
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

     

        private void wifichexbox_Checked_1(object sender, RoutedEventArgs e)
        {
            connect = new WI_FI();
            PSRL_LE_chexbox.IsEnabled = false;
            PSRM_chexbox.IsEnabled = false;
        }

        private void Ethernetchbox_Checked(object sender, RoutedEventArgs e)
        {
            connect = new Ethernet();
            PSRL_LE_chexbox.IsEnabled = true;
            PSRM_chexbox.IsEnabled = true;
        }

        private void PSRL_LE_chexbox_Checked(object sender, RoutedEventArgs e)
        {
            psr = new PSRLE();

          
        }

        private void PSRM_chexbox_Checked(object sender, RoutedEventArgs e)
        {
            psr = new PSRM();
          
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (StreamReader fr = new StreamReader(@"WagonNumber.txt"))
            {
                String result;
                result = await fr.ReadToEndAsync();
                string [] nwagon=result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string st in nwagon)
                {
                    WagonNumberCombBox.Items.Add(st);
                    WagonNumberCombBox.Text = nwagon.First();
                }
                
            }
            using (StreamReader fr = new StreamReader(@"Catalog.txt"))
            {
                String result = await fr.ReadToEndAsync();
                string mainpath = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
                string addpath = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (Directory.Exists(mainpath))
                {
                    pathfinaly = mainpath;
                    Catalogpath_.Text = mainpath;
                    if (Directory.Exists(mainpath + @"\\" + addpath))
                    {
                        txtbundercatalog.Text = addpath;
                        pathfinaly = mainpath + @"\\" + addpath;
                    }

                }
            }

        }

        private async void showfile_btn_Click(object sender, RoutedEventArgs e)
        {
            wagon = new Wagon();
            psr.connect = connect;
            wagon.psr = psr;
            wagon.number = WagonNumberCombBox.Text;
            //List<Files> files = await psractions.GetListFilesFromPSR(wagon, twoweek);
            List<Files> files = await Task.Run<List<Files>>(() => { return psractions.GetListFilesFromPSR(wagon, twoweek);});
            FilesDGV.ItemsSource = files;
             

        }
     
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeArrangeCmBx.SelectedIndex == 0) { twoweek = true; }
            else { twoweek = false; }
        }

        private async void btndownload_Click(object sender, RoutedEventArgs e)
        {
            List<Files> list = FilesDGV.SelectedItems.OfType<Files>().ToList();
            this.IsEnabled = false;
            dowmloadwindow.Show();
            await Task.Run(() => { psractions.Download(list, wagon, pathfinaly);});
            dowmloadwindow.Close();
            this.IsEnabled = true;
           
        

        }
    }
}
