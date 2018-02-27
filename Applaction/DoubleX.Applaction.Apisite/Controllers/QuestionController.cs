using DoubleX.Framework.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoubleX.Applaction.Apisite.Areas.Member.Controllers
{
    public class QuestionController : MvcBaseController
    {
        //
        // GET: /Member/Question/
        public ActionResult CommonQuestion()
        {
            return View();
        }

        public ActionResult TechnologyDocument()
        {
            return View();
        }
    }
}