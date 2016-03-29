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
using System.Linq;
using NPFaq.Common.Extensions;
using System.Collections.Generic;
using NPFaq.Common.Interactivity.InteractionRequest;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using NPFaq.Models;
using NPFaq.MatchServiceReference;
using NPFaq.Enums;

namespace NPFaq.ViewModels
{
    public class AddMatchViewModel : ValidationViewModelBase
    {
        private WrapMatch wrapMatch = null;
        public ICommand SubmitCommand { get; set; }
        public InteractionRequest CloseRequest { get; set; }
        private string matchName;
        [Required(ErrorMessage = "比赛不能为空")]
        public string MatchName
        {
            get { return matchName; }
            set { SetProperty(ref matchName, value, "MatchName"); }
        }
        private string level;
        [Required(ErrorMessage = "比赛级别不能为空")]
        public string Level
        {
            get { return level; }
            set { SetProperty(ref level, value, "Level"); }
        }
        private string sponsor;
        [Required(ErrorMessage = "主办方不能为空")]
        public string Sponsor
        {
            get { return sponsor; }
            set { SetProperty(ref sponsor, value, "Sponsor"); }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value, "Address"); }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value, "Description"); }
        }
        private DateTime startTime;
        [Required(ErrorMessage = "起始时间不能为空")]
        public DateTime StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value, "StartTime"); }
        }
        private DateTime endTime;
        [Required(ErrorMessage = "结束时间不能为空")]
        public DateTime EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, value, "EndTime"); }
        }
        private DateTime startSign;
        [Required(ErrorMessage = "报名起始时间不能为空")]
        public DateTime StartSign
        {
            get { return startSign; }
            set { SetProperty(ref startSign, value, "StartSign"); }
        }
        private DateTime endSign;
        [Required(ErrorMessage = "报名结束时间不能为空")]
        public DateTime EndSign
        {
            get { return endSign; }
            set { SetProperty(ref endSign, value, "EndSign"); }
        }
        private ObservableCollection<Common.Controls.UploadItemInfo> attachs;

        public ObservableCollection<Common.Controls.UploadItemInfo> Attachs
        {
            get { return attachs; }
            set { SetProperty(ref attachs, value, "Attachs"); }
        }

        public bool IsBusy { get; private set; }

        public AddMatchViewModel()
        {
            SubmitCommand = new DelegateCommand<object>(SubmitExecute);
            CloseRequest = new InteractionRequest();
            Attachs = new ObservableCollection<Common.Controls.UploadItemInfo>();
            StartTime = DateTime.Now;
            StartSign = DateTime.Now;
            EndTime = DateTime.Now.AddDays(30);
            EndSign = DateTime.Now.AddDays(10);
        }

        protected override void OnParameterChanged(object parameter)
        {
            if (parameter is WrapMatch)
            {
                wrapMatch = parameter as WrapMatch;
                MatchName = wrapMatch.Match.Name;
                level = wrapMatch.Match.Level;
                Sponsor = wrapMatch.Match.Sponsor;
                Address = wrapMatch.Match.Address;
                Description = wrapMatch.Match.Description;
                StartTime = wrapMatch.Match.StartTime.Value;
                EndTime = wrapMatch.Match.EndTime.Value;
                StartSign = wrapMatch.Match.StartSign.Value;
                EndSign = wrapMatch.Match.EndSign.Value;
                Attachs = wrapMatch.Attaches.Select(l =>
                {
                    var item = new Common.Controls.UploadItemInfo() { Name = l.FileName, State = Common.Controls.UploadDataState.Uploaded };
                    item.Stream = l.Thumbnail == null ? null : new MemoryStream(l.Thumbnail);
                    return item;
                }).ToObservableCollection();
            }
        }
        private void SubmitExecute(object obj)
        {
            if (!IsValid())
                return;
            IsBusy = true;
            if (wrapMatch == null)
            {
                wrapMatch = new WrapMatch();
                //wrapMatch.Match.Status = MatchStatus.Running.ToString();
                wrapMatch.Match.PublisherID = App.CurrentUser.ID;
            }
            wrapMatch.Match.Name = MatchName;
            wrapMatch.Match.Level = level;
            wrapMatch.Match.Sponsor = Sponsor;
            wrapMatch.Match.Address = Address;
            wrapMatch.Match.Description = Description;
            wrapMatch.Match.StartTime = StartTime;
            wrapMatch.Match.EndTime = EndTime;
            wrapMatch.Match.StartSign = StartSign;
            wrapMatch.Match.EndSign = EndSign;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.AddOrUpdateMatchCompleted += (s, e) =>
            {
                wrapMatch.Match.ID = e.Result;
                client.DeleteAttachesByMIDCompleted += (ss, ee) =>
                {
                    var attaches = new ObservableCollection<attach>();
                    Attachs.ToList().ForEach(l => attaches.Add(new attach()
                    {
                        Type = "Match",
                        TypeID = wrapMatch.Match.ID,
                        FileName = l.Name,
                        Path = string.Format("ClientBin/Attach/Matches/{0}/{1}", wrapMatch.Match.ID, l.Name)
                    }));
                    client.AddAttachesAsync(attaches);
                    load = 0;
                    uploadAttachs = Attachs.Where(l => l.State != Common.Controls.UploadDataState.Uploaded).ToList();
                    if (uploadAttachs.Count > 0)
                    {
                        StartUpload();
                    }
                    else
                        CloseRequest.Raise();
                };
                client.DeleteAttachesByMIDAsync(wrapMatch.Match.ID);
            };
            client.AddOrUpdateMatchAsync(wrapMatch.Match);
        }
        private int load = 0;
        private List<Common.Controls.UploadItemInfo> uploadAttachs;
        private void StartUpload()
        {
            WebClient client = new WebClient();
            client.OpenWriteCompleted += Client_OpenWriteCompleted;
            client.WriteStreamClosed += Client_WriteStreamClosed;
            string queryString = string.Format("filePath={0}&fileName={1}", "ClientBin/Attach/Matches/" + wrapMatch.Match.ID, uploadAttachs[load].Name);
            client.OpenWriteAsync(new Uri(App.Root + "UploadFileHandler.ashx?" + queryString), "post", uploadAttachs[load].Stream);
        }

        private void Client_WriteStreamClosed(object sender, WriteStreamClosedEventArgs e)
        {
            if (++load < uploadAttachs.Count)
            {
                StartUpload();
            }
            else
            {
                IsBusy = false;
                CloseRequest.Raise();
            }
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
    }
}
