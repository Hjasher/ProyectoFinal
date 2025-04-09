using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyberCoreDev.Controllers {
    public class HomeController : Controller 
    {
        public ActionResult Index() 
        {
            return View();
        }

        public ActionResult Almacenamiento()
        {
            return View();
        }
        public ActionResult Usuario()
        {
            return View();
        }
    }
}