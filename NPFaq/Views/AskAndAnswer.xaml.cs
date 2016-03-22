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

namespace NPFaq.Views
{
    /// <summary>
    /// 提交问答View
    /// </summary>
    public partial class AskAndAnswer : Common.Controls.BaseWindow
    {
        public AskAndAnswer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 修改问答
        /// </summary>
        /// <param name="question">选中的问题</param>
        public AskAndAnswer(Question question)
            : this()
        {
            (DataContext as DevExpress.Mvvm.ISupportParameter).Parameter = question;
        }
        /// <summary>
        /// 新增问答
        /// </summary>
        /// <param name="cid">分类ID</param>
        public AskAndAnswer(int cid)
           : this()
        {
            (DataContext as DevExpress.Mvvm.ISupportParameter).Parameter = cid;
        }

        private void Close_Raised(object sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Common.Controls.BaseWindow.Show(new CategoryManager(), "类别管理");
        }
    }
}
