using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoubleX.Applaction.Apisite.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("error/404")]
        public ActionResult NotFound()
        {
            return View();
        }
	}
}