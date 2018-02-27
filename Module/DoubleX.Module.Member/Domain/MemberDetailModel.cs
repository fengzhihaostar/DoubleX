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
    /// 会员详细信息
    /// </summary>
    //ref:http://www.cnblogs.com/usharei/archive/2012/04/20/2458858.html#ignoreHandling
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptOut)]
    public partial class MemberDetailModel : MemberEntity
    {
        [JsonIgnore]
        public override string Password { get; set; }

        /// <summary>
        /// 使用流量总数(Size)
        /// </summary>
        public decimal TrafficTotal { get; set; }

        /// <summary>
        /// 充值总额
        /// </summary>
        public decimal RechargeTotal { get; set; }

        /// <summary>
        /// 消费总额
        /// </summary>
        public decimal ConsumeTotal { get; set; }

        /// <summary>
        /// 当月充值总额
        /// </summary>
        public decimal MonthRechargeTotal { get; set; }

        /// <summary>
        /// 当月消费总额
        /// </summary>
        public decimal MonthConsumeTotal { get; set; }

        /// <summary>
        /// 当月流量统计
        /// </summary>
        public decimal MonthTrafficTotal { get; set; }
    }
}