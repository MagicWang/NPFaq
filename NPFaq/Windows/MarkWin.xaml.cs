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

namespace NPFaq.Windows
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public partial class MarkWin : Common.Controls.BaseWindow
    {
        public MarkWin()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
