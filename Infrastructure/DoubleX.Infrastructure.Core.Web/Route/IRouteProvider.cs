using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;

namespace DoubleX.Infrastructure.Core.Web
{
    /// <summary>
    /// 自定义路由驱动接口
    /// </summary>
    public interface IRouteProvider
    {
        /// <summary>
        /// MVC路由注册
        /// </summary>
        /// <param name="routes"></param>
        void RegisterMvcRoutes(RouteCollection routes);

        /// <summary>
        /// Api路由注册
        /// </summary>
        /// <param name="routes"></param>
        void RegisterApiRoutes(HttpConfiguration config);

        /// <summary>
        /// 注册顺序(越小越前)
        /// </summary>
        int Priority { get; }
    }
}
