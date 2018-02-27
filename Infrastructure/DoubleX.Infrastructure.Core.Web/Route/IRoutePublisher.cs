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
    /// 路由发布 接口
    /// </summary>
    public interface IRoutePublisher
    {
        /// <summary>
        /// 注册Mvc路由
        /// </summary>
        /// <param name="routes">Routes</param>
        void RegisterMvcRoutes(RouteCollection routes);
        
        /// <summary>
        /// 注册Api路由
        /// </summary>
        /// <param name="config"></param>
        void RegisterApiRoutes(HttpConfiguration config);
    }
}
