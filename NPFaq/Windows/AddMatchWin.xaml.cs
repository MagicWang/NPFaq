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
    /// 新增/修改用户
    /// </summary>
    public partial class AddMatchWin : Common.Controls.BaseWindow
    {
        public AddMatchWin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        public AddMatchWin(WrapMatch match)
            : this()
        {
            (DataContext as DevExpress.Mvvm.ISupportParameter).Parameter = match;
            Title = "编辑比赛";
        }

        private void Close_Raised(object sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
