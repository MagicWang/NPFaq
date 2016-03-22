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
    [TemplatePart(Name = "PART_CANCEL", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OK", Type = typeof(Button))]
    public class DialogWindow : ChildWindow
    {
        private Button okBtn;
        private Button cancelBtn;
        public DialogWindow()
        {
            DefaultStyleKey = typeof(DialogWindow);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            okBtn = GetTemplateChild("PART_OK") as Button;
            cancelBtn = GetTemplateChild("PART_CANCEL") as Button;
            if (okBtn != null)
                okBtn.Click += (s, e) => DialogResult = true;
            if (cancelBtn != null)
                cancelBtn.Click += (s, e) => DialogResult = false;
        }
        public static void Show(UIElement element, string title = "", EventHandler OnClosed = null)
        {
            DialogWindow dw = new DialogWindow() { Content = element, Title = title };
            if (OnClosed != null)
                dw.Closed += OnClosed;
            dw.Show();
        }
    }
}
