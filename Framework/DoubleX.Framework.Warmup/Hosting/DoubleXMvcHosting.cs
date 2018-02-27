using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using System.Reflection;
using System.Resources;
using System.Net.Http.Formatting;
using System.Web.Http.Dispatcher;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.TypeFinders;
using DoubleX.Infrastructure.Core.Web;
using DoubleX.Framework;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Binder;
using DoubleX.Framework.Web.Filter;

namespace DoubleX.Framework.Warmup.Hosting
{
    /// <summary>
    /// Mvc服务默认
    /// </summary>
    public class DoubleXMvcHosting : DoubleXDefaultHosting, IDoubleXHosting
    {
        void IDoubleXHosting.OnInit(HttpApplication applaction)
        {
            base.OnInit(applaction);

            //语言资源文件初始
            WarmupHelper.LanguageResourceInit(Languages);

            //初始全局RedisCient
            RedisHelper.Init(SettingConfig.GetValue(KeyModel.Config.Setting.KeyRedisDefault));

            //初始Log4net日志配置
            log4net.Config.XmlConfigurator.Configure();

            //模块加载(暂默认全部)
        }

        void IDoubleXHosting.OnStart(HttpApplication applaction)
        {
            base.OnStart(applaction);

            IRoutePublisher routePublisher = new RoutePublisher(new WebAppTypeFinder());

            #region WebApi 默认注册

            GlobalConfiguration.Configure(new Action<HttpConfiguration>((HttpConfiguration config) =>
            {
                config.MapHttpAttributeRoutes();

                routePublisher.RegisterApiRoutes(config);

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                //WebApi自定义请求模型绑定
                config.ParameterBindingRules.Insert(0, param =>
                {
                    if (param.ParameterType == typeof(RequestModel))
                        return new ApiRequestBind(param);
                    return null;
                });

                //WebApi自定义返回(Json及Json中时间处理)
                config.Services.Replace(typeof(IContentNegotiator), new WebApiJSONFormat(new JsonMediaTypeFormatter()));

                //让路由支持namespace
                config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

                //WebApi自定义全局过滤器
                //(该处注册后，每个action都会执行一次.controller/action不需要重复添加)
                config.Filters.Add(new ApiActionAttribute());       //处理返回过滤器
                config.Filters.Add(new ApiExceptionAttribute());    //异常过滤器

            }));



            #endregion

            #region Mvc 默认注册

            //区域注册
            AreaRegistration.RegisterAllAreas();

            //过滤器注册
            new Action<GlobalFilterCollection>((filters) =>
            {
                filters.Add(new HandleErrorAttribute());
                filters.Add(new MvcActionAttribute());
                filters.Add(new MvcExceptionAttribute());

            })(GlobalFilters.Filters);


            //路由注册
            new Action<RouteCollection>((routes) =>
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


                routePublisher.RegisterMvcRoutes(routes);

                //启用特性路由(error/404)
                routes.MapMvcAttributeRoutes();

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    namespaces: new[] { string.Format("{0}.Controllers",SettingConfig.GetValue(KeyModel.Config.Setting.KeyApplaction, KeyModel.Config.Setting.GroupSystem)) }
                );

            })(RouteTable.Routes);

            //js,css绑定
            new Action<BundleCollection>((bundles) =>
            {

                #region
                //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                //            "~/Scripts/jquery-{version}.js"));

                //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
                //// 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
                //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                //            "~/Scripts/modernizr-*"));

                //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //          "~/Scripts/bootstrap.js",
                //          "~/Scripts/respond.js"));

                //bundles.Add(new StyleBundle("~/Content/css").Include(
                //          "~/Content/bootstrap.css",
                //          "~/Content/site.css"));
                #endregion

            })(BundleTable.Bundles);


            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            //MVC模型绑定
            ModelBinderProviders.BinderProviders.Insert(0, new MvcRequestBindProvider());

            #endregion
        }

        void IDoubleXHosting.OnBegin(HttpApplication applaction)
        {
            base.OnBegin(applaction);

            //设置客户端标识
            WebHelper.SetVisitId();

            //设置请求上下文
            WebHelper.SetContext();
        }

        void IDoubleXHosting.OnEnd(HttpApplication applaction)
        {
            base.OnEnd(applaction);
        }
    }
}
