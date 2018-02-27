using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Repository
{
    /// <summary>
    /// 排序的默认实现(参考博客员@大叔Lind.DDD)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Orderable<T> : IOrderable<T>
    {
        private IQueryable<T> source;

        /// <summary>
        /// 传入输要排序的结果
        /// </summary>
        /// <param name="queryables"></param>
        public Orderable(IQueryable<T> queryable)
        {
            if (source == null)
                throw new ArgumentNullException("Orderable.queryable");

            source = queryable;
        }

        /// <summary>
        /// 排序之后的结果集
        /// </summary>
        public IQueryable<T> Queryable
        {
            get { return source; }
        }

        /// <summary>
        /// 递增
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IOrderable<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            source = (source as IOrderedQueryable<T>).OrderBy(keySelector);
            return this;
        }

        /// <summary>
        /// 然后递增
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IOrderable<T> ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            source = (source as IOrderedQueryable<T>).ThenBy(keySelector);
            return this;
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IOrderable<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            source = (source as IOrderedQueryable<T>).OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// 然后递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IOrderable<T> ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            source = (source as IOrderedQueryable<T>).ThenByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// 递增
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public IOrderable<T> Asc(string propertyName)
        {
            source = ApplyOrder(source, propertyName, "ThenByDescending");
            return this;
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public IOrderable<T> Desc(string propertyName)
        {
            return this;
        }

        /// <summary>
        /// 根据属性名(字符串)动态生成排序
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        protected IOrderedQueryable<T> ApplyOrder(IQueryable<T> source, String propertyName, String methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "p");
            PropertyInfo property = type.GetProperty(propertyName);
            Expression expr = Expression.Property(arg, property);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            return ((IOrderedQueryable<T>)(typeof(Queryable).GetMethods().Single(
                p => String.Equals(p.Name, methodName, StringComparison.Ordinal)
                    && p.IsGenericMethodDefinition
                    && p.GetGenericArguments().Length == 2
                    && p.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new Object[] { source, lambda })));
        }
    }
}
