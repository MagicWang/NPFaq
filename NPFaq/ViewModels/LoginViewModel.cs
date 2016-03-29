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
using System.ComponentModel.DataAnnotations;
using System.Windows.Browser;
using NPFaq.Enums;

namespace NPFaq.ViewModels
{
    public class LoginViewModel : ValidationViewModelBase
    {
        public ICommand LoginCommand { get; set; }
        private string userName;
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value, "UserName"); }
        }
        private string password;
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "密码长度4-8位")]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, "Password"); }
        }
        private bool isRemember;

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value, "IsRemember"); }
        }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand<string>(LoginExecute);
            IsRemember = true;
            LoadUser();
        }

        private void LoginExecute(string obj)
        {
            if (IsValid())
                Login(UserName, Password, IsRemember);
        }
        private static System.IO.IsolatedStorage.IsolatedStorageSettings userSettings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;

        public static void Login(string userName, string password, bool flag)
        {
            var client = Utils.ServicesFactory.CreateMatchService();
            client.LoginCompleted += (s, e) =>
            {
                if (e.Result)
                {
                    SaveCookie(userName, password);
                    if (flag)
                        SaveUser(userName, password);
                    else
                        DeleteUser();
                    client.GetUserByNameCompleted += (ss, ee) =>
                    {
                        App.CurrentUser.Name = userName;
                        App.CurrentUser.ID = ee.Result.ID;
                        App.CurrentUser.UserType = ee.Result.UserType;
                        App.CurrentUser.Tel = ee.Result.Tel;
                        App.CurrentUser.Info = ee.Result.Info;
                        App.CurrentUser.IsLogin = true;
                        if (App.CurrentUser.Role==Role.SuperAdministrator|| App.CurrentUser.Role == Role.Administrator)
                            new Common.Commands.NavigateCommand().Execute("/MatchManager");
                        else if (App.CurrentUser.Role == Role.Teacher)
                            new Common.Commands.NavigateCommand().Execute("/TeamManager");
                    };
                    client.GetUserByNameAsync(userName);
                }
                else
                    MessageBox.Show("用户名或密码错误");
            };
            client.LoginAsync(userName, password);
        }

        private static void SaveCookie(string userName, string password)
        {
            Common.Utils.CookieHelper.SetCookie("user", userName);
            Common.Utils.CookieHelper.SetCookie("pwd", password);
        }

        private void LoadUser()
        {
            userSettings.TryGetValue<string>("user", out userName);
            userSettings.TryGetValue<string>("pwd", out password);
        }
        private static void SaveUser(string userName, string password)
        {
            if (userSettings.Contains("user"))
                userSettings["user"] = userName;
            else
                userSettings.Add("user", userName);
            if (userSettings.Contains("pwd"))
                userSettings["pwd"] = password;
            else
                userSettings.Add("pwd", password);
            userSettings.Save();
        }
        private static void DeleteUser()
        {
            if (userSettings.Contains("user"))
                userSettings.Remove("user");
            if (userSettings.Contains("pwd"))
                userSettings.Remove("pwd");
            userSettings.Save();
        }
    }
}
