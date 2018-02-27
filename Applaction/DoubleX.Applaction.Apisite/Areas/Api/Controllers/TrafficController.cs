using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Traffic;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    /// <summary>
    /// 流量信息接口
    /// </summary>
    public class TrafficController : ApiController
    {
        protected ITrafficService trafficService;
        public TrafficController(ITrafficService iTrafficService)
        {
            trafficService = iTrafficService;
        }

        #region 使用信息

        /// <summary>
        /// 使用记录
        /// </summary>
        public HttpResponseMessage UseRecordQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                requestQuery.Sorting = new List<KeyValuePair<string, string>>();
                requestQuery.Sorting.Add(new KeyValuePair<string, string>("CreateDt", "DESC"));
                var list = trafficService.ReadTraffic(requestQuery, WebHelper.GetContext().Member.MemberId, out total);

                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        #endregion
    }
}