using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Converters
{
    public class LongToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            long size = 0;
            if (value != null)
                long.TryParse(value.ToString(), out size);
            if (size <= 1024)
                result = size + "B";
            else if (size <= 1024 * 1024)
                result = Math.Round(size / 1024d, 1) + "KB";
            else
                result = Math.Round(size / (1024 * 1024d), 2) + "M";
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
