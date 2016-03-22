using DevExpress.Mvvm;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Models
{
    public class User : TestServiceReference.faq_user
    {
        private bool isLogin;

        public bool IsLogin
        {
            get { return isLogin; }
            set
            {
                if (isLogin != value)
                {
                    isLogin = value;
                    RaisePropertyChanged("IsLogin");
                }
            }
        }
    }
}
