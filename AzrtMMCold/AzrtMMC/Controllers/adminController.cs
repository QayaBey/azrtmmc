using AzrtMMC.Context;
using AzrtMMC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    [Authorize]
    public class adminController : Controller
    {
        private readonly AppDbContext db;
        public adminController()
        {
            db = new AppDbContext();
        }
        // GET: admin

        public ActionResult index()
        {
            return RedirectToAction("partners");
        }
        //Partners
        public ActionResult partners()
        {
            ViewBag.PartnersActive = "active";
            return View(db.Partners.OrderByDescending(i => i.Id).ToList());
        }
        [HttpGet]
        public ActionResult createpartner()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> createpartner(HttpPostedFileBase image)
        {
            if (image == null)
            {
                TempData["Error"] = "Şəkil hissəsi boş ola bilməz.";
                return View();
            }
            string pic = System.IO.Path.GetFileName(image.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/partner"), pic);
            image.SaveAs(path);
            Partner partner = new Partner
            {
                ImagePath = image.FileName
            };
            db.Partners.Add(partner);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat bazaya yazıldı.";
            return RedirectToAction("partners");
        }

        public async Task<ActionResult> deletepartner(int? id)
        {
            if (id == null) return HttpNotFound();
            var partner = await db.Partners.FindAsync(id);
            if (partner == null) return HttpNotFound();
            db.Partners.Remove(partner);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("partners");
        }

        //Partners end

        //Blogs
        public ActionResult blogs()
        {
            ViewBag.BlogsActive = "active";
            return View(db.Blogs.OrderByDescending(i => i.Id).ToList());
        }

        [HttpGet]
        public ActionResult newblog()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newblog(Blog blog, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/blog"), pic);
                file.SaveAs(path);
                blog.ImagePath = file.FileName;
            }
            if (ModelState.IsValid)
            {
                blog.CreatedDate = DateTime.Now;
                db.Blogs.Add(blog);
                await db.SaveChangesAsync();
                TempData["Success"] = "Blog yaradıldı.";
                return RedirectToAction("blogs");
            }
            if (file == null) TempData["Error"] = "Şəkil boş ola bilməz.\n";
            if (blog.Title == null) TempData["Error"] = "Başlıq boş ola bilməz. \n";
            if (blog.Description == null) TempData["Error"] = "Məzmun boş ola bilməz.\n";
            return View(blog);
        }
        public async Task<ActionResult> deleteblog(int? id)
        {
            if (id == null) return HttpNotFound();
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null) return HttpNotFound();
            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
            TempData["Success"] = "Blog silindi.";
            return RedirectToAction("blogs");
        }

        [HttpGet]
        public async Task<ActionResult> editblog(int? id)
        {
            if (id == null) return HttpNotFound();
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null) return HttpNotFound();
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editblog(int id, Blog blog, HttpPostedFileBase file)
        {
            var myblog = await db.Blogs.FindAsync(id);
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/blog"), pic);
                file.SaveAs(path);
                myblog.ImagePath = file.FileName;
            }
            if (ModelState.IsValid)
            {
                myblog.Description = blog.Description;
                myblog.Title = blog.Title;
                await db.SaveChangesAsync();
                TempData["Success"] = "Blog düzəliş edildi.";
                return RedirectToAction("blogs");
            }
            if (file == null) TempData["Error"] = "Şəkil boş ola bilməz.\n";
            if (blog.Title == null) TempData["Error"] += "Başlıq boş ola bilməz. \n";
            if (blog.Description == null) TempData["Error"] += "Məzmun boş ola bilməz.\n";
            return View(blog);
        }
        //BLOGS END

        //Shops
        public ActionResult shops()
        {
            ViewBag.ShopsActive = "active";
            return View(db.Shops.OrderByDescending(i => i.Id).Include(m => m.Categories).ToList());
        }
        [HttpGet]
        public ActionResult newshop()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newshop(Shop shop, HttpPostedFileBase file)
        {
            if (shop.Name == null)
            {
                TempData["Error"] = "Ad hissəsi boş ola bilməz.";
                return View();
            }
            if (shop.Description == null)
            {
                TempData["Error"] = "Açıqlama hissəsi boş ola bilməz.";
                return View();
            }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/shop"), pic);
                file.SaveAs(path);
                shop.ImagePath = file.FileName;
            }
            db.Shops.Add(shop);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat daxil edildi.";
            return RedirectToAction("shops");
        }
        [HttpGet]
        public async Task<ActionResult> editshop(int? id)
        {
            if (id == null) return HttpNotFound();
            var shop = await db.Shops.FindAsync(id);
            if (shop == null) return HttpNotFound();
            return View(shop);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editshop(int id, Shop shop, HttpPostedFileBase file)
        {
            var myshop = await db.Shops.FindAsync(id);
            if (myshop != null)
            {
                if (shop.Name == null)
                {
                    TempData["Error"] = "Ad hissəsi boş ola bilməz.";
                    return View();
                }
                if (shop.Description == null)
                {
                    TempData["Error"] = "Açıqlama hissəsi boş ola bilməz.";
                    return View();
                }
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/shop"), pic);
                    file.SaveAs(path);
                    myshop.ImagePath = file.FileName;
                }
                myshop.Description = shop.Description;
                myshop.Name = shop.Name;
                await db.SaveChangesAsync();
                TempData["Success"] = "Məlumat düzəliş edildi.";
                return RedirectToAction("shops");
            }
            return HttpNotFound();
        }
        public async Task<ActionResult> deleteshop(int? id)
        {
            if (id == null) return HttpNotFound();
            var shop = await db.Shops.FindAsync(id);
            if (shop == null) return HttpNotFound();
            db.Shops.Remove(shop);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("shops");
        }

        //END OF SHOP -- THE END OF FUCKING WORLD

        //BEGIN CATEGORIES
        public ActionResult categories()
        {
            ViewBag.CategoriesActive = "active";
            return View(db.Categories.OrderByDescending(i => i.Id).Include(i => i.SubCategories).ToList());
        }

        [HttpGet]
        public async Task<ActionResult> newcategory()
        {
            ViewBag.Shops = await db.Shops.OrderBy(i => i.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newcategory(Category category, HttpPostedFileBase file)
        {
            ViewBag.Shops = await db.Shops.OrderBy(i => i.Name).ToListAsync();
            if (category.Name == null)
            {
                TempData["Error"] = "Ad boş ola bilməz.";
                return View();
            }
            if (category.Description == null)
            {
                TempData["Error"] = "Açıqlama boş ola bilməz.";
                return View();
            }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/category"), pic);
                file.SaveAs(path);
                category.ImagePath = file.FileName;
            }
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat əlavə edildi.";
            return RedirectToAction("categories");
        }
        public async Task<ActionResult> deletecategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("categories");
        }

        [HttpGet]
        public async Task<ActionResult> editcategory(int? id)
        {
            ViewBag.Shops = await db.Shops.OrderBy(i => i.Name).ToListAsync();
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editcategory(int id, Category category, HttpPostedFileBase file)
        {
            var mycategory = await db.Categories.FindAsync(id);
            ViewBag.Shops = db.Shops.OrderBy(i => i.Name).ToList();
            if (category.Name == null)
            {
                TempData["Error"] = "Ad boş ola bilməz.";
                return View();
            }
            if (category.Description == null)
            {
                TempData["Error"] = "Açıqlama boş ola bilməz.";
                return View();
            }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/category"), pic);
                file.SaveAs(path);
                mycategory.ImagePath = file.FileName;
            }
            mycategory.Name = category.Name;
            mycategory.Description = category.Description;
            mycategory.ShopId = category.ShopId;
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat düzəliş edildi.";
            return RedirectToAction("categories");
        }

        //the end of fucking world :D END CATEGORIES 

        //Begin SubCategories
        public async Task<ActionResult> subcategories()
        {
            ViewBag.SubCategoriesActive = "active";
            return View(await db.SubCategories.OrderByDescending(i => i.Id).ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> newsubcategory()
        {
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newsubcategory(SubCategory subcategory, HttpPostedFileBase file)
        {
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            if (subcategory.Name == null)
            {
                TempData["Error"] = "Ad boş ola bilməz.";
                return View();
            }
            if (subcategory.Description == null)
            {
                TempData["Error"] = "Açıqlama boş ola bilməz.";
                return View();
            }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/subcategory"), pic);
                file.SaveAs(path);
                subcategory.ImagePath = file.FileName;
            }
            db.SubCategories.Add(subcategory);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat əlavə edildi.";
            return RedirectToAction("subcategories");
        }

        public async Task<ActionResult> deletesubcategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.SubCategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            db.SubCategories.Remove(subcategory);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("subcategories");
        }
        [HttpGet]
        public async Task<ActionResult> editsubcategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.SubCategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            return View(subcategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editsubcategory(int id, SubCategory subcategory, HttpPostedFileBase file)
        {
            var mysubcategory = await db.SubCategories.FindAsync(id);
            if (subcategory.Name == null)
            {
                TempData["Error"] = "Ad boş ola bilməz.";
                return View();
            }
            if (subcategory.Description == null)
            {
                TempData["Error"] = "Açıqlama boş ola bilməz.";
                return View();
            }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/subcategory"), pic);
                file.SaveAs(path);
                mysubcategory.ImagePath = file.FileName;
            }
            mysubcategory.Name = subcategory.Name;
            mysubcategory.Description = subcategory.Description;
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat düzəliş edildi.";
            return RedirectToAction("subcategories");
        }

        //END OF SUBCATEGORIES ---------------


        //BEGIN PRODUCTS
        public async Task<ActionResult> products()
        {
            ViewBag.SubCategories = await db.SubCategories.ToListAsync();
            ViewBag.ProductsActive = "active";
            return View(await db.Products.OrderByDescending(i => i.Id).ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> newproduct()
        {
            ViewBag.SubCategories = await db.SubCategories.OrderBy(i => i.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newproduct(Product product, HttpPostedFileBase file, HttpPostedFileBase pdffile)
        {
            ViewBag.SubCategories = await db.SubCategories.OrderBy(i => i.Name).ToListAsync();
            if (product.Name == null) { TempData["Error"] = "Ad boş ola bilməz."; return View(); }
            if (product.Description == null) { TempData["Error"] = "Açıqlama boş ola bilməz."; return View(); }
            if (product.Property == null) { TempData["Error"] = "Xüsusiyyətlər boş ola bilməz."; return View(); }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/product/"), pic);
                file.SaveAs(path);
                product.ImagePath = file.FileName;
            }
            foreach (HttpPostedFileBase spesificFile in product.OtherImages)
            {
                if (spesificFile != null)
                {
                    var InputFileName = Path.GetFileName(spesificFile.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/wwwroot/product/") + InputFileName);
                    spesificFile.SaveAs(ServerSavePath);
                    db.Pictures.Add(new Picture { Product = product, ImagePath = InputFileName });
                    //await db.SaveChangesAsync();

                }
            }
            if (pdffile != null)
            {
                string pic = System.IO.Path.GetFileName(pdffile.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/product/"), pic);
                pdffile.SaveAs(path);
                product.PdfLink = pdffile.FileName;
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məhsul əlavə edildi.";
            return RedirectToAction("products");
        }


        [HttpGet]
        public async Task<ActionResult> editproduct(int? id)
        {
            ViewBag.SubCategories = await db.SubCategories.OrderBy(i => i.Name).ToListAsync();
            ViewBag.Pic = await db.Pictures.Where(x => x.Product.Id == id).Select(x => x.ImagePath).ToListAsync();
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editproduct(int id, Product product,HttpPostedFileBase file, HttpPostedFileBase pdffile)
        {
            ViewBag.SubCategories = await db.SubCategories.OrderBy(i => i.Name).ToListAsync();
            var myproduct = await db.Products.FindAsync(id);
            if (product.Name == null) { TempData["Error"] = "Ad boş ola bilməz."; return View(); }
            if (product.Description == null) { TempData["Error"] = "Açıqlama boş ola bilməz."; return View(); }
            if (product.Property == null) { TempData["Error"] = "Xüsusiyyətlər boş ola bilməz."; return View(); }
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/product/"), pic);
                file.SaveAs(path);
                myproduct.ImagePath = file.FileName;
            }

            foreach (HttpPostedFileBase spesificFile in product.OtherImages)
            {
                if (spesificFile != null)
                {
                    var InputFileName = Path.GetFileName(spesificFile.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/wwwroot/product/") + InputFileName);
                    spesificFile.SaveAs(ServerSavePath);

                    db.Pictures.Add(new Picture { Product = myproduct , ImagePath = InputFileName });
                    await db.SaveChangesAsync();

                }
            }

            if (pdffile != null)
            {
                string pic = System.IO.Path.GetFileName(pdffile.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/wwwroot/product/"), pic);
                pdffile.SaveAs(path);
                myproduct.PdfLink = pdffile.FileName;
            }
            myproduct.KullanimAlanlari = product.KullanimAlanlari;
            myproduct.Name = product.Name;
            myproduct.Property = product.Property;
            myproduct.Sektorler = product.Sektorler;
            myproduct.SubCategoryId = product.SubCategoryId;
            myproduct.VideoLink = product.VideoLink;
            await db.SaveChangesAsync();
            TempData["Success"] = "Məhsul düzəliş edildi.";
            return RedirectToAction("products");
        }

        public async Task<ActionResult> deleteproduct(int? id)
        {
            if (id == null) return HttpNotFound();
            var picture = await db.Pictures.Where(x => x.Product.Id == id).ToListAsync();
            if (picture == null) return HttpNotFound();
            db.Pictures.RemoveRange(picture);
            await db.SaveChangesAsync();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məhsul silindi.";
            return RedirectToAction("products");
        }

        //END OF THE PRODUCTS

        //BEGIN CONTACTS
        public async Task<ActionResult> contacts()
        {
            return View(await db.Contacts.OrderByDescending(i=>i.Id).ToListAsync());
        }
    }
}