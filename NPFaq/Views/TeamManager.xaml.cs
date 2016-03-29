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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using NPFaq.ViewModels;
using NPFaq.Models;

namespace NPFaq.Views
{
    public partial class TeamManager : Page
    {
        public TeamManager()
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
            var vm = DataContext as TeamManagerViewModel;
            if (vm != null)
            {
                foreach (WrapTeam item in vm.Teams)
                    item.IsChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as TeamManagerViewModel;
            if (vm != null)
            {
                foreach (WrapTeam item in vm.Teams)
                    item.IsChecked = false;
            }
        }
    }
}
