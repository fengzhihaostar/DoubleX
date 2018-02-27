using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Common
{
    /// <summary>
    /// 通知业务接口
    /// </summary>
    public interface INotificationService
    {

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        bool SendSms(string mobile, string content);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        /// <returns></returns>
        bool SendSms(string mobile, string content, out Guid taskId);

        /// <summary>
        /// 发批量发送短信(暂不实现)
        /// </summary>
        /// <param name="mobiles">批量手机号列表(可以做限制一次不超过100个的机号)</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        bool SendSmsList(List<string> mobiles, string content, out Guid taskId);


        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        bool SendEmail(string email, string content);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="content">消息内容</param>
        /// <param name="taskId">发送任务Id</param>
        /// <returns></returns>
        bool SendEmail(string email, string content, out Guid taskId);


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="receiver">接收者</param>
        /// <param name="type">类型</param>
        /// <param name="paramsList">参数列表</param>
        /// <returns></returns>
        Guid SendVerifyCode(string receiver, string type, List<KeyValuePair<string, string>> paramsList, Guid visitId);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="receiver">接收者</param>
        /// <param name="type">类型</param>
        /// <param name="paramsList">参数列表</param>
        /// <param name="code">验证码(out 返回)</param>
        /// <param name="visitId">访问者ID</param>
        /// <returns></returns>
        Guid SendVerifyCode(string receiver, string type, List<KeyValuePair<string, string>> paramsList, Guid visitId, out string code);

        /// <summary>
        /// 确认验证码是否正确
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="receiver">接收者</param>
        /// <param name="code">验证码</param>
        /// <param name="visitId">访问者ID</param>
        /// <param name="isExpire">是否验证通过立即过期</param>
        /// <returns></returns>
        bool ConfirmVerifyCode(Guid taskId, string receiver, string code, Guid visitId, bool isExpire = false);

        /// <summary>
        /// 设置验证码过期
        /// </summary>
        /// <param name="taskId"></param>
        void ExpireVerifyCode(Guid taskId);

    }
}
