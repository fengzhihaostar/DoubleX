using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 验证码信息实体
    /// </summary>
    public class VerifyCodeModel
    {
        /// <summary>
        /// 发送任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 访问者Id
        /// </summary>
        public Guid VisitId { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
