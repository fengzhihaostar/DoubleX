using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// MVC框架辅助类
    /// </summary>
    public class MvcHelper
    {  
        #region Route 路由操作

        /// <summary>
        /// 获取区域名称
        /// </summary>
        /// <param name="routeData">路由</param>
        /// <returns>区域名称</returns>
        public static string GetAreaName(RouteData routeData)
        {
            object result;
            if (routeData.DataTokens.TryGetValue("area", out result))
            {
                return result == null ? "" : result.ToString().ToLower();
            }
            return GetAreaName(routeData.Route).ToLower();
        }

        /// <summary>
        /// 获取区域名称
        /// </summary>
        /// <param name="routeData">路由</param>
        /// <returns>区域名称</returns>
        public static string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null && !string.IsNullOrWhiteSpace(area.Area))
            {
                return area.Area.ToLower();
            }
            var route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return route2.DataTokens["area"] == null ? "" : route2.DataTokens["area"].ToString().ToLower();
            }
            return "";
        }


        /// <summary>
        /// 获取控制器名称
        /// </summary>
        /// <param name="context">控制器上下文</param>
        /// <returns>控制器名称</returns>
        public static string GetControllName(ControllerContext context)
        {
            if (context == null)
                return "";
            return GetControllName(context.RouteData);
        }

        /// <summary>
        /// 获取控制器名称
        /// </summary>
        /// <param name="routeData">路由数据</param>
        /// <returns>控制器名称</returns>
        public static string GetControllName(RouteData routeData)
        {
            if (routeData != null && routeData.Values != null && routeData.Values.Count > 0)
                return routeData.Values["controller"].ToString().ToLower();
            return "";
        }


        /// <summary>
        /// 获取Action名称
        /// </summary>
        /// <param name="context">控制器上下文</param>
        /// <returns>Action名称</returns>
        public static string GeActionName(ControllerContext context)
        {
            if (context == null)
                return "";
            return GeActionName(context.RouteData);
        }

        /// <summary>
        /// 获取Action名称
        /// </summary>
        /// <param name="routeData">路由数据</param>
        /// <returns>Action名称</returns>
        public static string GeActionName(RouteData routeData)
        {
            if (routeData != null && routeData.Values != null && routeData.Values.Count > 0)
                return routeData.Values["action"].ToString().ToLower();
            return "";
        }


        /// <summary>
        /// 获取路由路径
        /// </summary>
        /// <param name="context">控制器上下文</param>
        /// <returns></returns>
        public static string GetRoutePath(ControllerContext context)
        {
            if (context == null)
                return "";
            return GetRoutePath(context.RouteData);
        }

        /// <summary>
        /// 获取路由路径
        /// </summary>
        /// <param name="routeData">路由数据</param>
        /// <returns></returns>
        public static string GetRoutePath(RouteData routeData)
        {
            if (routeData == null)
                return "";
            var path = string.Format("|{0}|{1}|{2}", GetAreaName(routeData), GetControllName(routeData), GeActionName(routeData));
            while (path.IndexOf("||") > -1) {
                path = path.Replace("||", "|");
            }
            if (path == "||")
            {
                path = "";
            }
            return path.ToLower();
        }

        #endregion

        #region Result 返回操作

        /// <summary>
        /// MVC返回JSON
        /// </summary>
        public static JsonResult ToJsonResult(dynamic data = null, Encoding contentEncoding = null, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, string format = null)
        {
            if (contentEncoding == null)
            {
                contentEncoding = Encoding.UTF8;
            }
            if (format == null)
            {
                format = "yyyy-MM-dd HH:mm:ss";
            }
            if (data == null)
            {
                data = new object { };
            }
            return new MvcJSONResultFormat
            {
                Data = data,
                ContentType = "application/json",
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                DateTimeFormatStr = format
            };
        }

        /// <summary>
        /// MVC返回下载文件
        /// </summary>
        public static FileContentResult ToFileResult(byte[] data, string fileName)
        {
            //火狐浏览器不需将中文文件名进行编码格式转换
            if (HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower().IndexOf("firefox") == -1)
            {
                fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            }
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            return new FileContentResult(data, "application/octet-stream");
        }

        #endregion

    }
}
