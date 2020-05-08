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
    public class categoryController : Controller
    {
        private readonly AppDbContext db;
        public categoryController()
        {
            db = new AppDbContext();
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
        }
        // GET: category
       
        public async Task<ActionResult> details(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            CategorySubcategory model = new CategorySubcategory
            {
                Category = category,
                SubCategories = db.SubCategories.Where(i => i.CategoryId == category.Id).ToList()
            };
            return View(model);
        }
    }
}