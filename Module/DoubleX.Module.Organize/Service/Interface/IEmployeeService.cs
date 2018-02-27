using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Organize
{
    /// <summary>
    /// 职员业务接口
    /// </summary>
    public interface IEmployeeService : IService<EmployeeEntity, Guid>
    {
        /// <summary>
        /// 职员分页查询(根据关键字)
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="key">关键字</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        List<EmployeeEntity> Query(RequestQueryModel queryModel, string key, out long count);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">ids</param>
        /// <param name="defaultPassword">初始密码</param>
        void ResetPwd(List<Guid> ids, string defaultPassword = "123456");

        /// <summary>
        /// 职员登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        EmployeeEntity Login(string account, string password);


    }
}
