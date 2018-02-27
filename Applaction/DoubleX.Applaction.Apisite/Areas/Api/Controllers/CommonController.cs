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
using DoubleX.Module.Common;
using DoubleX.Module.Member;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    public class CommonController : ApiController
    {
        protected INotificationService notificationService;
        public CommonController(INotificationService iNotificationService)
        {
            notificationService = iNotificationService;
        }

        #region 通知消息操作

        /// <summary>
        /// 发送验证码
        /// </summary>
        public HttpResponseMessage SendVerifyCode(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                string type = JsonHelper.GetValue(request.Obj, "type");
                if (VerifyHelper.IsEmpty(type))
                {
                    throw new MessageException(EnumResultCode.请求错误);
                }

                string receiver = JsonHelper.GetValue(request.Obj, "receiver");
                if (VerifyHelper.IsEmpty(receiver))
                {
                    throw new MessageException(EnumMessageCode.请输入验证码接收地址);
                }

                Guid taskId = notificationService.SendVerifyCode(receiver, type, null, request.CurrentContext.VisitId);
                result.Obj = taskId;
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 确认验证码
        /// </summary>
        public HttpResponseMessage ConfirmVerifyCode(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid taskId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "TaskId"));
                if (VerifyHelper.IsEmpty(taskId))
                {
                    throw new MessageException(EnumResultCode.请求错误);
                }

                string receiver = JsonHelper.GetValue(request.Obj, "Receiver");
                string code = JsonHelper.GetValue(request.Obj, "Code");
                if (VerifyHelper.IsEmpty(receiver))
                {
                    throw new MessageException(EnumMessageCode.请输入验证码接收地址);
                }
                if (VerifyHelper.IsEmpty(code))
                {
                    throw new MessageException(EnumMessageCode.请输入验证码);
                }

                bool isSuccess = notificationService.ConfirmVerifyCode(taskId, receiver, code, request.CurrentContext.VisitId);
                if (!isSuccess)
                {
                    throw new MessageException(EnumMessageCode.验证码错误);
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        #endregion


    }
}