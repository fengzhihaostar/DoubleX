using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleX.Framework
{
    /// <summary>
    /// 路径信息实体
    /// </summary>
    public class PathModel
    {
        /// <summary>
        /// 主页文本
        /// </summary>
        public string HomeText { get; set; }
        /// <summary>
        /// 主页连接
        /// </summary>
        public string HomeLink { get; set; }

        /// <summary>
        /// 路径列表
        /// </summary>
        public List<KeyValuePair<string, string>> Items { get; set; }

        /// <summary>
        /// 当前文本
        /// </summary>
        public string CurrentText { get; set; }

        /// <summary>
        /// 当前连接
        /// </summary>
        public string CurrentLink { get; set; }
    }
}
