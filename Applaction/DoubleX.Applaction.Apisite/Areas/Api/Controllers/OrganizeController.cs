using System;
using System.Collections.Generic;
using System.Linq;
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
using DoubleX.Module.Organize;

namespace DoubleX.Applaction.Web.Areas.Api.Controllers
{
    /// <summary>
    /// 组织机构模块API控制器
    /// </summary>
    public class OrganizeController : ApiController
    {
        protected IEmployeeService employeeService;
        public OrganizeController(IEmployeeService iEmployeeService)
        {
            employeeService = iEmployeeService;
        }

        #region 职员操作

        /// <summary>
        /// 职员查询
        /// </summary>
        public HttpResponseMessage EmployeeQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var list = employeeService.Query(requestQuery, StringHelper.Get(request.Obj["Key"]), out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员移除
        /// </summary>
        public HttpResponseMessage EmployeeRemove(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                List<Guid> ids = JsonHelper.Deserialize<List<Guid>>(StringHelper.Get(request.Obj["ids"]));
                if (ids.Count == 0)
                {
                    throw new DefaultException(EnumResultCode.参数错误);
                }
                employeeService.Remove(x => ids.Contains(x.Id));
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员密码重置
        /// </summary>
        public HttpResponseMessage EmployeeResetPwd(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                List<Guid> ids = JsonHelper.Deserialize<List<Guid>>(StringHelper.Get(request.Obj["ids"]));
                if (ids.Count == 0)
                {
                    throw new DefaultException(EnumResultCode.参数错误);
                }
                employeeService.ResetPwd(ids);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员登录
        /// </summary>
        public HttpResponseMessage EmployeeLogin(RequestModel request)
        {
            var result = WebHelper.GetResult<string>(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = employeeService.Login(StringHelper.Get(request.Obj["account"]), StringHelper.Get(request.Obj["password"]));
                if (!VerifyHelper.IsEmpty(model) && !VerifyHelper.IsEmpty(model.Id))
                {
                    WebHelper.SetEmployee(model);
                    result.Code = EnumHelper.GetValue(EnumResultCode.跳转地址);
                    result.Redirect = UrlsHelper.GetRefUrl(defaultUrl: WebHelper.GetManageUrl());
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        #endregion
    }
}
