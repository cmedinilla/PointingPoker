using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string roomID)
        {
            if (string.IsNullOrEmpty(roomID))
            {
                return View();
            }
            else
            {
                ViewBag.roomId = roomID;
               return View("~/Views/Home/Chat.cshtml");
            }
        }
    }
}