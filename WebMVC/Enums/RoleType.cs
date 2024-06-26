using System.ComponentModel.DataAnnotations;

namespace WebMVC.Enums
{
    public enum RoleType
    {
        [Display(Name = "Quản trị viên")]
        Admin = 3,
        [Display(Name = "Người dùng")]
        User = 0,
    }
}