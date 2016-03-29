using NPFaq.Enums;
using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Converters
{
    public class FuncVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (App.CurrentUser == null)
                return Visibility.Collapsed;
            int param = 0;
            int.TryParse(parameter.ToString(), out param);
            if (param == 0)
            {
                if (App.CurrentUser.Role == Role.SuperAdministrator || App.CurrentUser.Role == Role.Administrator || App.CurrentUser.Role == Role.Teacher)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (param == 1)
            {
                if (App.CurrentUser.Role == Role.SuperAdministrator || App.CurrentUser.Role == Role.Administrator || App.CurrentUser.Role == Role.Teacher)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else if (param == 2)
            {
                if (App.CurrentUser.Role == Role.SuperAdministrator || App.CurrentUser.Role == Role.Administrator)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
