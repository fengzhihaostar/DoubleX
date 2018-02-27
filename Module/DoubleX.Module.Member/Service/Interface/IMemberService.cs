using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Member
{
    /// <summary>
    /// 会员业务接口
    /// </summary>
    public interface IMemberService : IService<MemberEntity, Guid>
    {
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="memberId">会员Id</param>
        MemberDetailModel GetDetail(Guid memberId);

        /// <summary>
        /// 会员分页查询(根据关键字)
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="key">关键字</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        List<MemberEntity> Query(RequestQueryModel queryModel, string key, out long count);

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="clientIp"></param>
        /// <returns></returns>
        MemberEntity Login(string account, string password, string clientIp);

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="clientIp"></param>
        MemberEntity Regist(string account, string email, string mobile, string password, string nameTag, string clientIp);


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">ids</param>
        /// <param name="defaultPassword">初始密码</param>
        void ResetPwd(List<Guid> ids, string defaultPassword = "123456");

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        MemberEntity BindMobile(Guid memberId, string mobile);


        /// <summary>
        /// 绑定邮箱地址
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        MemberEntity BindEmail(Guid memberId, string email);

        /// <summary>
        /// 找回密码修改
        /// </summary>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="clientIp"></param>
        MemberEntity MemberForgetPwd(string email, string mobile, string password, string clientIp);


    }
}
