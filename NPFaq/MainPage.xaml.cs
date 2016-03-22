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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPFaq
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // 在 Frame 导航之后，请确保选中表示当前页的 HyperlinkButton
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // 如果导航过程中出现错误，则显示错误窗口
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            //ChildWindow errorWin = new ErrorWindow(e.Uri);
            //errorWin.Show();
        }

        private void Link4_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser.IsLogin = false;
            Common.Utils.CookieHelper.DeleteCookie("user");
            Common.Utils.CookieHelper.DeleteCookie("pwd");
        }
    }
}