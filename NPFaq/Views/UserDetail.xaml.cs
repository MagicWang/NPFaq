using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Text.RegularExpressions;
using NPFaq.ViewModels;
using System.Windows.Data;
using NPFaq.Models;
using NPFaq.MatchServiceReference;

namespace NPFaq.Views
{
    /// <summary>
    /// 用户详细信息
    /// </summary>
    public partial class UserDetail : Page
    {
        public UserDetail()
        {
            InitializeComponent();
        }
    }
}
