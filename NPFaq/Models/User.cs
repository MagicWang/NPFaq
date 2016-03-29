using DevExpress.Mvvm;
using NPFaq.Enums;
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
    public class User : MatchServiceReference.user
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

        public Role Role
        {
            get
            {
                Role role = Role.Unknown;
                Enum.TryParse<Role>(this.UserType, out role);
                return role;
            }
        }

        public bool IsAdministrator
        {
            get { return Role == Role.Administrator || Role == Role.SuperAdministrator; }
        }
    }
}
