using AzrtMMC.Context;
using AzrtMMC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class subcategoryController : Controller
    {
        private readonly AppDbContext db;
        public subcategoryController()
        {
            db = new AppDbContext();
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
        }
        // GET: subcategory
        public async Task<ActionResult> details(int? id)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.SubCategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            SubcategoryProduct model = new SubcategoryProduct
            {
                SubCategory = subcategory,
                Products = db.Products.Where(i => i.SubCategoryId == subcategory.Id).ToList()
            };
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(model);
        }
    }
}