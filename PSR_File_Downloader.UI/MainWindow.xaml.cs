using Microsoft.Win32;
using PSR_File_Downloader.Actions;
using PSR_File_Downloader.Actions.FileActions;
using PSR_File_Downloader.Actions.PSRActions;
using PSR_File_Downloader.IAction;
using PSR_File_Downloader.Model;
using PSR_File_Downloader.Model.Connects;
using PSR_File_Downloader.UI.Resource;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSR_File_Downloader.UI
{
 
    public partial class MainWindow : Window
    {
        IPSR psractions;
        IFile fileactions;
        public PSR psr;
        Connect connect;
        Wagon wagon;
        bool twoweek;
        string pathfinaly;
        DownloadWindow dowmloadwindow;
        public  MainWindow()
        {
            psractions = new PSRAction();
            InitializeComponent();          
            CultureInfo ci = new CultureInfo("ru-RU");
            twoweek = true;
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
            DateConverter.psr = psr;
            fileactions = new FileActionPSRL(psractions);

          
        }

        private void PSRM_chexbox_Checked(object sender, RoutedEventArgs e)
        {
            psr = new PSRM();
            DateConverter.psr = psr;
            fileactions = new FileActionPSRM(psractions);
          
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
                string mainpath = "";
                string addpath = "";
               
                if (result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length > 0)
                {
                    mainpath = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
                if (result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    addpath = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                }
                if (Directory.Exists(mainpath))
                {
                    pathfinaly = mainpath;
                    Catalogpath_.Text = mainpath;
                    if (Directory.Exists(mainpath + @"\\" + addpath))
                    {
                        txtbundercatalog.Text = addpath;
                        
                       
                    }
                    

                }
            }

        }

        private async void showfile_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                wagon = new Wagon();
                psr.connect = connect;
                wagon.psr = psr;
                wagon.number = WagonNumberCombBox.Text;
                List<Files> files = await Task.Run<List<Files>>(() => { return fileactions.GetListFilesFromPSR(wagon, twoweek); });
                FilesDGV.ItemsSource = files;
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally { this.IsEnabled = true; }
            
           
        }
     
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeArrangeCmBx.SelectedIndex == 0) { twoweek = true; }
            else { twoweek = false; }
        }

        private async void btndownload_Click(object sender, RoutedEventArgs e)
        {
            List<Files> filesnoDownload=null;
            if (txtbundercatalog.Text.Trim() == ""&& chekbxusingundercatalog.IsChecked==true)
            {
                System.Windows.MessageBox.Show("Некорректное имя подпапки!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (chekbxusingundercatalog.IsChecked == true && Directory.Exists(Catalogpath_.Text) && !Directory.Exists(Catalogpath_.Text + @"\\" + txtbundercatalog.Text))
            {
                Directory.CreateDirectory(Catalogpath_.Text + @"\\" + txtbundercatalog.Text);
                pathfinaly = Catalogpath_.Text + @"\\" + txtbundercatalog.Text;
            }
            try
            {
                CancellationTokenSource canceldownload = new CancellationTokenSource();
                if (FilesDGV.SelectedItems.Count > 0)
                {
                    dowmloadwindow = new DownloadWindow(canceldownload);

                    fileactions.prbarIncrement += dowmloadwindow.PrBarIncremntAllfiles;
                    fileactions.prbarmax += dowmloadwindow.PrBarMaxAllfiles;
                    fileactions.prbartext += dowmloadwindow.PrBarTxtAllfiles;

                    fileactions.prbarIncrementOneFile += dowmloadwindow.PrBarIncremntOneFile;
                    fileactions.prbarmax += dowmloadwindow.PrBarMaxOneFile;
                    fileactions.prbarvalueOneFile += dowmloadwindow.PrBarValueOneFile;
                    List<Files> list = FilesDGV.SelectedItems.OfType<Files>().ToList();
                    this.IsEnabled = false;
                    dowmloadwindow.Show();
                    await Task.Run(() => { fileactions.DownloaderFiles(list, wagon, pathfinaly, canceldownload); });
                    dowmloadwindow.Close();
                    filesnoDownload = fileactions.NotDownload(pathfinaly, list).ToList();
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);


            }
            finally
            {
                if (filesnoDownload != null)
                {
                    if (filesnoDownload.Count > 0)
                    {
                        NotDownloadWindow notdownloadwindow = new NotDownloadWindow(filesnoDownload);
                        notdownloadwindow.ShowDialog();
                    }
                }
                this.IsEnabled = true;
            }
        }

        private void Catalogpath__MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog savepath = new FolderBrowserDialog();

            if (savepath.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {

                if (Directory.Exists(savepath.SelectedPath))
                {
                    Catalogpath_.Text = savepath.SelectedPath;
                    if (Directory.Exists(Catalogpath_.Text + @"\\" + txtbundercatalog.Text))
                    {

                        pathfinaly = savepath.SelectedPath + @"\\" + txtbundercatalog.Text;

                    }
                    else 
                    {
                        pathfinaly = savepath.SelectedPath;

                    }
                  
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Directory.Exists(Catalogpath_.Text))
            {
               
                if (Directory.Exists(Catalogpath_.Text + @"\\" + txtbundercatalog.Text))
                {

                    using (StreamWriter fr = new StreamWriter(@"Catalog.txt", false))
                    {
                        fr.WriteLine(Catalogpath_.Text);
                        fr.WriteLine(txtbundercatalog.Text);

                    }
                }
                else
                {
                    using (StreamWriter fr = new StreamWriter(@"Catalog.txt", false))
                    {

                        fr.WriteLine(Catalogpath_.Text);

                    }

                }

            }
        }


    }
}

