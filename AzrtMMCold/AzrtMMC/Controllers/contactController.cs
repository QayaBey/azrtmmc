using AzrtMMC.Context;
using AzrtMMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Controllers
{
    public class contactController : Controller
    {
        private readonly AppDbContext db;
        public contactController()
        {
            db = new AppDbContext();
        }
        // GET: contact
        public ActionResult index()
        {
            //Send to Layout
            ViewBag.Shops = db.Shops.OrderByDescending(i => i.Id).ToList();
            //Send to route
            ViewBag.RouteController = RouteData.Values["controller"];
            ViewBag.RouteAction = RouteData.Values["action"];
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> sendmessage(Contact model)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(model);
                await db.SaveChangesAsync();
                TempData["Success"] = "Mesajınız uğurla göndərildi.";
            }
            return Redirect("/contact");
        }

        [NonAction]
        public async Task SendEmailAsync(string toEmailAddress, string emailSubject, string emailMessage)
        {
            SmtpClient client = new SmtpClient("HereAreMyMailServer", 11);
            MailAddress from = new MailAddress("website@azrtmmc.com", "My Name", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(toEmailAddress);
            var message = new MailMessage();
            message.To.Add(toEmailAddress);
            message.Subject = emailSubject;
            message.Body = emailMessage;
            await client.SendMailAsync(message);
        }
    }
}