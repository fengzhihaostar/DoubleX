using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;

namespace DoubleX.Applaction.Apisite.Areas.Manage.Controllers
{
    public class HomeController : MvcBaseController
    {
        /// <summary>
        /// 管理中心-首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var context = WebHelper.GetContext();
            return View();

        }
    }
}