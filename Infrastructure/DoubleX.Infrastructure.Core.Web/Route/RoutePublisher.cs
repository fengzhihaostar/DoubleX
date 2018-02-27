using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using DoubleX.Infrastructure.Core.TypeFinders;
using System.Web.Http;
using DoubleX.Infrastructure.Core.Module;

namespace DoubleX.Infrastructure.Core.Web
{
    /// <summary>
    /// 路由注册
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        protected readonly ITypeFinder typeFinder;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder"></param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        /// <summary>
        /// Find a plugin descriptor by some type which is located into its assembly
        /// </summary>
        /// <param name="providerType">Provider type</param>
        /// <returns>Plugin descriptor</returns>
        protected virtual ModuleDescriptor FindModule(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");

            //foreach (var plugin in PluginManager.ReferencedPlugins)
            //{
            //    if (plugin.ReferencedAssembly == null)
            //        continue;

            //    if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
            //        return plugin;
            //}

            return null;
        }

        /// <summary>
        /// MVC路由注册
        /// </summary>
        /// <param name="routes"></param>
        public virtual void RegisterMvcRoutes(RouteCollection routes)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                //Ignore not installed plugins
                //var plugin = FindModule(providerType);
                //if (plugin != null && !plugin.Installed)
                //    continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterMvcRoutes(routes));
        }

        /// <summary>
        /// Api路由注册
        /// </summary>
        /// <param name="config"></param>
        public virtual void RegisterApiRoutes(HttpConfiguration config)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                //Ignore not installed plugins
                //var plugin = FindModule(providerType);
                //if (plugin != null && !plugin.Installed)
                //    continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterApiRoutes(config));
        }
    }
}
