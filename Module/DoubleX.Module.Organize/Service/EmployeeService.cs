using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Service;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Module.Organize
{
    public class EmployeeService : DefaultService<EmployeeRepository, EmployeeEntity, Guid>, IEmployeeService
    {
        public EmployeeService()
            : base(new EmployeeRepository())
        {

        }

        /// <summary>
        /// 职员分页查询(根据关键字)
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="key">关键字</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        public List<EmployeeEntity> Query(RequestQueryModel queryModel, string key, out long count)
        {
            key = StringHelper.FormatDefault(key);
            Expression<Func<EmployeeEntity, bool>> predicate = x => x.Account.ToLower().Contains(key);
            return Query(queryModel, predicate, out count);
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
        /// 职员登录
        /// </summary>
        /// <param name="account">职员账号</param>
        /// <param name="password">职员密码</param>
        /// <returns></returns>
        public EmployeeEntity Login(string account, string password)
        {
            account = StringHelper.FormatDefault(account);
            password = StringHelper.FormatDefault(password);

            if (VerifyHelper.IsEmpty(account) || VerifyHelper.IsEmpty(password))
            {
                throw new MessageException(EnumResultCode.请输入登录账号或密码);
            }

            var query = Query(x => x.Account.ToLower().Trim() == account);
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

            //登录+1
            model.LoginCount = model.LoginCount + 1;
            Update(model);

            return model;
        }
    }
}
