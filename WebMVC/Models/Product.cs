using System;

namespace WebMVC.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Alias { get; set; }
        public string Description { get; set; }
        public string SrcImage { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

    }
}