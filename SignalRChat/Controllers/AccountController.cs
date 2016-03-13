using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SignalRChat.Models;

namespace SignalRChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult join()
        {
            return View();
        }

        [HttpPost]
        public ActionResult join(JoinModel joinModel)
        {

            string userData = joinModel.RoomId;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                joinModel.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true,
                userData,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            // Redirect back to original URL.
            Response.Redirect(FormsAuthentication.GetRedirectUrl(joinModel.Name, true));
            return RedirectToAction("Index", "Home");
        }

        public void addRoom(string roomId)
        {
           

        }
    }
}