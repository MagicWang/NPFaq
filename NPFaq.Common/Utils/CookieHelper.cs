using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace NPFaq.Common.Utils
{
    public class CookieHelper
    {
        public static void SetCookie(string key, string value, DateTime? expireDate = null)
        {
            expireDate = expireDate ?? DateTime.Now + TimeSpan.FromMinutes(15); //有效期为15分钟
            string cookie = string.Format("{0}={1};expires={2}", key, value, expireDate.Value.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }
        public static string GetCookie(string key)
        {
            string[] cookies = HtmlPage.Document.Cookies.Split(';');
            var result = (from c in cookies
                          let keyValues = c.Split('=')
                          where keyValues.Length == 2 && keyValues[0].Trim() == key.Trim()
                          select keyValues[1]).FirstOrDefault();
            return result;
        }

        public static void DeleteCookie(string key)
        {
            DateTime expir = DateTime.UtcNow - TimeSpan.FromDays(1);
            string cookie = string.Format("{0}=;expires={1}", key, expir.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }

        public static bool Exists(string key, string value)
        {
            return HtmlPage.Document.Cookies.Contains(string.Format("{0}={1}", key, value));
        }
    }
}
