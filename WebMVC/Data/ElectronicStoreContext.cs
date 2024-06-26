using System.Data.Entity;
using WebMVC.Models;

namespace WebMVC.Data
{
    public class ElectronicStoreContext : DbContext
    {
        public ElectronicStoreContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        public static ElectronicStoreContext Create()
        {
            return new ElectronicStoreContext();
        }
    }
}