namespace Sales.Backend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Common.Models;

    public class CategoryView : Category
    {
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}