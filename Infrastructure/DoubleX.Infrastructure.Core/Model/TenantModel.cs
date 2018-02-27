using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 租户信息
    /// </summary>
    public class TenantModel
    {
        /// <summary>
        /// 租户类型(默认为管理系统)
        /// </summary>
        public EnumTenantType TenantType
        {
            get
            {
                if (tenantType == EnumTenantType.Default)
                {
                    tenantType = EnumTenantType.租户系统;
                }
                return tenantType;
            }
            set
            {
                tenantType = value;
            }
        }
        private EnumTenantType tenantType = EnumTenantType.Default;
    }
}
