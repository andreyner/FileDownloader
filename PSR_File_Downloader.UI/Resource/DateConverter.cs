using PSR_File_Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PSR_File_Downloader.UI.Resource
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public  class DateConverter : IValueConverter
    {
        private string _format = "dd-MM-yy";
        public static PSR psr;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (psr is PSRM)
            {
                _format = "dd-MM-yy HH:MM";
            }
            else { _format = "dd-MM-yy HH:MM:ss"; }
            return date.ToString(_format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DateTime.ParseExact((string)value, _format, culture);
        }
    }
}
