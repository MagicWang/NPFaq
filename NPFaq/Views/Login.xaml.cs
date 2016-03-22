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
using NPFaq.Models;
using System.Windows.Data;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace NPFaq.Views
{
    /// <summary>
    /// 登录View
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void Pwd_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(Pwd, "HiddenWaterMark", true);
        }

        private void Pwd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pwd.Password))
                VisualStateManager.GoToState(Pwd, "ShowWaterMark", true);
        }

        private void Pwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAutomationPeer bam = new ButtonAutomationPeer(LoginBtn);
                IInvokeProvider iip = bam.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                iip.Invoke();
            }
        }

        private void Pwd_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pwd.Password))
                VisualStateManager.GoToState(Pwd, "ShowWaterMark", true);
            else
                VisualStateManager.GoToState(Pwd, "HiddenWaterMark", true);
        }
    }
}
