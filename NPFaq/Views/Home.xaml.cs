using NPFaq.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPFaq
{
    /// <summary>
    /// 常见问题View
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAutomationPeer bam = new ButtonAutomationPeer(SearchBtn);
                IInvokeProvider iip = bam.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                iip.Invoke();
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as HomeViewModel;
            if (vm != null)
            {
                foreach (var item in vm.Questions)
                    item.IsChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as HomeViewModel;
            if (vm != null)
            {
                foreach (var item in vm.Questions)
                    item.IsChecked = false;
            }
        }
    }
}