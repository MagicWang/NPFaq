using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NPFaq.Common.Utils
{
    public class ImageHelper
    {
        /// <summary>
        /// 根据文件后缀后判断是否是图片文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
                return false;
            Regex r = new Regex(@"(\.jpg|\.jpeg|\.png)", RegexOptions.IgnoreCase);
            return r.IsMatch(ext);
        }
        public static ImageSource GetThumbnail(Stream stream, int width)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            Image image = new Image();
            image.Source = bitmap;

            double cx = width;
            double cy = bitmap.PixelHeight * (cx / bitmap.PixelWidth);

            double scaleX = cx / bitmap.PixelWidth;
            double scaleY = cy / bitmap.PixelHeight;

            WriteableBitmap thumb = new WriteableBitmap((int)cx, (int)cy);
            thumb.Render(image, new ScaleTransform() { ScaleX = scaleX, ScaleY = scaleY });
            thumb.Invalidate();

            return thumb;
        }
    }
}
