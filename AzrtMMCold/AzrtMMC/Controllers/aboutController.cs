using AzrtMMC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class aboutController : Controller
    {
        private readonly AppDbContext db;
        public aboutController()
        {
            db = new AppDbContext(); 
        }
        // GET: about
        //[ActionName("Qaya")]
        [Route("azrtmmc/haqqimizda")]
        public ActionResult index()
        {
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View("index");
        }
    }
}