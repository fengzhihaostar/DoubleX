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
using DoubleX.Module.Trade;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    public class MemberController : ApiController
    {
        protected INotificationService notificationService;
        protected IMemberService memberService;
        protected IRechargeRecordService rechargeRecordService;

        public MemberController(IMemberService iMemberService, INotificationService iNotificationService, IRechargeRecordService iRechargeRecordService)
        {
            notificationService = iNotificationService;
            memberService = iMemberService;
            rechargeRecordService = iRechargeRecordService;
        }

        #region 会员操作

        /// <summary>
        /// 会员获取
        /// </summary>
        public HttpResponseMessage MemberGet(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid id = WebHelper.GetMemberId(request);
                if (VerifyHelper.IsEmpty(id))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                result.Obj = memberService.Get(x => x.Id == id);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 会员详细获取
        /// </summary>
        public HttpResponseMessage MemberGetDetail(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid id = WebHelper.GetMemberId(request);
                if (VerifyHelper.IsEmpty(id))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                result.Obj = memberService.GetDetail(id);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 会员查询
        /// </summary>
        public HttpResponseMessage MemberQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var list = memberService.Query(requestQuery, StringHelper.Get(request.Obj["Key"]), out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 会员修改
        /// </summary>
        public HttpResponseMessage MemberModify(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = JsonHelper.Deserialize<MemberEntity>(request.JsonString);
                if (VerifyHelper.IsEmpty(model))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                var entity = memberService.Get(WebHelper.GetMemberId(request));
                if (VerifyHelper.IsEmpty(entity))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                if (!entity.MobileIsVerify)
                {
                    entity.Mobile = model.Mobile;
                }
                if (!entity.EmailIsVerify)
                {
                    entity.Email = model.Email;
                }
                entity.RealName = model.RealName;
                entity.Credits = model.Credits;
                entity.Birthday = model.Birthday;
                entity.Sex = model.Sex;
                entity.LastId = entity.Id;
                entity.LastDt = DateTime.Now;
                entity.NameTag = model.NameTag;

                memberService.Update(entity);

                result.Obj = entity;
            }
            return WebApiHelper.ToHttpResponseMessage(result);

        }


        /// <summary>
        /// 会员移除
        /// </summary>
        public HttpResponseMessage MemberRemove(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                List<Guid> ids = JsonHelper.Deserialize<List<Guid>>(StringHelper.Get(request.Obj["ids"]));
                if (ids.Count == 0)
                {
                    throw new DefaultException(EnumResultCode.参数错误);
                }
                memberService.Remove(x => ids.Contains(x.Id));
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }



        /// <summary>
        /// 会员登录
        /// </summary>
        public HttpResponseMessage MemberLogin(RequestModel request)
        {
            var result = WebHelper.GetResult<string>(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = memberService.Login(StringHelper.Get(request.Obj["account"]), StringHelper.Get(request.Obj["password"]), BrowserHelper.GetClientIP());
                if (!VerifyHelper.IsEmpty(model) && !VerifyHelper.IsEmpty(model.Id))
                {
                    WebHelper.SetMember(model);
                    result.Code = EnumHelper.GetValue(EnumResultCode.跳转地址);
                    result.Redirect = UrlsHelper.GetRefUrl(defaultUrl: WebHelper.GetMemberUrl());
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 会员注册
        /// </summary>
        public HttpResponseMessage MemberRegist(RequestModel request)
        {
            var result = WebHelper.GetResult<string>(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                string receiver = "", mobile = JsonHelper.GetValue(request.Obj, "Mobile"),
                    email = JsonHelper.GetValue(request.Obj, "Email"),
                    code = JsonHelper.GetValue(request.Obj, "Code"),
                    pwd = JsonHelper.GetValue(request.Obj, "Pwd"),
                    nameTag = JsonHelper.GetValue(request.Obj, "NameTag"),
                    sendType = JsonHelper.GetValue(request.Obj, "SendType");
                Guid taskId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "TaskId"));

                #region 传入信息判断

                if (VerifyHelper.IsEmpty(sendType) ||
                    (sendType != KeyModel.Config.Template.KeyRegistMobile && sendType != KeyModel.Config.Template.KeyRegistEmail))
                    throw new MessageException(EnumMessageCode.信息错误);

                if ((VerifyHelper.IsEmpty(mobile) && VerifyHelper.IsEmpty(email)) || VerifyHelper.IsEmpty(taskId))
                    throw new MessageException(EnumMessageCode.信息错误);

                if (sendType == KeyModel.Config.Template.KeyRegistMobile && VerifyHelper.IsEmpty(mobile))
                    throw new MessageException(EnumMessageCode.请输入手机号码);

                if (sendType == KeyModel.Config.Template.KeyRegistEmail && VerifyHelper.IsEmpty(email))
                    throw new MessageException(EnumMessageCode.请输入邮箱地址);

                if (VerifyHelper.IsEmpty(pwd))
                    throw new MessageException(EnumMessageCode.请输入密码);

                if (VerifyHelper.IsEmpty(code))
                    throw new MessageException(EnumMessageCode.请输入验证码);

                #endregion

                receiver = sendType == KeyModel.Config.Template.KeyRegistMobile ? mobile : email;

                //验证码效验
                bool isSuccess = notificationService.ConfirmVerifyCode(taskId, receiver, code, request.CurrentContext.VisitId);
                if (!isSuccess)
                {
                    throw new MessageException(EnumMessageCode.验证码错误);
                }

                //注册
                var entity = memberService.Regist(receiver, email, mobile, pwd, nameTag, BrowserHelper.GetClientIP());
                if (VerifyHelper.IsEmpty(entity))
                    throw new MessageException(EnumMessageCode.注册失败);
                //首次注册，送100元， 添加充值记录信息
                rechargeRecordService.Insert(new RechargeRecordEntity
                {
                    AccountId = entity.Id,
                    MoneyValue = 100,
                    Descript = "注册赠送",
                    CreateId = entity.Id,
                    CreateDt = DateTime.Now,
                    LastId = entity.Id,
                    LastDt = DateTime.Now
                     
                });
                //返回
                if (!VerifyHelper.IsEmpty(entity) && !VerifyHelper.IsEmpty(entity.Id))
                {
                    notificationService.ExpireVerifyCode(taskId);
                    WebHelper.SetMember(entity);
                    result.Code = EnumHelper.GetValue(EnumResultCode.跳转地址);
                    result.Redirect = UrlsHelper.GetRefUrl(defaultUrl: WebHelper.GetMemberUrl());
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public HttpResponseMessage MemberEditPwd(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var entity = memberService.Get(WebHelper.GetMemberId(request));
                if (VerifyHelper.IsEmpty(entity))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                var oldPwd = StringHelper.Get(JsonHelper.GetValue(request.Obj, "OldPwd"));
                var newPwd = StringHelper.Get(JsonHelper.GetValue(request.Obj, "NewPwd"));

                if (VerifyHelper.IsEmpty(oldPwd + newPwd))
                {
                    throw new MessageException("请输入旧密码和新密码");
                }

                if (entity.Password != CoreHelper.GetPassword(oldPwd))
                {
                    throw new MessageException("旧密码不正确");
                }

                entity.Password = CoreHelper.GetPassword(newPwd);
                entity.LastId = entity.Id;
                entity.LastDt = DateTime.Now;
                memberService.Update(entity);

                result.Obj = entity;
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        public HttpResponseMessage MemberResetPwd(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                List<Guid> ids = JsonHelper.Deserialize<List<Guid>>(StringHelper.Get(request.Obj["ids"]));
                if (ids.Count == 0)
                {
                    throw new DefaultException(EnumResultCode.参数错误);
                }
                memberService.ResetPwd(ids);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        public HttpResponseMessage MemberBindMobile(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid taskId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "TaskId"));
                Guid memberId = WebHelper.GetMemberId(request);
                string receiver = JsonHelper.GetValue(request.Obj, "Receiver");
                string code = JsonHelper.GetValue(request.Obj, "Code");

                #region 传入信息判断

                if (VerifyHelper.IsEmpty(memberId) || VerifyHelper.IsEmpty(taskId))
                    throw new MessageException(EnumMessageCode.信息错误);

                if (VerifyHelper.IsEmpty(receiver))
                    throw new MessageException(EnumMessageCode.请输入手机号码);

                if (VerifyHelper.IsEmpty(code))
                    throw new MessageException(EnumMessageCode.请输入验证码);

                #endregion

                bool isSuccess = notificationService.ConfirmVerifyCode(taskId, receiver, code, request.CurrentContext.VisitId);
                if (!isSuccess)
                {
                    throw new MessageException(EnumMessageCode.验证码错误);
                }

                var entity = memberService.BindMobile(memberId, receiver);
                if (!VerifyHelper.IsEmpty(entity))
                {
                    notificationService.ExpireVerifyCode(taskId);
                    result.Obj = entity;
                }
                else
                {
                    throw new MessageException(EnumMessageCode.绑定失败);
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 绑定邮箱地址
        /// </summary>
        public HttpResponseMessage MemberBindEmail(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid taskId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "TaskId"));
                Guid memberId = WebHelper.GetMemberId(request);
                string receiver = JsonHelper.GetValue(request.Obj, "Receiver");
                string code = JsonHelper.GetValue(request.Obj, "Code");

                #region 传入信息判断

                if (VerifyHelper.IsEmpty(memberId) || VerifyHelper.IsEmpty(taskId))
                    throw new MessageException(EnumMessageCode.信息错误);

                if (VerifyHelper.IsEmpty(receiver))
                    throw new MessageException(EnumMessageCode.请输入邮箱地址);

                if (VerifyHelper.IsEmpty(code))
                    throw new MessageException(EnumMessageCode.请输入验证码);

                #endregion

                bool isSuccess = notificationService.ConfirmVerifyCode(taskId, receiver, code, request.CurrentContext.VisitId);
                if (!isSuccess)
                {
                    throw new MessageException(EnumMessageCode.验证码错误);
                }

                var entity = memberService.BindEmail(memberId, receiver);
                if (!VerifyHelper.IsEmpty(entity))
                {
                    notificationService.ExpireVerifyCode(taskId);
                    result.Obj = entity;
                }
                else
                {
                    throw new MessageException(EnumMessageCode.绑定失败);
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }


        /// <summary>
        /// 找回密码
        /// </summary>
        public HttpResponseMessage MemberForgetPwd(RequestModel request)
        {
            var result = WebHelper.GetResult<string>(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                string receiver = "", mobile = JsonHelper.GetValue(request.Obj, "Mobile"),
                    email = JsonHelper.GetValue(request.Obj, "Email"),
                    code = JsonHelper.GetValue(request.Obj, "Code"),
                    pwd = JsonHelper.GetValue(request.Obj, "Pwd"),
                    sendType = JsonHelper.GetValue(request.Obj, "SendType");
                Guid taskId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "TaskId"));

                #region 传入信息判断

                if (VerifyHelper.IsEmpty(sendType) ||
                    (sendType != KeyModel.Config.Template.KeyForgetPwdMobile && sendType != KeyModel.Config.Template.KeyForgetPwdEmail))
                    throw new MessageException(EnumMessageCode.信息错误);

                if ((VerifyHelper.IsEmpty(mobile) && VerifyHelper.IsEmpty(email)) || VerifyHelper.IsEmpty(taskId))
                    throw new MessageException(EnumMessageCode.信息错误);

                if (sendType == KeyModel.Config.Template.KeyForgetPwdMobile && VerifyHelper.IsEmpty(mobile))
                    throw new MessageException(EnumMessageCode.请输入手机号码);

                if (sendType == KeyModel.Config.Template.KeyForgetPwdEmail && VerifyHelper.IsEmpty(email))
                    throw new MessageException(EnumMessageCode.请输入邮箱地址);

                if (VerifyHelper.IsEmpty(pwd))
                    throw new MessageException(EnumMessageCode.请输入密码);

                if (VerifyHelper.IsEmpty(code))
                    throw new MessageException(EnumMessageCode.请输入验证码);

                #endregion

                receiver = sendType == KeyModel.Config.Template.KeyForgetPwdMobile ? mobile : email;

                //验证码效验
                bool isSuccess = notificationService.ConfirmVerifyCode(taskId, receiver, code, request.CurrentContext.VisitId);
                if (!isSuccess)
                {
                    throw new MessageException(EnumMessageCode.验证码错误);
                }

                //找回密码修改
                var entity = memberService.MemberForgetPwd(email, mobile, pwd, BrowserHelper.GetClientIP());
                if (VerifyHelper.IsEmpty(entity))
                    throw new MessageException(EnumMessageCode.找回密码失败);

                //返回
                if (!VerifyHelper.IsEmpty(entity) && !VerifyHelper.IsEmpty(entity.Id))
                {
                    notificationService.ExpireVerifyCode(taskId);
                    WebHelper.SetMember(entity);
                    result.Code = EnumHelper.GetValue(EnumResultCode.跳转地址);
                    result.Redirect = UrlsHelper.GetRefUrl(defaultUrl: WebHelper.GetMemberUrl());
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }
        #endregion
    }
}