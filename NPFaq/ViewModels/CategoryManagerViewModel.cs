using NPFaq.TestServiceReference;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using NPFaq.Common.Extensions;
using NPFaq.Views;
using NPFaq.Models;
using DevExpress.Mvvm;

namespace NPFaq.ViewModels
{
    public class CategoryManagerViewModel : ViewModelBase
    {
        private ObservableCollection<faq_category> categorys;

        public ObservableCollection<faq_category> Categorys
        {
            get { return categorys; }
            set { SetProperty(ref categorys, value, "Categorys"); }
        }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public CategoryManagerViewModel()
        {
            AddCommand = new DelegateCommand<object>(AddExecute);
            RemoveCommand = new DelegateCommand<object>(RemoveExecute);
            Refresh();
        }

        private void Refresh()
        {
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetAllCategorysCompleted += (s, e) => Categorys = e.Result;
            client.GetAllCategorysAsync();
        }

        private void RemoveExecute(object obj)
        {
            var category = obj as faq_category;
            if (category != null)
            {
                var client = Utils.ServicesFactory.CreateTestService();
                client.DeleteCategoryCompleted += (s, e) => Refresh();
                client.DeleteCategoryAsync(category.ID);
            }
        }

        private void AddExecute(object obj)
        {
            TextBox tb = new TextBox();
            Common.Controls.DialogWindow.Show(tb, "分类名称", (s, e) =>
            {
                var dw = s as Common.Controls.DialogWindow;
                if (dw.DialogResult == true)
                {
                    var client = Utils.ServicesFactory.CreateTestService();
                    client.AddCategoryCompleted += (ss, ee) => Refresh();
                    client.AddCategoryAsync(tb.Text);
                }
            });
        }
    }
}
