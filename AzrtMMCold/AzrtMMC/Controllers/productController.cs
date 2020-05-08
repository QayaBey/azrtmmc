using AzrtMMC.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class productController : Controller
    {
        private readonly AppDbContext db;
        public productController()
        {
            db = new AppDbContext();
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();

        }
        // GET: product
        public async Task<ActionResult> details(int? id)
        {
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            
            if (product == null) return HttpNotFound();
            ViewBag.Pic =await db.Pictures.Where(x => x.Product.Id == id).Select(x => x.ImagePath).ToListAsync();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View(product);
        }
        public async Task<ActionResult> downloadpdf(int? id)
        {
            if (id == null) return HttpNotFound();
            var pdf = await db.Products.FindAsync(id);
            if (String.IsNullOrEmpty(pdf.PdfLink)) return HttpNotFound();
            string pic = pdf.PdfLink;
            string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/product"), pic);
            return File(path, "application/pdf");
        }
    }
}