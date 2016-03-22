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

namespace NPFaq.Common.Attachs
{
    /// <summary>
    /// Silverlight鼠标事件帮助类
    /// </summary>
    public class Mouse
    {
        #region 模拟鼠标双击事件

        #region MouseDoubleClick

        public static MouseButtonEventHandler GetMouseDoubleClick(DependencyObject obj)
        {
            return (MouseButtonEventHandler)obj.GetValue(MouseDoubleClickProperty);
        }

        public static void SetMouseDoubleClick(DependencyObject obj, MouseButtonEventHandler value)
        {
            obj.SetValue(MouseDoubleClickProperty, value);
        }

        public static readonly DependencyProperty MouseDoubleClickProperty = DependencyProperty.RegisterAttached(
            "MouseDoubleClick",
            typeof(MouseButtonEventHandler),
            typeof(Mouse),
            new PropertyMetadata(new PropertyChangedCallback(OnMouseDoubleClickChanged)));

        #endregion

        #region MouseDoubleClickCommand

        public static ICommand GetMouseDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MouseDoubleClickCommandProperty);
        }

        public static void SetMouseDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MouseDoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("MouseDoubleClickCommand", typeof(ICommand), typeof(Mouse), new PropertyMetadata(OnMouseDoubleClickChanged));

        #endregion

        private static DateTime? _lastClickTime = null;
        private const int MaxClickInterval = 200;//ms

        private static void OnMouseDoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = obj as UIElement;
            if (element != null)
                element.MouseLeftButtonUp += element_MouseLeftButtonUp;
        }

        /// <summary>
        /// 通过检测两次鼠标单机的间隔来模拟鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //进入双击事件
            if (_lastClickTime.HasValue && DateTime.Now.Subtract(_lastClickTime.Value).Milliseconds <= MaxClickInterval)
            {
                //触发事件
                MouseButtonEventHandler handler = (sender as UIElement).GetValue(MouseDoubleClickProperty) as MouseButtonEventHandler;
                if (handler != null)
                {
                    handler(sender, e);
                }
                ICommand command = (sender as UIElement).GetValue(MouseDoubleClickCommandProperty) as ICommand;
                if (command != null)
                {
                    if (command.CanExecute(sender))
                    {
                        command.Execute(sender);
                    }
                }
                //重新计时
                _lastClickTime = null;
            }
            else
            {
                _lastClickTime = DateTime.Now;
            }
        }

        #endregion
    }
}
