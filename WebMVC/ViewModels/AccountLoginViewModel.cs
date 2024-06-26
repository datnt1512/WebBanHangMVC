using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMVC.ViewModels
{
    public class AccountLoginViewModel : ViewModelBase
    {
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = ErrorMessageRequired)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessageStringLength)]
        public string Password { get; set; }

        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = ErrorMessageRequired)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessageStringLength)]
        public string UserName { get; set; }
    }
}