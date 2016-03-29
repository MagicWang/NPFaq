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
    public partial class AddTeamWin : Common.Controls.BaseWindow
    {
        public AddTeamWin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        public AddTeamWin(WrapTeam team)
            : this()
        {
            (DataContext as DevExpress.Mvvm.ISupportParameter).Parameter = team;
            Title = "编辑队伍";
            matchNames.ItemsSource = new List<WrapTeam>() {team };
            matchNames.SelectedIndex = 0;
            matchNames.IsReadOnly = true;
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
