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

namespace NPFaq.ViewModels
{
    public class AskAndAnswerViewModel : ValidationViewModelBase
    {
        private Question q = new Models.Question();
        private int categoryID = 0;
        public ICommand SubmitCommand { get; set; }
        public InteractionRequest CloseRequest { get; set; }
        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, "Title"); }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, "IsBusy"); }
        }

        private string question;
        [Required(ErrorMessage = "问题不能为空")]
        public string Question
        {
            get { return question; }
            set { SetProperty(ref question, value, "Question"); }
        }
        private string keyWord;
        [Required(ErrorMessage = "关键字不能为空")]
        public string KeyWord
        {
            get { return keyWord; }
            set { SetProperty(ref keyWord, value, "KeyWord"); }
        }
        private faq_category selectedCategory;
        [Required(ErrorMessage = "类别不能为空")]
        public faq_category SelectedCategory
        {
            get { return selectedCategory; }
            set { SetProperty(ref selectedCategory, value, "SelectedCategory"); }
        }
        private ObservableCollection<faq_category> categorys;

        public ObservableCollection<faq_category> Categorys
        {
            get { return categorys; }
            set { SetProperty(ref categorys, value, "Categorys"); }
        }

        private ObservableCollection<faq_answer> answers;

        public ObservableCollection<faq_answer> Answers
        {
            get { return answers; }
            set { SetProperty(ref answers, value, "Answers"); }
        }
        private ObservableCollection<Common.Controls.UploadItemInfo> attachs;

        public ObservableCollection<Common.Controls.UploadItemInfo> Attachs
        {
            get { return attachs; }
            set { SetProperty(ref attachs, value, "Attachs"); }
        }

        public AskAndAnswerViewModel()
        {
            SubmitCommand = new DelegateCommand<object>(SubmitExecute);
            CloseRequest = new InteractionRequest();
            Answers = new ObservableCollection<faq_answer>() { new faq_answer() };
            Attachs = new ObservableCollection<Common.Controls.UploadItemInfo>();
            var client = Utils.ServicesFactory.CreateTestService();
            client.GetAllCategorysCompleted += (s, e) => { Categorys = e.Result; SelectedCategory = Categorys.FirstOrDefault(l => l.ID == categoryID); };
            client.GetAllCategorysAsync();
            Title = "新增问题";
        }

        protected override void OnParameterChanged(object parameter)
        {
            if (parameter != null && parameter is int)
            {
                categoryID = (int)parameter;
            }
            else if (parameter is Question)
            {
                Title = "编辑问题";
                q = parameter as Question;
                Question = q.Question;
                KeyWord = q.KeyWord;
                categoryID = q.CategoryID ?? 1;
                Answers = q.faq_answer.Count > 0 ? q.faq_answer : Answers;
                Attachs = q.Attachs.Select(l =>
                {
                    var item = new Common.Controls.UploadItemInfo() { Name = l.Path, State = Common.Controls.UploadDataState.Uploaded };
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
            q.Question = Question;
            q.Questioner = App.CurrentUser.Name;
            q.CategoryID = SelectedCategory.ID;
            q.KeyWord = KeyWord;
            q.Time = DateTime.Now;
            q.faq_answer = new ObservableCollection<faq_answer>();
            q.faq_attach = new ObservableCollection<faq_attach>();
            for (int i = 0; i < Answers.Count; i++)
            {
                if (string.IsNullOrEmpty(Answers[i].Answer))
                    continue;
                var answer = new faq_answer()
                {
                    QuestionID = q.ID,
                    Answer = Answers[i].Answer,
                    Answerer = App.CurrentUser.Name,
                    Order = i,
                    Time = DateTime.Now
                };
                q.faq_answer.Add(answer);
            }
            for (int i = 0; i < Attachs.Count; i++)
            {
                var attach = new faq_attach()
                {
                    QuestionID = q.ID,
                    Path = Attachs[i].Name,
                    Answerer = App.CurrentUser.Name,
                    Type = Common.Utils.ImageHelper.IsImageFile(Attachs[i].Name) ? "Image" : "Other",
                    Time = DateTime.Now
                };
                q.faq_attach.Add(attach);
            }

            var client = Utils.ServicesFactory.CreateTestService();
            client.AddOrUpdateQuestionCompleted += (s, ee) =>
            {
                if (ee.Result > 0)
                {
                    q.ID = ee.Result;
                    load = 0;
                    uploadAttachs = Attachs.Where(l => l.State != Common.Controls.UploadDataState.Uploaded).ToList();
                    if (uploadAttachs.Count > 0)
                    {
                        StartUpload();
                    }
                    else
                        CloseRequest.Raise();
                }
                else
                    MessageBox.Show("提交失败");
            };
            client.AddOrUpdateQuestionAsync(q.CopyTo<faq_question>());
        }
        private int load = 0;
        private List<Common.Controls.UploadItemInfo> uploadAttachs;
        private void StartUpload()
        {
            WebClient client = new WebClient();
            client.OpenWriteCompleted += Client_OpenWriteCompleted;
            client.WriteStreamClosed += Client_WriteStreamClosed;
            string queryString = string.Format("filePath={0}&fileName={1}", "ClientBin/Attach/" + q.ID, uploadAttachs[load].Name);
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
