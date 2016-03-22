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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NPFaq.ViewModels
{
    /// <summary>
    /// 带有数据验证的ViewModel基类
    /// </summary>
    public class ValidationViewModelBase : DevExpress.Mvvm.ViewModelBase, System.ComponentModel.IDataErrorInfo
    {
        protected virtual bool IsValid()
        {
            var validationResult = new List<ValidationResult>();
            return Validator.TryValidateObject(this, new ValidationContext(this), validationResult, true);
        }
        public string this[string columnName]
        {
            get
            {
                var value = GetType().GetProperty(columnName).GetValue(this, null);
                var validationResult = new List<ValidationResult>();
                Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = columnName }, validationResult);
                return string.Join(",", validationResult.Select(l => l.ErrorMessage));
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
