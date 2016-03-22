using DevExpress.Mvvm;
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

namespace NPFaq.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private bool isSearch;
        private ObservableCollection<faq_category> categorys;

        public ObservableCollection<faq_category> Categorys
        {
            get { return categorys; }
            set { SetProperty(ref categorys, value, "Categorys"); }
        }
        private faq_category selectedCategory;

        public faq_category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                SetProperty(ref selectedCategory, value, "SelectedCategory");
                Refresh();
            }
        }
        private PagedCollectionView count;

        public PagedCollectionView Count
        {
            get { return count; }
            set { SetProperty(ref count, value, "Count"); }
        }
        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { SetProperty(ref pageSize, value, "PageSize"); }
        }

        private ObservableCollection<Question> questions;

        public ObservableCollection<Question> Questions
        {
            get { return questions; }
            set { SetProperty(ref questions, value, "Questions"); }
        }
        private Question selectedQuestion;

        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                SetProperty(ref selectedQuestion, value, "SelectedQuestion");
                if (value != null)
                {
                    Questions.ToList().ForEach(l => l.IsSelected = false);
                    value.IsSelected = true;
                }
            }
        }
        public ICommand SearchCommand { get; set; }
        public ICommand ChangePageCommand { get; set; }
        public ICommand ShowAttachCommand { get; set; }
        public ICommand AskCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public HomeViewModel()
        {
            SearchCommand = new DelegateCommand<string>(SearchExecute);
            ChangePageCommand = new DelegateCommand<object>(ChangePageExecute);
            ShowAttachCommand = new DelegateCommand<object>(ShowAttachExecute);
            AskCommand = new DelegateCommand<object>(AskExecute);
            EditCommand = new DelegateCommand<object>(EditExecute);
            DeleteCommand = new DelegateCommand<object>(DeleteExecute);
            PageSize = 10;
            LoadCategory();
        }

        private void DeleteExecute(object obj)
        {
            if (obj is Question)
            {
                //单个删除
                if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var client = Utils.ServicesFactory.CreateTestService();
                    client.DeleteFaqByIDCompleted += (s, e) => Refresh();
                    client.DeleteFaqByIDAsync(new ObservableCollection<int>() { (obj as Question).ID });
                }
            }
            else
            {
                //批量删除
                var checkedFaqs = Questions.Where(l => l.IsChecked).ToList();
                if (checkedFaqs.Count > 0)
                {
                    if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var client = Utils.ServicesFactory.CreateTestService();
                        client.DeleteFaqByIDCompleted += (s, e) => Refresh();
                        client.DeleteFaqByIDAsync(checkedFaqs.Select(l => l.ID).ToObservableCollection());
                    }
                }
                else
                    MessageBox.Show("请先选择要删除的项");
            }
        }
        private void EditExecute(object obj)
        {
            if (SelectedQuestion == null)
            {
                MessageBox.Show("请选择要编辑的问题");
                return;
            }
            var c = new AskAndAnswer(SelectedQuestion.DeepCopy());
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    Refresh();
            };
            c.Show();
        }
        private void AskExecute(object obj)
        {
            var c = new AskAndAnswer(SelectedCategory.ID);
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    Refresh();
            };
            c.Show();
        }

        private void ShowAttachExecute(object obj)
        {
            faq_question q = obj as faq_question;
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetImagesByQuestionIDCompleted += (s, e) =>
            {
                if (e.Result.Count <= 0)
                {
                    MessageBox.Show("无图片附件"); return;
                }
                var c = new NPFaq.Common.Controls.GalleryControl();
                c.ItemsSource = e.Result;
                Common.Controls.BaseWindow.Show(c, "图片预览");
            };
            client.GetImagesByQuestionIDAsync(q.ID);
        }

        private void Refresh()
        {
            if (SelectedCategory == null)
                return;
            isSearch = false;
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetCountOfCategoryCompleted += (s, e) =>
            {
                Count = new PagedCollectionView(Enumerable.Range(1, e.Result));
            };
            client.GetCountOfCategoryAsync(SelectedCategory.ID);
        }
        private void ChangePageExecute(object obj)
        {
            if (isSearch)
                return;
            int page = 0;
            int.TryParse(obj.ToString(), out page);
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetPagedQuestionsCompleted += (s, e) =>
            {
                Questions = e.Result.Select(l => l.CopyTo<Question>()).ToObservableCollection();
            };
            client.GetPagedQuestionsAsync(PageSize, page, SelectedCategory.ID);
        }

        private void LoadCategory()
        {
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetAllCategorysCompleted += (s, e) => { Categorys = e.Result; SelectedCategory = Categorys.FirstOrDefault(); };
            client.GetAllCategorysAsync();
        }

        private void SearchExecute(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return;
            isSearch = true;
            var client = Utils.ServicesFactory.CreateTestService();
            client.SearchFaqsCompleted += (s, e) =>
            {
                Questions = e.Result.Select(l => l.CopyTo<Question>()).ToObservableCollection();
                Count = new PagedCollectionView(Questions);
            };
            client.SearchFaqsAsync(obj);
        }
    }
}
