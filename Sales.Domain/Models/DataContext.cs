namespace Sales.Domain.Models
{
    using System.Data.Entity;
    using Common.Models;

    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<Product> Products { get; set; }

        public System.Data.Entity.DbSet<Sales.Common.Models.Category> Categories { get; set; }
    }
}