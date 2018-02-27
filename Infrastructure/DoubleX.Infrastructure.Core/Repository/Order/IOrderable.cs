using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace DoubleX.Infrastructure.Core.Repository
{
    /// <summary>
    /// 排序规范接口
    /// 注：(1)函数返回自身连续调用
    /// (2)global::特性返回Queryable数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderable<T>
    {
        #region Lambda表达式实现

        /// <summary>
        /// 递增
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IOrderable<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector);

        /// <summary>
        /// 然后递增
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="keySelector1"></param>
        /// <returns></returns>
        IOrderable<T> ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector);
        /// <summary>
        /// 递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IOrderable<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector);

        /// <summary>
        /// 然后递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IOrderable<T> ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector);

        #endregion

        #region 根据传入字符实现

        /// <summary>
        /// 递增
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        IOrderable<T> Asc(string propertyName);

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        IOrderable<T> Desc(string propertyName);

        #endregion

        /// <summary>
        /// 排序后的结果集(解决名称冲突)
        /// ref:https://zhidao.baidu.com/question/33127462.html
        /// </summary>
        global::System.Linq.IQueryable<T> Queryable { get; }
    }
}
