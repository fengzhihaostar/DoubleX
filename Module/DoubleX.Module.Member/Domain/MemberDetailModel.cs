using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Member
{
    /// <summary>
    /// ��Ա��ϸ��Ϣ
    /// </summary>
    //ref:http://www.cnblogs.com/usharei/archive/2012/04/20/2458858.html#ignoreHandling
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptOut)]
    public partial class MemberDetailModel : MemberEntity
    {
        [JsonIgnore]
        public override string Password { get; set; }

        /// <summary>
        /// ʹ����������(Size)
        /// </summary>
        public decimal TrafficTotal { get; set; }

        /// <summary>
        /// ��ֵ�ܶ�
        /// </summary>
        public decimal RechargeTotal { get; set; }

        /// <summary>
        /// �����ܶ�
        /// </summary>
        public decimal ConsumeTotal { get; set; }

        /// <summary>
        /// ���³�ֵ�ܶ�
        /// </summary>
        public decimal MonthRechargeTotal { get; set; }

        /// <summary>
        /// ���������ܶ�
        /// </summary>
        public decimal MonthConsumeTotal { get; set; }

        /// <summary>
        /// ��������ͳ��
        /// </summary>
        public decimal MonthTrafficTotal { get; set; }
    }
}