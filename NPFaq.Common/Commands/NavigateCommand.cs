using NPFaq.Common.Utils;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Commands
{
    /// <summary>
    /// 导航到指定uri
    /// </summary>
    public class NavigateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var frame = VisualTreeHelper1.GetChildObject<Frame>(Application.Current.RootVisual);
            if (frame != null && parameter != null)
                frame.Navigate(new Uri(parameter.ToString(), UriKind.Relative));
        }
    }
}
