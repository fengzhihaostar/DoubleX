using DoubleX.Infrastructure.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Project
{
    public enum ProjectType
    {
        [Description("内部账户")]
        InsideTest = 99,
        [Description("试用账户")]
        Trial = 0,
        [Description("正常账户(按时计费)")]
        BillingOnTime = 1,
        [Description("正常账户(按字计费)")]
        BillingOnWords = 2
    }

    /// <summary>
    /// 项目信息
    /// </summary>
    [Table("TB_Project")]
    public partial class ProjectEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 用户表主键
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 翻译Key
        /// </summary>
        public string TranslateKey { get; set; }

        /// <summary>
        /// 创建IP地址
        /// </summary>
        public string CreateIP { get; set; }

        /// <summary>
        /// 最后登录地址
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 有效期限
        /// </summary>
        public DateTime ValidityTime { get; set; }

        /// <summary>
        /// 有效字数
        /// </summary>
        public long SurplusWords { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int ProjectType { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int State { get; set; }
    }
}
