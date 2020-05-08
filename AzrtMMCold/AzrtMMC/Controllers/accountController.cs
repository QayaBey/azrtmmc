using AzrtMMC.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class accountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public accountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AppIdentityDbContext()));
            var user = userManager.FindByName("azrtmmc");
            if (user == null)
            {
                ApplicationUser newuser = new ApplicationUser
                {
                    Name = "Azrt MMC",
                    Email = "umman@azrtmmc.com",
                    UserName = "azrtmmc"
                };
                userManager.Create(newuser, "azrtmmc555");
            }
        }
        // GET: account
        public ActionResult index()
        {
            return View("login");
        }
        [HttpGet]
        public async Task<ActionResult> login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/partners");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindAsync(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "İstifadəçi adı və yaxud şifrə yanlışdır.");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identity = await userManager.CreateIdentityAsync(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/admin/partners" : returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("login");
        }
    }
}