using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Framework.Web;

namespace DoubleX.Framework.Web.Filter
{
    //资料参考： http://www.cnblogs.com/Showshare/p/exception-handle-with-mvcfilter.html
    // [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]  //类级别
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]    //方法级别
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            base.OnException(filterContext);

            ResultModel result = new ResultModel() { Code = EnumHelper.GetValue(EnumResultCode.未知异常) };
            try
            {
                if (filterContext != null && filterContext.Exception != null)
                {
                    //异步记录日志
                    Log4netHelper.Get(KeyModel.Log.ExceptionName).AsyncWriter(filterContext.Exception.ToString());
                    result = DoubleXHelper.GetResult(filterContext.Exception);
                }
            }
            catch (Exception ex)
            {
                //异步记录日志
                Log4netHelper.Get(KeyModel.Log.ExceptionName).AsyncWriter(ex.ToString());
                result = DoubleXHelper.GetResult(ex);
            }
            finally
            {
                filterContext.Response = WebApiHelper.ToHttpResponseMessage(result);
            }
        }
    }
}
