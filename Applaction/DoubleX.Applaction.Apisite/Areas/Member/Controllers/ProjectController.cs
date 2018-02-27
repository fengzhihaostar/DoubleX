using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DoubleX.Applaction.Apisite.Areas.Member.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult ProjectInfo()
        {
            return View();
        }

        public ActionResult ProjectAdd()
        {
            return View();
        }
    }
}
