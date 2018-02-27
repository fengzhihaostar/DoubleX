using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleX.Module.Common
{
    /// <summary>
    /// 表名信息
    /// </summary>
    public static class TableNameKey
    {
        #region 会员中心

        /// <summary>
        /// 会员信息
        /// </summary>
        public const string Member = "TB_Account";

        #endregion

        #region 组织机构

        public const string OrganizeEmployee = "TB_Admin";

        #endregion

        #region 交易支付

        public const string TradeRechargeRecord = "TB_RechargeRecord";
        public const string TradeCostRecord = "TB_CostRecord";

        #endregion

        #region 流量使用

        public const string Traffic = "TB_Traffic";

        #endregion
    }
}
