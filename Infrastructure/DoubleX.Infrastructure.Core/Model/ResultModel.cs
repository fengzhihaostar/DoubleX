using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class ResultModel<TEntity>
    {
        public int Code { get { return code; } set { code = value; } }
        protected int code = EnumHelper.GetValue(EnumResultCode.操作成功);

        public string Message
        {
            get
            {
                if (VerifyHelper.IsEmpty(message) && Code != EnumHelper.GetValue(EnumResultCode.操作成功))
                {
                    message = EnumHelper.GetName(typeof(EnumResultCode), Code);
                }
                return message;
            }
            set
            {
                message = value;
            }
        }
        protected string message = null;

        public string Redirect { get; set; }

        public TEntity Obj { get; set; }
    }

    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class ResultModel : ResultModel<dynamic> { }


    /// <summary>
    /// 返回结果对象(Query查询)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ResultQueryModel<TEntity>
    {
        public ResultQueryModel() { }

        public ResultQueryModel(RequestQueryModel queryModel, long total, TEntity items)
        {
            if (VerifyHelper.IsNull(queryModel))
            {
                queryModel = new RequestQueryModel();
            }
            PageIndex = queryModel.PageIndex;
            PageSize = queryModel.PageSize;
            Total = total;
            Items = items;
        }

        public ResultQueryModel(int pageIndex, int pageSize, long total, TEntity items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Items = items;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 返回结果集
        /// </summary>
        public virtual TEntity Items { get; set; }
    }

    /// <summary>
    /// 返回结果对象(Query查询)
    /// </summary>
    public class ResultQueryModel : ResultQueryModel<dynamic>
    {
        public ResultQueryModel() { }
        public ResultQueryModel(RequestQueryModel queryModel, long total, dynamic items)
        {
            if (VerifyHelper.IsNull(queryModel)){
                queryModel = new RequestQueryModel();
            }
            PageIndex = queryModel.PageIndex;
            PageSize = queryModel.PageSize;
            Total = total;
            Items = items;
        }

        public ResultQueryModel(int pageIndex, int pageSize, long total, dynamic items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Items = items;
        }

    }

}
