namespace Sales.Backend.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Models;
    using Helpers;
    using Models;

    [Authorize]
    public class CategoriesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        public async Task<ActionResult> Index()
        {
            return View(await this.db.Categories.OrderBy(c => c.Description).ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Categories";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var category = this.ToCategory(view, pic);
                this.db.Categories.Add(category);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Category ToCategory(CategoryView view, string pic)
        {
            return new Category
            {
                CategoryId = view.CategoryId,
                Description = view.Description,
                ImagePath = pic,
            };
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            var view = this.ToView(category);
            return View(view);
        }

        private CategoryView ToView(Category category)
        {
            return new CategoryView
            {
                CategoryId = category.CategoryId,
                Description = category.Description,
                ImagePath = category.ImagePath,
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.ImagePath;
                var folder = "~/Content/Categories";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var category = this.ToCategory(view, pic);
                this.db.Entry(category).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await this.db.Categories.FindAsync(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var category = await this.db.Categories.FindAsync(id);
            this.db.Categories.Remove(category);
            await this.db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}