using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 充值记录
    /// </summary>
    [Table("TB_RechargeRecord")]
    public partial class RechargeRecordEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 会员账号Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 费用类型
        /// </summary>
        [Required]
        public int PaymentType { get; set; }

        /// <summary>
        /// 费用类型关联值
        /// </summary>
        [StringLength(200)]
        public string PaymentTypeRefValue { get; set; }

        /// <summary>
        /// 相关金额
        /// </summary>
        public decimal MoneyValue { get; set; }

        /// <summary>
        /// 描述说明
        /// </summary>
        [StringLength(600)]
        public string Descript { get; set; }

        /// <summary>
        /// 充值状态
        /// </summary>
        public int RechargeState { get; set; }



        /// <summary>
        /// 支付方式名称
        /// </summary>
        [NotMapped]
        public string PaymentTypeText
        {
            get
            {
                return EnumHelper.GetName(typeof(EnumPaymentType), PaymentType);
            }
        }

        /// <summary>
        /// 支付状态名称
        /// </summary>
        [NotMapped]
        public string RechargeStateText
        {
            get
            {
                return EnumHelper.GetName(typeof(EnumRechargeState), RechargeState);
            }
        }

    }
}
