using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Entity;
using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Infrastructure.Core.Model;
using System.Linq.Expressions;

namespace DoubleX.Infrastructure.Core.Service
{
    /// <summary>
    /// 业务操作基类
    /// </summary>
    public interface IService<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        #region 添加对象/集合

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">对象</param>
        void Insert(TEntity entity);

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="list">集合</param>
        void Insert(IEnumerable<TEntity> list);

        /// <summary>
        /// 添加集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        void BulkInsert(IEnumerable<TEntity> list);

        #endregion

        #region 修改对象/集合

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity">对象</param>
        void Update(TEntity entity);

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>void</returns>
        void Update(IEnumerable<TEntity> list);

        /// <summary>
        /// 修改集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        void BulkUpdate(IEnumerable<TEntity> list);

        #endregion

        #region 删除对象/集合

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(TKey id);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">对象</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="list">集合</param>
        void Delete(IEnumerable<TEntity> list);

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 删除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        void BulkDelete(IEnumerable<TEntity> list);

        #endregion

        #region 移除对象/集合

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="id">Id</param>
        void Remove(TKey id);

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        void Remove(TEntity entity);

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="list">集合</param>
        void Remove(IEnumerable<TEntity> list);

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        void Remove(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 移除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        void BulkRemove(IEnumerable<TEntity> list);

        #endregion

        #region 获取对象/集合

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">Id</param>
        TEntity Get(TKey id);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns>TEntity 对象 or null</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting = null);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="top">数量</param>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        List<TEntity> Query(int top, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="predicate">条件</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        List<TEntity> Query(RequestQueryModel queryModel, Expression<Func<TEntity, bool>> predicate, out long count);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="pageIndex">获取第几页 从0开始(小于0时强制为0)</param>
        /// <param name="pageSize">每页数量 为0时所有</param>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <param name="count">数据总数</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        List<TEntity> Query(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting, out long count);

        #endregion
    }
}
