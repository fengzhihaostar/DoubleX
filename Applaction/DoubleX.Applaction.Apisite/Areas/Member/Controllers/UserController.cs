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
    /// 用户控制器
    /// </summary>
    public class UserController : MvcBaseController
    {
        protected IMemberService memberService;

        public UserController(IMemberService iMemberService)
        {
            memberService = iMemberService;
        }

        /// <summary>
        /// 页面-信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// 页面-修改密码
        /// </summary>
        public ActionResult EditPwd()
        {
            return View();
        }

        /// <summary>
        /// 页面-账号安全
        /// </summary>
        /// <returns></returns>
        public ActionResult Security(RequestModel request) {
            var memberDetail = memberService.GetDetail(WebHelper.GetMemberId(request));
            if (VerifyHelper.IsEmpty(memberDetail))
                throw new MessageException(EnumMessageCode.信息错误);

            return View(memberDetail);
        }

        /// <summary>
        /// 页面-手机绑定
        /// </summary>
        public ActionResult BindMobile(RequestModel request)
        {
            var memberDetail = memberService.GetDetail(WebHelper.GetMemberId(request));
            if (VerifyHelper.IsEmpty(memberDetail))
                throw new MessageException(EnumMessageCode.信息错误);

            if (memberDetail.MobileIsVerify) {
                return Redirect(WebHelper.GetMemberUrl("/user/security"));
            }
            return View(memberDetail);
        }

        /// <summary>
        /// 页面-邮箱绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult BindEmail(RequestModel request)
        {
            var memberDetail = memberService.GetDetail(WebHelper.GetMemberId(request));
            if (VerifyHelper.IsEmpty(memberDetail))
                throw new MessageException(EnumMessageCode.信息错误);

            if (memberDetail.EmailIsVerify)
            {
                return Redirect(WebHelper.GetMemberUrl("/user/security"));
            }
            return View(memberDetail);
        }
    }
}