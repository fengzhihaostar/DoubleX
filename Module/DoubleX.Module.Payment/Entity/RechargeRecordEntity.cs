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
    /// ��ֵ��¼
    /// </summary>
    [Table("TB_RechargeRecord")]
    public partial class RechargeRecordEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// ��Ա�˺�Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Required]
        public int PaymentType { get; set; }

        /// <summary>
        /// �������͹���ֵ
        /// </summary>
        [StringLength(200)]
        public string PaymentTypeRefValue { get; set; }

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
        /// ��ֵ״̬
        /// </summary>
        public int RechargeState { get; set; }



        /// <summary>
        /// ֧����ʽ����
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
        /// ֧��״̬����
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
