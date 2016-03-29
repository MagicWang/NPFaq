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
using NPFaq.Common.Controls;
using System.IO;

namespace NPFaq.ViewModels
{
    public class TeamManagerViewModel : ViewModelBase
    {
        private bool isSearch;
        private ObservableCollection<WrapTeam> _teams;
        private PagedCollectionView teams;

        public PagedCollectionView Teams
        {
            get { return teams; }
            set { SetProperty(ref teams, value, "Teams"); }
        }
        private WrapTeam selectedTeam;

        public WrapTeam SelectedTeam
        {
            get { return selectedTeam; }
            set
            {
                SetProperty(ref selectedTeam, value, "SelectedTeam");
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
        public ICommand UploadCommand { get; set; }
        public ICommand ReviewCommand { get; set; }
        public ICommand MarkCommand { get; set; }
        public TeamManagerViewModel()
        {
            SearchCommand = new DelegateCommand<string>(SearchExecute);
            AddCommand = new DelegateCommand<object>(AddExecute);
            EditCommand = new DelegateCommand<object>(EditExecute);
            DeleteCommand = new DelegateCommand<object>(DeleteExecute);
            UploadCommand = new DelegateCommand<object>(UploadExecute);
            ReviewCommand = new DelegateCommand<object>(ReviewExecute);
            MarkCommand = new DelegateCommand<object>(MarkExecute);
            PageSize = 20;
            InitData();
        }
        private void ReviewExecute(object obj)
        {
            var checkedFaqs = _teams.Where(l => l.IsChecked).ToList();
            if (checkedFaqs.Count > 0)
            {
                if (MessageBox.Show("是否通过", "审核", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var client = Utils.ServicesFactory.CreateMatchService();
                    client.ReviewTeamCompleted += (s, e) => MessageBox.Show("审核完成");
                    client.ReviewTeamAsync(checkedFaqs.Select(l => l.Team.ID).ToObservableCollection());
                }
            }
            else
                MessageBox.Show("请先选择要审核的项");
        }
        private void MarkExecute(object obj)
        {
            if (obj is WrapTeam)
            {
                MarkWin win = new MarkWin();
                win.Closed += (s, e) =>
                {
                    var client = Utils.ServicesFactory.CreateMatchService();
                    client.MarkTeamCompleted += (ss, ee) => MessageBox.Show("打分完成");
                    client.MarkTeamAsync((obj as WrapTeam).Team.ID, win.resultTB.Text, win.rankTB.Text);
                };
                win.Show();
            }
        }
        private void UploadExecute(object obj)
        {
            var team = obj as WrapTeam;
            if (team == null || team.Team.ID <= 0)
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var attach = new attach()
                {
                    Type = "Team",
                    TypeID = team.Team.ID,
                    FileName = dialog.File.Name,
                    Path = string.Format("ClientBin/Attach/Teams/{0}/{1}", team.Team.ID, dialog.File.Name)
                };
                var client = Utils.ServicesFactory.CreateMatchService();
                client.AddAttachesAsync(new ObservableCollection<attach>() { attach });
                var item = new UploadItemInfo(dialog.File.Name, dialog.File.OpenRead());
                StartUpload(team.Team.ID, item);
            }
        }
        private void StartUpload(int teamID, UploadItemInfo item)
        {
            WebClient client = new WebClient();
            client.OpenWriteCompleted += Client_OpenWriteCompleted;
            client.WriteStreamClosed += Client_WriteStreamClosed;
            string queryString = string.Format("filePath={0}&fileName={1}", "ClientBin/Attach/Teams/" + teamID, item.Name);
            client.OpenWriteAsync(new Uri(App.Root + "UploadFileHandler.ashx?" + queryString), "post", item.Stream);
        }

        private void Client_WriteStreamClosed(object sender, WriteStreamClosedEventArgs e)
        {
            InitData();
            MessageBox.Show("上传成功");
        }

        private void Client_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            //将图片数据流发送到服务器上 e.UserState即为需要上传的客户端的流
            Stream clientStream = e.UserState as Stream;
            clientStream.Position = 0;
            Stream serverStream = e.Result;
            byte[] buffer = new byte[4096];
            int readcount = 0;
            //将需要上传的流读取到指定的字节数组中
            while ((readcount = clientStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                //将指定的字节数组写入到目标地址的流
                serverStream.Write(buffer, 0, readcount);
            }
            serverStream.Close();
            clientStream.Close();
        }
        private void DeleteExecute(object obj)
        {
            if (obj is WrapTeam)
            {
                //单个删除
                if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var client = Utils.ServicesFactory.CreateMatchService();
                    client.DeleteTeamByIDCompleted += (s, e) => InitData();
                    client.DeleteTeamByIDAsync(new ObservableCollection<int>() { (obj as WrapTeam).Team.ID });
                }
            }
            else
            {
                //批量删除
                var checkedFaqs = _teams.Where(l => l.IsChecked).ToList();
                if (checkedFaqs.Count > 0)
                {
                    if (MessageBox.Show("是否删除", "删除", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var client = Utils.ServicesFactory.CreateMatchService();
                        client.DeleteTeamByIDCompleted += (s, e) => InitData();
                        client.DeleteTeamByIDAsync(checkedFaqs.Select(l => l.Team.ID).ToObservableCollection());
                    }
                }
                else
                    MessageBox.Show("请先选择要删除的项");
            }
        }
        private void EditExecute(object obj)
        {
            if (SelectedTeam == null)
            {
                MessageBox.Show("请选择要编辑的队伍");
                return;
            }
            var c = new AddTeamWin(SelectedTeam);
            c.Closed += (s, e) =>
            {
                if (c.DialogResult == true)
                    InitData();
            };
            c.Show();
        }
        private void AddExecute(object obj)
        {
            var c = new AddTeamWin();
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
            client.GetAllTeamsCompleted += (s, e) =>
            {
                if (!App.CurrentUser.IsAdministrator)
                    _teams = new ObservableCollection<WrapTeam>(e.Result.Where(l => l.TeacherID == App.CurrentUser.ID).Select(l => new WrapTeam(l)));
                else
                    _teams = new ObservableCollection<WrapTeam>(e.Result.Select(l => new WrapTeam(l)));
                Teams = new PagedCollectionView(_teams);
            };
            client.GetAllTeamsAsync();
        }

        private void SearchExecute(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return;
            isSearch = true;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.SearchTeamCompleted += (s, e) =>
            {
                if (!App.CurrentUser.IsAdministrator)
                    _teams = new ObservableCollection<WrapTeam>(e.Result.Where(l => l.TeacherID == App.CurrentUser.ID).Select(l => new WrapTeam(l)));
                else
                    _teams = new ObservableCollection<WrapTeam>(e.Result.Select(l => new WrapTeam(l)));
                Teams = new PagedCollectionView(_teams);
            };
            client.SearchTeamAsync(obj);
        }
    }
}
