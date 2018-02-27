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
    /// <summary>
    /// 账号相关控制器
    /// </summary>
    public class AccountController : MvcBaseController
    {
        protected readonly IEmployeeService employeeService;

        public AccountController(IEmployeeService iEmployeeService)
        {
            employeeService = iEmployeeService;
        }

        /// <summary>
        /// 页面-登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 页面-退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            WebHelper.SetEmployee(null, isClear: true);
            return new RedirectResult(WebHelper.GetManageUrl("/account/login"));
        }
    }
}