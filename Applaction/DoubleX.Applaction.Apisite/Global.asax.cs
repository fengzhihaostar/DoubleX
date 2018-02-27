using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DoubleX.Framework.Warmup.Hosting;
using DoubleX.Framework.Warmup.Working;

namespace DoubleX.Applaction.Apisite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 运行工作器
        /// </summary>
        private static DoubleXWorking<DoubleXMvcHosting> worker;

        /// <summary>
        /// 开始运行
        /// </summary>
        protected void Application_Start()
        {
            worker = new DoubleXWorking<DoubleXMvcHosting>(this);
            worker.ApplactionInit(this);
            worker.ApplactionStart(this);
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        protected void Application_BeginRequest()
        {
            worker.RequestBegin(this);
        }

        /// <summary>
        /// 请求结束
        /// </summary>
        protected void Application_EndRequest()
        {
            worker.RequestEnd(this);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
        }
    }
}
