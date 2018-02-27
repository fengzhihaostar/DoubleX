using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 请求信息实体(默认)
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 请求信息对象JObject
        /// </summary>
        public JObject Obj { get; set; }

        /// <summary>
        /// 请求信息对象Json字符串
        /// </summary>
        public string JsonString
        {
            get
            {
                return JsonHelper.Serialize(Obj);
            }
        }

        /// <summary>
        /// 请求Query
        /// </summary>
        public NameValueCollection QueryString
        {
            get
            {
                if (HttpContext.Current.Request.QueryString != null)
                {
                    return HttpContext.Current.Request.QueryString;
                }
                return null;
            }
        }

        /// <summary>
        /// 请求上下文
        /// </summary>
        public ContextModel CurrentContext { get; set; }

    }

    /// <summary>
    /// 请求信息实体(列表查询参数)
    /// </summary>
    public class RequestQueryModel
    {
        public RequestQueryModel()
        {
            PageIndex = 1;
            PageSize = 0;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = value; } }
        protected int pageSize = 10;


        /// <summary>
        /// 排序字段
        /// </summary>
        public List<KeyValuePair<string, string>> Sorting { get; set; }
    }
}
