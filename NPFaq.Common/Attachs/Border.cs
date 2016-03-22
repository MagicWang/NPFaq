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
    public class Border
    {
        public static Brush GetBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderBrushProperty);
        }

        public static void SetBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderBrushProperty, value);
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(Border), new PropertyMetadata(null, OnBorderBrushPropertyChanged));

        private static void OnBorderBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = AddParent(d);
            if (p != null)
                p.BorderBrush = e.NewValue as Brush;
        }

        private static System.Windows.Controls.Border AddParent(DependencyObject d)
        {
            System.Windows.Controls.Border result = null;
            var p = VisualTreeHelper.GetParent(d);
            if (p is System.Windows.Controls.Border)
            {
                result = (System.Windows.Controls.Border)p;
            }
            else if (p is Grid)
            {
                FrameworkElement f = d as FrameworkElement;
                result = new System.Windows.Controls.Border();
                ((Grid)p).Children.Add(result);
                Grid.SetRow(result, Grid.GetRow(f));
                Grid.SetRowSpan(result, Grid.GetRowSpan(f));
                Grid.SetColumn(result, Grid.GetColumn(f));
                Grid.SetColumnSpan(result, Grid.GetColumnSpan(f));
            }
            else
            {
                throw new InvalidOperationException("暂时只支持Grid");
            }
            return result;
        }

        public static Thickness GetBorderThickness(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(BorderThicknessProperty);
        }

        public static void SetBorderThickness(DependencyObject obj, Thickness value)
        {
            obj.SetValue(BorderThicknessProperty, value);
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(Border), new PropertyMetadata(new Thickness(0), OnBorderThicknessPropertyChanged));

        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = AddParent(d);
            if (p != null)
                p.BorderThickness = (Thickness)e.NewValue;
        }

        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(Border), new PropertyMetadata(new CornerRadius(0), OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = AddParent(d);
            if (p != null)
                p.CornerRadius = (CornerRadius)e.NewValue;
        }
    }
}
