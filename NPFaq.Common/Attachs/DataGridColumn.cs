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
    /// Silverlight DataGridColumn 
    /// </summary>
    public class DataGridColumn
    {
        public static Visibility GetVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(VisibilityProperty);
        }

        public static void SetVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(VisibilityProperty, value);
        }

        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(DataGridColumn), new PropertyMetadata(OnVisibilityChanged));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as System.Windows.Controls.DataGridColumn;
            if (c != null)
                c.Visibility = (Visibility)e.NewValue;
        }
    }
}
