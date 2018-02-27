using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoubleX.Applaction.Apisite.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VoinceToVoince()
        {
            return View();
        }
        public ActionResult TxtToVoince()
        {
            return View();
        }
    }
}