using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Member;

namespace DoubleX.Applaction.Apisite.Areas.Member.Controllers
{
    /// <summary>
    /// 账号控制器
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// 页面-注册
        /// </summary>
        public ActionResult Regist()
        {
            return View();
        }

        /// <summary>
        /// 页面-登录
        /// </summary>
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
            WebHelper.SetMember(null, isClear: true);
            return new RedirectResult(WebHelper.GetMemberUrl("/account/login"));
        }


        /// <summary>
        /// 页面-忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPwd()
        {
            return View();
        }

        /// <summary>
        /// 页面-协议
        /// </summary>
        /// <returns></returns>
        public ActionResult Agreement()
        {
            return View();
        }
    }
}