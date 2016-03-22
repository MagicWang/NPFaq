using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Controls
{
    public class BaseWindow : ChildWindow
    {
        public BaseWindow()
        {
            DefaultStyleKey = typeof(BaseWindow);
        }
        public static void Show(UIElement element, string title = "", EventHandler OnClosed = null)
        {
            BaseWindow bw = new BaseWindow() { Content = element, Title = title };
            if (OnClosed != null)
                bw.Closed += OnClosed;
            bw.Show();
        }
    }
}
