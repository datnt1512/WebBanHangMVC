using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMVC.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }
        [DisplayName("Tên loại sản phẩm")]
        [Required(ErrorMessage = "Tên loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên loại không được vượt quá 100 ký tự")]
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}