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
    public class AddUserViewModel : ValidationViewModelBase
    {
        public ICommand SubmitCommand { get; set; }
        public InteractionRequest CloseRequest { get; set; }
        private string userName;
        [Required(ErrorMessage = "用户不能为空")]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value, "UserName"); }
        }
        private string password;
        [Required(ErrorMessage = "密码不能为空")]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, "Password"); }
        }
        private string userType;
        [Required(ErrorMessage = "用户类型不能为空")]
        public string UserType
        {
            get { return userType; }
            set { SetProperty(ref userType, value, "UserType"); }
        }
        private int tel;
        public int Tel
        {
            get { return tel; }
            set { SetProperty(ref tel, value, "Tel"); }
        }
        private string info;
        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value, "Info"); }
        }
        public AddUserViewModel()
        {
            SubmitCommand = new DelegateCommand<object>(SubmitExecute);
            CloseRequest = new InteractionRequest();
        }

        protected override void OnParameterChanged(object parameter)
        {
            if (parameter is user)
            {
                user user = parameter as user;
                UserName = user.Name;
                if (user.UserType == "Administrator")
                    UserType = "管理员";
                else if (user.UserType == "Teacher")
                    UserType = "教师";
                Password = "123456";
                Tel = user.Tel.HasValue ? user.Tel.Value : 0;
                Info = user.Info;
            }
        }
        private void SubmitExecute(object obj)
        {
            if (!IsValid())
                return;
            user user = null;
            if (Parameter is user)
            {
                user = Parameter as user;
            }
            else
            {
                user = new user()
                {
                    Name = UserName,
                    Password = Password
                };
            }
            if (UserType == "管理员")
                user.UserType = Role.Administrator.ToString();
            else if (UserType == "教师")
                user.UserType = Role.Teacher.ToString();
            user.Tel = Tel;
            user.Info = Info;
            var client = Utils.ServicesFactory.CreateMatchService();
            client.AddOrUpdateUserCompleted += (s, e) => CloseRequest.Raise();
            client.AddOrUpdateUserAsync(user);
        }
    }
}
