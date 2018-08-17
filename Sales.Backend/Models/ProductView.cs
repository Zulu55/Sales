namespace Sales.Backend.Models
{
    using System.Web;
    using Common.Models;

    public class ProductView : Product 
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}