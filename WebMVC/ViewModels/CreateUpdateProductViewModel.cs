using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.ViewModels
{
    public class CreateUpdateProductViewModel : ViewModelBase
    {
        public int? ProductId { get; set; }
        [Display(Name = "Tài sản phẩm")]
        [Required(ErrorMessage = ErrorMessageRequired)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = ErrorMessageStringLength)]
        public string ProductName { get; set; }

        public string Alias { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [AllowHtml]
        [Required(ErrorMessage = ErrorMessageRequired)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = ErrorMessageStringLength)]
        public string Description { get; set; }
        public string SrcImage { get; set; }

        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = ErrorMessageRequired)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải là một giá trị dương.")]
        public decimal Price { get; set; }

        [Display(Name = "Loại sản phẩm")]
        [Required(ErrorMessage = ErrorMessageRequired)]
        public int ProductCategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}