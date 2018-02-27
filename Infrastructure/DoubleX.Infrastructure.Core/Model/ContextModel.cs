using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 上下文对象
    /// </summary>
    public class ContextModel
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        public Guid VisitId { get; set; }

        /// <summary>
        /// 文化/区域(语言)
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// 租户信息
        /// </summary>
        public TenantModel Tenant { get; set; }

        /// <summary>
        /// 职员信息
        /// </summary>
        public EmployeeModel Employee { get; set; }

        /// <summary>
        /// 职员是否登录
        /// </summary>
        public bool IsEmployeeLogin
        {
            get
            {
                return !VerifyHelper.IsNull(Employee) && !VerifyHelper.IsEmpty(Employee.EmployeeId);
            }
        }

        /// <summary>
        /// 会员信息
        /// </summary>
        public MemberModel Member { get; set; }

        /// <summary>
        /// 会员是否登录
        /// </summary>
        public bool IsMemberLogin
        {
            get
            {
                return !VerifyHelper.IsNull(Member) && !VerifyHelper.IsEmpty(Member.MemberId);
            }
        }


    }
}
