using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Framework.Web;

namespace DoubleX.Framework.Web.Filter
{
    //资料参考： http://www.cnblogs.com/Showshare/p/exception-handle-with-mvcfilter.html
    // [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]  //类级别
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]    //方法级别
    public class MvcExceptionAttribute : HandleErrorAttribute
    {
        //1. 不支持exception记录(事件中)
        //2. 无法捕捉到500之外的http exception
        //3. controller之外抛出的异常无法处理
        //4. ajax调用出现exception时，会将错误页面内容返回(己处理了)
        public override void OnException(ExceptionContext filterContext)
        {
            //如果异常未处理
            if (!filterContext.ExceptionHandled)
            {
                //异步记录日志
                Log4netHelper.Get(KeyModel.Log.ExceptionName).AsyncWriter(filterContext.Exception.ToString());
                //Log4netHelper.Get(KeyModel.Log.ServiceName).Writer("这里是自定义错误信息");

                //返回操作
                if (VerifyHelper.IsAjax(filterContext.HttpContext))
                {
                    //当结果为json时，设置异常已处理（如有单独的Attribute记录日志，这里不需要设置，如设置的true的话就不会再跑记录日志的attribute）
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = MvcHelper.ToJsonResult(DoubleXHelper.GetResult(filterContext.Exception));
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = new RedirectResult(WebHelper.GetErrorUrl(DoubleXHelper.GetResult(filterContext.Exception)));
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}
