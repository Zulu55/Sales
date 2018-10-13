namespace Sales.Backend.Models
{
    using Sales.Domain.Models;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<Sales.Common.Models.Category> Categories { get; set; }
    }
}