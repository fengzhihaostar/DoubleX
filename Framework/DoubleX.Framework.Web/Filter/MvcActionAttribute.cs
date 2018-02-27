using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Framework.Web;

namespace DoubleX.Framework.Web.Filter
{
    /// <summary>
    /// MvcAction 执行过滤器
    /// </summary>
    public class MvcActionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 全局执行前过滤
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                var routePath = StringHelper.FormatDefault(MvcHelper.GetRoutePath(filterContext));
                var managePath = StringHelper.FormatDefault(WebHelper.GetManagePath(isRouteFormat: true));
                var memberPath = StringHelper.FormatDefault(WebHelper.GetMemberPath(isRouteFormat: true));

                //管理中心登录判断
                if (routePath.Contains(managePath))
                {
                    var loginUrls = new List<string>(){
                        string.Format("{0}|account|login",managePath),
                        string.Format("{0}|account|signin",managePath)
                    };
                    if (!WebHelper.GetContext().IsEmployeeLogin && !loginUrls.Contains(routePath))
                    {
                        filterContext.HttpContext.Response.Clear();
                        filterContext.Result = new RedirectResult(WebHelper.GetManageUrl(string.Format("/account/login?_ref={0}", UrlsHelper.GetUrl())));
                    }
                }
                //会员中心登录判断
                if (routePath.Contains(memberPath))
                {
                    var loginUrls = new List<string>(){
                        string.Format("{0}|account|login",memberPath),
                        string.Format("{0}|account|regist",memberPath),
                        string.Format("{0}|account|agreement",memberPath),
                        string.Format("{0}|account|forgetpwd",memberPath)
                    };
                    if (!WebHelper.GetContext().IsMemberLogin && !loginUrls.Contains(routePath))
                    {
                        filterContext.HttpContext.Response.Clear();
                        filterContext.Result = new RedirectResult(WebHelper.GetMemberUrl(string.Format("/account/login?_ref={0}", UrlsHelper.GetUrl())));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DefaultException(EnumResultCode.未知异常, ex);
            }
        }

        /// <summary>
        /// 全局执行后过滤
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
             base.OnActionExecuted(filterContext);
        }
    }
}
