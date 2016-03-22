using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NPFaq.Common.Converters
{
    public class BytesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is byte[])
            {
                byte[] bs = value as byte[];
                BitmapImage imageSource = null;
                try
                {
                    using (MemoryStream stream = new MemoryStream(bs))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        BitmapImage b = new BitmapImage();
                        b.SetSource(stream);
                        imageSource = b;
                    }
                }
                catch (System.Exception ex)
                {
                }
                return imageSource;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
