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
    public class UserManagerViewModel : ViewModelBase
    {
        private bool isSearch;
        private ObservableCollection<user> _users;
        private PagedCollectionView users;

        public PagedCollectionView Users
        {
            get { return users; }
            set { SetProperty(ref users, value, "Users"); }
        }
        private user selectedUser;

        public user SelectedUser
        {
            get { return selectedUser; }
            set
            {
                SetProperty(ref selectedUser, value, "SelectedUser");
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
        public UserManagerViewModel()
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
            if (SelectedUser == null)
            {
                MessageBox.Show("请选择要删除的用户");
                return;
            }
            if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var client = Utils.ServicesFactory.CreateMatchService();
                client.DeleteUserByIDCompleted += (s, e) => InitData();
                client.DeleteUserByIDAsync(new ObservableCollection<int>() { SelectedUser.ID });
            }
            //if (obj is user)
            //{
            //    //单个删除
            //    if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //    {
            //        var client = Utils.ServicesFactory.CreateMatchService();
            //        client.DeleteFaqByIDCompleted += (s, e) => Refresh();
            //        client.DeleteFaqByIDAsync(new ObservableCollection<int>() { (obj as Question).ID });
            //    }
            //}
            //else
            //{
            //    //批量删除
            //    var checkedFaqs = Questions.Where(l => l.IsChecked).ToList();
            //    if (checkedFaqs.Count > 0)
            //    {
            //        if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //        {
            //            var client = Utils.ServicesFactory.CreateTestService();
            //            client.DeleteFaqByIDCompleted += (s, e) => Refresh();
            //            client.DeleteFaqByIDAsync(checkedFaqs.Select(l => l.ID).ToObservableCollection());
            //        }
            //    }
            //    else
            //        MessageBox.Show("请先选择要删除的项");
            //}
        }
        private void EditExecute(object obj)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("请选择要编辑的用户");
                return;
            }
            var c = new AddUserWin(SelectedUser);
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    InitData();
            };
            c.Show();
        }
        private void AddExecute(object obj)
        {
            var c = new AddUserWin();
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
            client.GetAllUsersCompleted += (s, e) =>
            {
                var user = e.Result.FirstOrDefault(l => l.Name == "admin");
                if (user != null)
                    e.Result.Remove(user);
                _users = e.Result;
                Users = new PagedCollectionView(_users);
            };
            client.GetAllUsersAsync();
        }

        private void SearchExecute(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return;
            isSearch = true;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.SearchUserCompleted += (s, e) =>
            {
                var user = e.Result.FirstOrDefault(l => l.Name == "admin");
                if (user != null)
                    e.Result.Remove(user);
                _users = e.Result;
                Users = new PagedCollectionView(_users);
            };
            client.SearchUserAsync(obj);
        }
    }
}
