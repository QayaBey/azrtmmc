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
    public class shopController : Controller
    {
        private readonly AppDbContext db;
        public shopController()
        {
            db = new AppDbContext();
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
        }
        // GET: products
        public ActionResult index()
        {
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(db.Shops.OrderByDescending(i=>i.Id).ToList());
        }
        //[ActionName("partner")]
        //[Route("Brends/partner")]
        public async Task<ActionResult> details(int? id)
        {
            if (id == null) return HttpNotFound();
            var shop = await db.Shops.FindAsync(id);
            if (shop == null) return HttpNotFound();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            ShopCategory model = new ShopCategory
            {
                Shop = shop,
                Categories = db.Categories.Where(i => i.ShopId == shop.Id).ToList()
            };
            return View("details",model);
        }
    }
}