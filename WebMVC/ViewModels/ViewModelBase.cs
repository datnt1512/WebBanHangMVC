using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.ViewModels
{
    public class ViewModelBase
    {
        public const string ErrorMessageRequired = "{0} không được trống";
        public const string ErrorMessageMaxLength = "{0} phải nhỏ hơn {1} ký tự";
        public const string ErrorMessageStringLength = "{0} phải từ {2} tới {1} ký tự";
    }
}