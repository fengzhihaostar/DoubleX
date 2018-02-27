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
    /// ���ü�¼
    /// </summary>
    [Table("TB_CostRecord")]
    public partial class CostRecordEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// ��Ա�˺�Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Required]
        public int CostType { get; set; }

        /// <summary>
        /// �������͹���ֵ
        /// </summary>
        [StringLength(200)]
        public string CostTypeRefValue { get; set; }

        /// <summary>
        /// ��ؽ��
        /// </summary>
        public decimal MoneyValue { get; set; }

        /// <summary>
        /// ����˵��
        /// </summary>
        [StringLength(600)]
        public string Descript { get; set; }

        /// <summary>
        /// ������������
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
