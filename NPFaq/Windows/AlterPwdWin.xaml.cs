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
    public partial class AlterPwdWin : Common.Controls.BaseWindow
    {
        public AlterPwdWin()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(oldPwd.Password) || string.IsNullOrEmpty(newPwd.Password) || string.IsNullOrEmpty(rePwd.Password))
            {
                errorMsg = "密码不能为空";
            }
            else if (newPwd.Password != rePwd.Password)
            {
                errorMsg = "两次输入密码不一致";
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                return;
            }
            var client = Utils.ServicesFactory.CreateMatchService();
            client.AlterPasswordCompleted += (ss, eee) =>
            {
                if (eee.Result)
                {
                    MessageBox.Show("修改成功");
                    DialogResult = true;
                }
                else
                    MessageBox.Show("原始密码输入不正确");
            };
            client.AlterPasswordAsync(App.CurrentUser.ID, oldPwd.Password, newPwd.Password);
        }
    }
}
