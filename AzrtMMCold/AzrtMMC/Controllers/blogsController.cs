using AzrtMMC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class blogsController : Controller
    {
        private readonly AppDbContext db;
        public blogsController()
        {
            db = new AppDbContext();
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
        }
        // GET: blog
        public ActionResult index()
        {
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(db.Blogs.OrderByDescending(i => i.Id).ToList());
        }
        public async Task<ActionResult> details(int? id)
        {
            if (id == null) return HttpNotFound();
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null) return HttpNotFound();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(blog);
        }
    }
}