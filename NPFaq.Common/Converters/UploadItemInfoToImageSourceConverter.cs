using NPFaq.Common.Controls;
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
    public class UploadItemInfoToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource result = null;
            if (value != null && value is UploadItemInfo)
            {
                var item = value as UploadItemInfo;
                if (item.Stream != null && (Utils.ImageHelper.IsImageFile(item.Name) || item.State == UploadDataState.Uploaded))
                    result = Utils.ImageHelper.GetThumbnail(item.Stream, 90);
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
