using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Traffic
{
    /// <summary>
    /// 流量记录
    /// </summary>
    [Table("TB_Traffic")]
    public partial class TrafficEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 会员账号Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 子密钥
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 翻译唯一标识(关联Project)
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 流量大小(字符串长度或音频字符长度)
        /// </summary>
        public long Size { get; set; }

        public string Src { get; set; }


        /// <summary>
        /// 平台编辑
        /// </summary>
        public int PlatformCode { get; set; }
    }

    public partial class Traffic_ProjectName : TrafficEntity
    {
        public string ProjectName { get; set; }
    }
}
