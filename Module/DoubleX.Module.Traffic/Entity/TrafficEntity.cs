using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoubleX.Infrastructure.Core.Entity;

namespace DoubleX.Module.Traffic
{
    /// <summary>
    /// ������¼
    /// </summary>
    [Table("TB_Traffic")]
    public partial class TrafficEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// ��Ա�˺�Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// ����Կ
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ����Ψһ��ʶ(����Project)
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// ������С(�ַ������Ȼ���Ƶ�ַ�����)
        /// </summary>
        public long Size { get; set; }

        public string Src { get; set; }


        /// <summary>
        /// ƽ̨�༭
        /// </summary>
        public int PlatformCode { get; set; }
    }

    public partial class Traffic_ProjectName : TrafficEntity
    {
        public string ProjectName { get; set; }
    }
}
