using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Organize;

namespace DoubleX.Applaction.Apisite.Areas.Manage.Controllers
{
    public class OrganizeController : MvcBaseController
    {
        /// <summary>
        /// 页面-职员-列表
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeList()
        {
            return View();
        }

        /// <summary>
        /// 页面-职员-编辑 
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeEdit()
        {
            return View();
        }
    }
}