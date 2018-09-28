namespace Sales.API.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Common.Models;
    using Domain.Models;

    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        public IQueryable<Category> GetCategories()
        {
            return db.Categories.OrderBy(c => c.Description);
        }
    }
}