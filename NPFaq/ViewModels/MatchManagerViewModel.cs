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
using NPFaq.MatchServiceReference;
using NPFaq.Windows;

namespace NPFaq.ViewModels
{
    public class MatchManagerViewModel : ViewModelBase
    {
        private bool isSearch;
        private ObservableCollection<WrapMatch> _matches;
        private PagedCollectionView matches;

        public PagedCollectionView Matches
        {
            get { return matches; }
            set { SetProperty(ref matches, value, "Matches"); }
        }
        private WrapMatch selectedMatch;

        public WrapMatch SelectedMatch
        {
            get { return selectedMatch; }
            set
            {
                SetProperty(ref selectedMatch, value, "SelectedMatch");
            }
        }
        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { SetProperty(ref pageSize, value, "PageSize"); }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public MatchManagerViewModel()
        {
            SearchCommand = new DelegateCommand<string>(SearchExecute);
            AddCommand = new DelegateCommand<object>(AddExecute);
            EditCommand = new DelegateCommand<object>(EditExecute);
            DeleteCommand = new DelegateCommand<object>(DeleteExecute);
            PageSize = 20;
            InitData();
        }

        private void DeleteExecute(object obj)
        {
            if (obj is WrapMatch)
            {
                //单个删除
                if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var client = Utils.ServicesFactory.CreateMatchService();
                    client.DeleteMatchByIDCompleted += (s, e) => InitData();
                    client.DeleteMatchByIDAsync(new ObservableCollection<int>() { (obj as WrapMatch).Match.ID });
                }
            }
            else
            {
                //批量删除
                var checkedFaqs = _matches.Where(l => l.IsChecked).ToList();
                if (checkedFaqs.Count > 0)
                {
                    if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var client = Utils.ServicesFactory.CreateMatchService();
                        client.DeleteMatchByIDCompleted += (s, e) => InitData();
                        client.DeleteMatchByIDAsync(checkedFaqs.Select(l => l.Match.ID).ToObservableCollection());
                    }
                }
                else
                    MessageBox.Show("请先选择要删除的项");
            }
        }
        private void EditExecute(object obj)
        {
            if (SelectedMatch == null)
            {
                MessageBox.Show("请选择要编辑的比赛");
                return;
            }
            var c = new AddMatchWin(SelectedMatch);
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    InitData();
            };
            c.Show();
        }
        private void AddExecute(object obj)
        {
            var c = new AddMatchWin();
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    InitData();
            };
            c.Show();
        }

        private void InitData()
        {
            var client = Utils.ServicesFactory.CreateMatchService();
            client.GetAllMatchesCompleted += (s, e) =>
            {
                _matches = new ObservableCollection<WrapMatch>(e.Result.Select(l => new WrapMatch(l)));
                Matches = new PagedCollectionView(_matches);
            };
            client.GetAllMatchesAsync();
        }

        private void SearchExecute(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return;
            isSearch = true;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.SearchMatchCompleted += (s, e) =>
            {
                _matches = new ObservableCollection<WrapMatch>(e.Result.Select(l => new WrapMatch(l)));
                Matches = new PagedCollectionView(_matches);
            };
            client.SearchMatchAsync(obj);
        }
    }
}
