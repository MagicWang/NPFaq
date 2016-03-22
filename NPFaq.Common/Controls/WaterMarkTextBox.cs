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

namespace NPFaq.Common.Controls
{
    public class WaterMarkTextBox : TextBox
    {
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WaterMarkTextBox), new PropertyMetadata(string.Empty));

        public WaterMarkTextBox()
        {
            this.DefaultStyleKey = typeof(WaterMarkTextBox);
            Loaded += WaterMarkTextBox_Loaded;
        }

        private void WaterMarkTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
                VisualStateManager.GoToState(this, "ShowWaterMark", true);
            else
                VisualStateManager.GoToState(this, "HiddenWaterMark", true);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            VisualStateManager.GoToState(this, "HiddenWaterMark", true);
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (string.IsNullOrEmpty(this.Text))
                VisualStateManager.GoToState(this, "ShowWaterMark", true);
        }
        
    }
}
