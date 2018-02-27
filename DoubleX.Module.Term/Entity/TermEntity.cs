using DoubleX.Infrastructure.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [Table("TB_UserTerm")]
    public partial class TermEntity : EntityFrameworkEntity
    {
        /// <summary>
        /// 项目表主键
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 子账户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 术语源语言
        /// </summary>
        public string TermSrcLang { get; set; }

        /// <summary>
        /// 术语原文
        /// </summary>
        public string TermSrc { get; set; }

        /// <summary>
        /// 术语译文语言
        /// </summary>
        public string TermTgtLang { get; set; }

        /// <summary>
        /// 术语译文
        /// </summary>
        public string TermTgt { get; set; }

        /// <summary>
        /// 平台编码
        /// </summary>
        public int PlatformCode { get; set; }

        /// <summary>
        /// 创建IP
        /// </summary>
        public string CreateIP { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastIP { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int Status { get; set; }
    }
}
