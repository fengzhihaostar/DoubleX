using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Member;

namespace DoubleX.Applaction.Apisite.Areas.Member.Controllers
{
    /// <summary>
    /// 流量控制器
    /// </summary>
    public class TrafficController : MvcBaseController
    {
        /// <summary>
        /// 页面-使用记录
        /// </summary>
        /// <returns></returns>
        public ActionResult UseRecord()
        {
            return View();
        }
	}
}