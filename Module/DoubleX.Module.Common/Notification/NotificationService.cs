using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Service;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Module.Common
{
    /// <summary>
    /// 通知业务操作
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        public bool SendSms(string mobile, string content)
        {
            if (VerifyHelper.IsEmpty(mobile) || VerifyHelper.IsEmpty(content))
                throw new DefaultException(EnumResultCode.参数错误, "mobile", "content");

            Guid taskId = Guid.Empty;
            return SendSms(mobile, content, out taskId);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        /// <returns></returns>
        public bool SendSms(string mobile, string content, out Guid taskId)
        {
            if (VerifyHelper.IsEmpty(mobile) || VerifyHelper.IsEmpty(content))
                throw new DefaultException(EnumResultCode.参数错误, "mobile", "content");


            taskId = Guid.Empty;
            var model = new
            {
                Mobile = mobile,
                Content = content
            };
            var result = DataCenterHelper.Post(DataCenterConfig.GetApiUrl(KeyModel.Config.DataCenter.KeySmsSend), model);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发批量发送短信(暂不实现)
        /// </summary>
        /// <param name="mobiles">批量手机号列表(可以做限制一次不超过100个的机号)</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        public bool SendSmsList(List<string> mobiles, string content, out Guid taskId)
        {
            if (VerifyHelper.IsEmpty(mobiles) || VerifyHelper.IsEmpty(content))
                throw new DefaultException(EnumResultCode.参数错误, "mobiles", "content");

            taskId = Guid.Empty;
            return false;
        }



        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        public bool SendEmail(string email, string content)
        {
            if (VerifyHelper.IsEmpty(email) || VerifyHelper.IsEmpty(content))
                throw new DefaultException(EnumResultCode.参数错误, "email", "content");

            Guid taskId = Guid.Empty;
            return SendSms(email, content, out taskId);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        /// <returns></returns>
        public bool SendEmail(string email, string content, out Guid taskId)
        {
            if (VerifyHelper.IsEmpty(email) || VerifyHelper.IsEmpty(content))
                throw new DefaultException(EnumResultCode.参数错误, "mobile", "content");


            taskId = Guid.Empty;
            var model = new
            {
                Mobile = email,
                Content = content
            };
            var result = DataCenterHelper.Post(DataCenterConfig.GetApiUrl(KeyModel.Config.DataCenter.KeyEmailSend), model);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="receiver">接收者</param>
        /// <param name="type">类型</param>
        /// <param name="paramsList">参数列表</param>
        /// <returns>发送任务ID</returns>
        public Guid SendVerifyCode(string receiver, string type, List<KeyValuePair<string, string>> paramsList, Guid visitId)
        {
            if (VerifyHelper.IsEmpty(receiver) || VerifyHelper.IsEmpty(type) || VerifyHelper.IsEmpty(visitId))
                throw new DefaultException(EnumResultCode.参数错误, "receiver", "type", "visitId");

            string code = "";
            return SendVerifyCode(receiver, type, paramsList, visitId, out code);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="receiver">接收者</param>
        /// <param name="type">类型</param>
        /// <param name="paramsList">参数列表</param>
        /// <param name="code">验证码(out 返回)</param>
        /// <param name="visitId">访问者ID</param>
        /// <returns></returns>
        public Guid SendVerifyCode(string receiver, string type, List<KeyValuePair<string, string>> paramsList, Guid visitId, out string code)
        {
            if (VerifyHelper.IsEmpty(receiver) || VerifyHelper.IsEmpty(type) || VerifyHelper.IsEmpty(visitId))
                throw new DefaultException(EnumResultCode.参数错误, "receiver", "type", "visitId");

            //内容
            var content = TemplateConfig.GetValue(type);

            //参数
            if (VerifyHelper.IsEmpty(paramsList))
            {
                paramsList = new List<KeyValuePair<string, string>>();
            }

            //code（必须,默认）
            code = RandHelper.GetRandNumber(6);
            var codeParamModel = paramsList.Where(x => x.Key.ToLower() == "code").FirstOrDefault();
            if (VerifyHelper.IsEmpty(paramsList))
            {
                paramsList.Add(new KeyValuePair<string, string>("code", code));
            }
            else
            {
                codeParamModel = new KeyValuePair<string, string>("code", code);
            }

            //替换
            if (!VerifyHelper.IsEmpty(paramsList))
            {
                paramsList.ForEach(x =>
                {
                    content = content.Replace("{" + x.Key + "}", x.Value);
                });
            }


            List<string> smsTypes = new List<string>(){
                KeyModel.Config.Template.KeyRegistMobile,
                KeyModel.Config.Template.KeyBindMobile,
                KeyModel.Config.Template.KeyForgetPwdMobile
            };

            List<string> emailTypes = new List<string>(){
                KeyModel.Config.Template.KeyRegistEmail,
                KeyModel.Config.Template.KeyBindEmail,
                KeyModel.Config.Template.KeyForgetPwdEmail
            };

            bool isSuccess = false;

            //发送
            if (smsTypes.Contains(type))
            {
                isSuccess = SendSms(receiver, content);
            }
            if (emailTypes.Contains(type))
            {
                isSuccess = SendEmail(receiver, content);
            }

            if (!isSuccess)
            {
                throw new DefaultException(EnumResultCode.验证码发送失败);
            }

            var model = new VerifyCodeModel();
            model.TaskId = GuidHelper.NewId();
            model.Receiver = receiver;
            model.Code = code;
            model.VisitId = visitId;
            model.SendTime = DateTime.Now;

            //保存
            RedisHelper.Set<VerifyCodeModel>(string.Format(KeyModel.Session.FormatVerifyCode, model.TaskId), model, DateTime.Now.AddMinutes(10));

            return model.TaskId;
        }

        /// <summary>
        /// 确认验证码是否正确
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="receiver">接收者</param>
        /// <param name="code">验证码</param>
        /// <param name="visitId">访问者ID</param>
        /// <param name="isExpire">是否验证通过立即过期</param>
        /// <returns></returns>
        public bool ConfirmVerifyCode(Guid taskId, string receiver, string code, Guid visitId, bool isExpire = false)
        {
            if (VerifyHelper.IsEmpty(taskId) || VerifyHelper.IsEmpty(receiver) || VerifyHelper.IsEmpty(code) || VerifyHelper.IsEmpty(visitId))
            {
                throw new DefaultException(EnumResultCode.参数错误, "taskId", "receiver", "code", "visitId");
            }

            string key = string.Format(KeyModel.Session.FormatVerifyCode, taskId);

            VerifyCodeModel model = RedisHelper.Get<VerifyCodeModel>(key);

            if (VerifyHelper.IsEmpty(model))
            {
                throw new DefaultException(EnumResultCode.验证码己失效);
            }
            if (model.Code == code && model.Receiver == receiver && model.VisitId == visitId)
            {
                if (isExpire)
                {
                    RedisHelper.Set<VerifyCodeModel>(key, null);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 设置验证码过期
        /// </summary>
        /// <param name="taskId"></param>
        public void ExpireVerifyCode(Guid taskId)
        {
            if (VerifyHelper.IsEmpty(taskId))
            {
                throw new DefaultException(EnumResultCode.参数错误, "taskId");
            }
            string key = string.Format(KeyModel.Session.FormatVerifyCode, taskId);
            RedisHelper.Set<VerifyCodeModel>(key, null);
        }
    }
}
