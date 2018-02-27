using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Newtonsoft.Json;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// WebApi工具类
    /// </summary>
    public class WebApiHelper
    {
        #region Route 路由操作

        /// <summary>
        /// 获取AreaName
        /// </summary>
        public static string GetAreaName(HttpActionContext context)
        {
            var area = context.RequestContext.RouteData.Values["area"];
            if (area != null)
            {
                return area.ToString().ToLower();
            }
            return "";
        }

        /// <summary>
        /// 获取ControllName
        /// </summary>
        public static string GetControllName(HttpActionContext context)
        {
            var controller = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (controller != null)
            {
                return controller.ToString().ToLower();
            }
            return "";
        }

        /// <summary>
        /// 获取ActionName
        /// </summary>
        public static string GetActoin(HttpActionContext context)
        {
            var action = context.ActionDescriptor.ActionName;
            if (action != null)
            {
                return action.ToString().ToLower();
            }
            return "";
        }

        /// <summary>
        /// 获取当前路径路径格式:{areaName}|{controllName}|{actionName}
        /// </summary>
        public static string GetRoutePath(HttpActionContext context)
        {
            return string.Format("{0}|{1}|{2}", GetAreaName(context), GetControllName(context), GetActoin(context));
        }

        #endregion

        #region Action 返回

        /// <summary>
        /// 将结果将返统一返回消息对象(Webapi使用)
        /// </summary>
        public static HttpResponseMessage ToHttpResponseMessage(dynamic obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var jsonStr = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeFormat);
            return new HttpResponseMessage { Content = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json") };
        }

        #endregion
    }

}
