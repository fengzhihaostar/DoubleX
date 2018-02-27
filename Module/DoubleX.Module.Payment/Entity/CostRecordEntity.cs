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
    /// 费用记录
    /// </summary>
    [Table("TB_CostRecord")]
    public partial class CostRecordEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 会员账号Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 费用类型
        /// </summary>
        [Required]
        public int CostType { get; set; }

        /// <summary>
        /// 费用类型关联值
        /// </summary>
        [StringLength(200)]
        public string CostTypeRefValue { get; set; }

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
        /// 费用类型名称
        /// </summary>
        [NotMapped]
        public string CostTypeText
        {
            get
            {
                return EnumHelper.GetName(typeof(EnumCostType), CostType);
            }
        }
    }
}
