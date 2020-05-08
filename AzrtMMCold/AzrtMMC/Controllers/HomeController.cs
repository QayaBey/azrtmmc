using AzrtMMC.Context;
using AzrtMMC.Models;
using AzrtMMC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class homeController : Controller
    {
        private readonly AppDbContext db;
        public homeController()
        {
            db = new AppDbContext();
        }
        
        public ActionResult index()
        {
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
            var blogs = db.Blogs.OrderByDescending(i=>i.Id).Take(4).ToList();
            if (blogs.Count>0)
            {
                ViewBag.DateTime = blogs.FirstOrDefault().CreatedDate.ToShortDateString();
            }
            PartnerBlog model = new PartnerBlog
            {
                Blogs = blogs,
                Partners = db.Partners.ToList()
            };
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(model);
        }

    }
}