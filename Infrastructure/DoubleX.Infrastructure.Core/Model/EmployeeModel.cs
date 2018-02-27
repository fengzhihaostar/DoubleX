using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 职员信息
    /// </summary>
    public class EmployeeModel
    {
        /// <summary>
        /// 职员Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 职员类型
        /// </summary>
        public EnumEmployeeType EmployeeType
        {
            get
            {
                if (employeeType == EnumEmployeeType.Default)
                {
                    employeeType = EnumEmployeeType.租户职员;
                }
                return employeeType;
            }
            set
            {
                employeeType = value;
            }
        }
        private EnumEmployeeType employeeType = EnumEmployeeType.Default;

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
