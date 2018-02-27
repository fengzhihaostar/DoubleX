using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Reflection;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Extras.DynamicProxy2;
﻿using Castle.DynamicProxy;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Framework.Interceptor;
using DoubleX.Framework.Warmup.Hosting;
using DoubleX.Module.Common;
using DoubleX.Module.Organize;
using DoubleX.Module.Member;
using DoubleX.Module.Traffic;
using DoubleX.Module.Trade;
using DoubleX.Module.Project;
using DoubleX.Module.Term;

namespace DoubleX.Framework.Warmup.Working
{
    /// <summary>
    /// 运行工作器
    /// </summary>
    public class DoubleXWorking<TEntity> where TEntity : class,IDoubleXHosting
    {
        #region 类属性(Hosting,Init,Start,Begin,End)

        /// <summary>
        /// 应用实例
        /// </summary>
        private volatile TEntity hosting;

        /// <summary>
        /// 初始操作
        /// </summary>
        private volatile Func<HttpApplication, TEntity> initialize;

        /// <summary>
        /// 启动操作
        /// </summary>
        private volatile Action<HttpApplication, TEntity> starter;


        /// <summary>
        /// 请求开始操作
        /// </summary>
        private volatile Action<HttpApplication, TEntity> requestBegin;

        /// <summary>
        /// 请求结束操作
        /// </summary>
        private volatile Action<HttpApplication, TEntity> requestEnd;


        #endregion

        #region 构造方法（属性设置）

        public DoubleXWorking(HttpApplication applaction)
        {
            initialize = new Func<HttpApplication, TEntity>((app) =>
            {
                hosting = (TEntity)CreateHost();
                hosting.OnInit(applaction);
                return hosting;
            });

            starter = new Action<HttpApplication, TEntity>((app, t) =>
            {
                hosting.OnStart(app);
            });

            requestBegin = new Action<HttpApplication, TEntity>((app, t) =>
            {
                hosting.OnBegin(app);
            });

            requestEnd = new Action<HttpApplication, TEntity>((app, t) =>
            {
                hosting.OnEnd(app);
            });
        }

        public DoubleXWorking(Func<HttpApplication, TEntity> init, Action<HttpApplication, TEntity> start, Action<HttpApplication, TEntity> begin, Action<HttpApplication, TEntity> end)
        {
            initialize = init;
            starter = start;
            requestBegin = begin;
            requestEnd = end;

        }

        #endregion

        /// <summary>
        /// 应用程序初始
        /// </summary>
        /// <param name="applaction"></param>
        public void ApplactionInit(HttpApplication applaction)
        {
            hosting = initialize(applaction);
        }

        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="applaction"></param>
        public void ApplactionStart(HttpApplication applaction)
        {
            starter(applaction, hosting);
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        /// <param name="applaction"></param>
        public void RequestBegin(HttpApplication applaction)
        {
            requestBegin(applaction, hosting);
        }

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="applaction"></param>
        public void RequestEnd(HttpApplication applaction)
        {
            requestEnd(applaction, hosting);
        }

        /// <summary>
        /// 创建并获取应用服务
        /// </summary>
        /// <returns></returns>
        public IDoubleXHosting CreateHost()
        {
            var container = CreateContainer();
            return container.Resolve<IDoubleXHosting>();
        }

        /// <summary>
        /// 创建IOC容器
        /// </summary>
        /// <returns></returns>
        protected IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            //启动工作器
            builder.RegisterType<DoubleXDefaultHosting>().As<IDoubleXHosting>();
            builder.RegisterType<DoubleXMvcHosting>().As<IDoubleXHosting>();

            //拦截器
            builder.RegisterType<ServiceLogInterceptor>();

            //拦截注册示例
            //builder.RegisterType<EmployeeService>().As<IEmployeeService>().EnableClassInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));
            //EnableInterfaceInterceptors()
            //EnableClassInterceptors()

            #region 公共模块

            builder.RegisterType<NotificationService>().As<INotificationService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 组织机构

            //builder.RegisterType<EmployeeService>().As<IEmployeeService>();
            //builder.RegisterType<RoleService>().As<IRoleService>();

            builder.RegisterType<EmployeeService>().As<IEmployeeService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 会员信息

            builder.RegisterType<MemberService>().As<IMemberService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 流量信息

            builder.RegisterType<TrafficService>().As<ITrafficService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 付款/费用

            builder.RegisterType<RechargeRecordService>().As<IRechargeRecordService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            builder.RegisterType<CostRecordService>().As<ICostRecordService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 项目信息

            builder.RegisterType<ProjectService>().As<IProjectService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            #region 术语管理

            builder.RegisterType<TermService>().As<ITermService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(ServiceLogInterceptor));

            #endregion

            //MVC
            //builder.RegisterAssemblyTypes(assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterControllers(Assembly.Load(SettingConfig.GetValue(KeyModel.Config.Setting.KeyApplaction, KeyModel.Config.Setting.GroupSystem)));//注册所有的Controller

            //WebApi
            //builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterApiControllers(Assembly.Load(SettingConfig.GetValue(KeyModel.Config.Setting.KeyApplaction, KeyModel.Config.Setting.GroupSystem))).PropertiesAutowired();

            var container = builder.Build();

            //注册api容器需要使用HttpConfiguration对象
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            CoreHelper.SetContainer(container);

            return container;
        }
    }
}
