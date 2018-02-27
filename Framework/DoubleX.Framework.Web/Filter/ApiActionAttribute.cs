
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Framework.Web.Filter
{
    /// <summary>
    /// ApiAction执行过滤器
    /// </summary>
    public class ApiActionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 全局执行前过滤
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                actionContext.Response = WebApiHelper.ToHttpResponseMessage(ex);
            }
        }

        /// <summary>
        /// 全局执行后过滤
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
