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
using DoubleX.Module.Common;

namespace DoubleX.Module.Member
{
    public class MemberService : DefaultService<MemberRepository, MemberEntity, Guid>, IMemberService
    {
        public MemberService()
            : base(new MemberRepository())
        {

        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="memberId">会员Id</param>
        public MemberDetailModel GetDetail(Guid memberId)
        {
            if (VerifyHelper.IsEmpty(memberId))
            {
                throw new DefaultException(EnumResultCode.参数错误, "memberId");
            }
            return repository.GetDetail(memberId);
        }

        /// <summary>
        /// 会员分页查询(根据关键字)
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="key">关键字</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        public List<MemberEntity> Query(RequestQueryModel queryModel, string key, out long count)
        {
            key = StringHelper.FormatDefault(key);
            Expression<Func<MemberEntity, bool>> predicate = x => x.Account.ToLower().Contains(key);
            return Query(queryModel, predicate, out count);
        }

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="account">会员账号</param>
        /// <param name="password">会员密码</param>
        /// <returns></returns>
        public MemberEntity Login(string account, string password, string clientIp)
        {
            account = StringHelper.FormatDefault(account);
            password = StringHelper.FormatDefault(password);

            if (VerifyHelper.IsEmpty(account) || VerifyHelper.IsEmpty(password))
            {
                throw new MessageException(EnumResultCode.请输入登录账号或密码);
            }

            var query = Query(x => x.Account.ToLower().Trim() == account
                || (x.Email.ToLower().Trim() == account && x.EmailIsVerify)
                || (x.Mobile.ToLower().Trim() == account && x.MobileIsVerify));
            if (VerifyHelper.IsEmpty(query))
            {
                throw new MessageException(EnumResultCode.账号错误);
            }
            password = CoreHelper.GetPassword(password);
            var model = query.Where(x => x.Password == password).FirstOrDefault();
            if (VerifyHelper.IsEmpty(model))
            {
                throw new MessageException(EnumResultCode.密码错误);
            }

            //登录
            model.LastLoginDt = DateTime.Now;
            model.LastLoginIP = clientIp;
            Update(model);

            return model;
        }

        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="password">密码</param>
        /// <param name="clientIp">客户端ip</param>
        public MemberEntity Regist(string account, string email, string mobile, string password, string nameTag, string clientIp)
        {
            if ((VerifyHelper.IsEmpty(mobile) && VerifyHelper.IsEmpty(email)) || VerifyHelper.IsEmpty(password) || VerifyHelper.IsEmpty(clientIp))
                throw new DefaultException(EnumResultCode.参数错误, "mobile", "email", "password", "clientIp");

            var query = Query(x => x.Account.ToLower().Trim() == account
                || (x.Email.ToLower().Trim() == email && x.EmailIsVerify)
                || (x.Mobile.ToLower().Trim() == mobile && x.MobileIsVerify));
            if (!VerifyHelper.IsEmpty(query))
            {
                throw new MessageException(EnumMessageCode.该账号己注册);
            }

            var entity = new MemberEntity()
            {
                Account = VerifyHelper.IsEmpty(email) ? mobile : email,
                Password = MD5Helper.Get(password),
                RealName = "",
                NameTag = nameTag,
                Sex = EnumHelper.GetValue(EnumSex.保密),
                Email = email,
                EmailIsVerify = VerifyHelper.IsEmpty(email) ? false : true,
                Mobile = mobile,
                MobileIsVerify = VerifyHelper.IsEmpty(mobile) ? false : true,
                Credits = "",
                Birthday = DateTimeHelper.DefaultDateTime,
                Country = "",
                Area = "",
                Address = "",
                LastLoginIP = "#",
                LastLoginDt = DateTimeHelper.DefaultDateTime,
                //Balance = 0,
                Balance = 100,
                Type = EnumHelper.GetValue(EnumMemberType.Default),
                State = EnumHelper.GetValue(EnumMemberState.启用),
                IsDelete = false,
                CreateId = Guid.Empty,
                CreateDt = DateTime.Now,
                LastId = Guid.NewGuid(),
                LastDt = DateTime.Now
            };
            Insert(entity);

            return entity;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="defaultPassword">默认密码</param>
        public void ResetPwd(List<Guid> ids, string defaultPassword = "123456")
        {
            if (VerifyHelper.IsEmpty(ids))
                throw new MessageException(EnumResultCode.请选择要处理信息);

            var list = Query(x => ids.Contains(x.Id));
            foreach (var item in list)
            {
                item.Password = CoreHelper.GetPassword(defaultPassword);
            }
            Update(list);
        }


        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        public MemberEntity BindMobile(Guid memberId, string mobile)
        {
            if (VerifyHelper.IsEmpty(memberId) || VerifyHelper.IsEmpty(mobile))
                throw new DefaultException(EnumResultCode.参数错误, "memberId", "mobile");

            var query = Query(x => x.Mobile.ToLower().Trim() == mobile && x.MobileIsVerify);
            if (!VerifyHelper.IsEmpty(query))
            {
                throw new MessageException(EnumMessageCode.该手机号码己绑定);
            }

            var entity = Get(x => x.Id == memberId);
            entity.Mobile = mobile;
            entity.MobileIsVerify = true;
            entity.LastId = entity.Id;
            entity.LastDt = DateTime.Now;
            Update(entity);

            return entity;
        }


        /// <summary>
        /// 绑定邮箱地址
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="mobile"></param>
        public MemberEntity BindEmail(Guid memberId, string email)
        {

            if (VerifyHelper.IsEmpty(memberId) || VerifyHelper.IsEmpty(email))
                throw new DefaultException(EnumResultCode.参数错误, "memberId", "email");

            var query = Query(x => x.Email.ToLower().Trim() == email && x.EmailIsVerify);
            if (!VerifyHelper.IsEmpty(query))
            {
                throw new MessageException(EnumMessageCode.该邮箱地址己绑定);
            }

            var entity = Get(x => x.Id == memberId);
            entity.Email = email;
            entity.EmailIsVerify = true;
            entity.LastId = entity.Id;
            entity.LastDt = DateTime.Now;
            Update(entity);

            return entity;
        }

        /// <summary>
        /// 找回密码修改
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="password">密码</param>
        /// <param name="clientIp">客户端ip</param>
        public MemberEntity MemberForgetPwd(string email, string mobile, string password, string clientIp)
        {
            if ((VerifyHelper.IsEmpty(mobile) && VerifyHelper.IsEmpty(email)) || VerifyHelper.IsEmpty(password) || VerifyHelper.IsEmpty(clientIp))
                throw new DefaultException(EnumResultCode.参数错误, "mobile", "email", "password", "clientIp");

            var query = Query(x => (x.Email.ToLower().Trim() == email && x.EmailIsVerify)
                || (x.Mobile.ToLower().Trim() == mobile && x.MobileIsVerify));

            if (VerifyHelper.IsEmpty(query))
            {
                throw new MessageException(EnumMessageCode.未找到账号信息);
            }

            var entity = query.FirstOrDefault();
            entity.Password = MD5Helper.Get(password);
            entity.LastDt = DateTime.Now;
            Update(entity);

            return entity;
        }

    }
}
