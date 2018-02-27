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
    //public class DefaultService<TEntity, TKey> : DefaultService<IRepository<TEntity, TKey>, TEntity, TKey>
    //    where TEntity : class, IEntity<TKey>
    //{
    //    public DefaultService(IRepository<TEntity, TKey> rep)
    //    {
    //        repository = rep;
    //    }
    //}

    public class DefaultService<TRepository, TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TRepository : IRepository<TEntity, TKey>
    {
        /// <summary>
        /// 操作仓储
        /// </summary>
        protected TRepository repository { get; set; }

        public DefaultService(TRepository rep)
        {
            repository = rep;
        }

        #region 添加对象/集合

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Insert(TEntity entity)
        {
            repository.Insert(entity);
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Insert(IEnumerable<TEntity> list)
        {
            repository.Insert(list);
        }

        /// <summary>
        /// 添加集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkInsert(IEnumerable<TEntity> list)
        {
            repository.BulkInsert(list);
        }

        #endregion

        #region 修改对象/集合

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Update(TEntity entity)
        {
            repository.Update(entity);
        }

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Update(IEnumerable<TEntity> list)
        {
            repository.Update(list);
        }

        /// <summary>
        /// 修改集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkUpdate(IEnumerable<TEntity> list)
        {
            repository.BulkUpdate(list);
        }

        #endregion

        #region 删除对象/集合

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(TKey id)
        {
            repository.Delete(id);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Delete(TEntity entity)
        {
            repository.Delete(entity);
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Delete(IEnumerable<TEntity> list)
        {
            repository.Delete(list);
        }

        /// <summary>
        /// 删除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkDelete(IEnumerable<TEntity> list)
        {
            repository.BulkDelete(list);
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            repository.Delete(predicate);
        }

        #endregion

        #region 移除对象/集合

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Remove(TKey id)
        {
            repository.Remove(id);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Remove(TEntity entity)
        {
            repository.Remove(entity);
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Remove(IEnumerable<TEntity> list)
        {
            repository.Remove(list);
        }

        /// <summary>
        /// 移除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkRemove(IEnumerable<TEntity> list)
        {
            repository.BulkRemove(list);
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public virtual void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            repository.Remove(predicate);
        }


        #endregion

        #region 获取对象/集合

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">Id</param>
        public virtual TEntity Get(TKey id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="predicate">表格式</param>
        /// <returns>TEntity 对象 or null</returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return repository.Get(predicate);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public virtual List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting = null)
        {
            return repository.Query(predicate, sorting);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="top">数量</param>
        /// <param name="predicate">条件</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public virtual List<TEntity> Query(int top, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting)
        {
            return repository.Query(top, predicate, sorting);
        }


        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="queryModel">查询请求实体</param>
        /// <param name="predicate">条件</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        public virtual List<TEntity> Query(RequestQueryModel queryModel, Expression<Func<TEntity, bool>> predicate, out long count)
        {
            int pageIndex = 0;
            int pageSize = 10;
            List<KeyValuePair<string, string>> sorting = null;
            if (!VerifyHelper.IsNull(queryModel))
            {
                pageIndex = queryModel.PageIndex;
                pageSize = queryModel.PageSize;
                sorting = queryModel.Sorting;
            }
            return Query(pageIndex, pageSize, predicate, sorting, out count);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="pageIndex">获取第几页 从0开始(小于0时强制为0)</param>
        /// <param name="pageSize">每页数量 为0时所有</param>
        /// <param name="predicate">条件</param>
        /// <param name="sorting">排序</param>
        /// <param name="count">条件数据总数</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public virtual List<TEntity> Query(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting, out long count)
        {
            return repository.Query(pageIndex, pageSize, predicate, sorting, out count);
        }

        #endregion

    }
}
#region 
/**

/// <summary>
/// 业务操作的默认实现
/// </summary>
public class DefaultService<TEntity, TKey> : IService<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// 操作仓储
    /// </summary>
    private IRepository<TEntity, TKey> repository { get; set; }


    public DefaultService(IRepository<TEntity, TKey> rep)
    {
        repository = rep;
    }


    #region 添加对象/集合

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="entity">对象</param>
    public virtual void Insert(TEntity entity)
    {
        repository.Insert(entity);
    }

    /// <summary>
    /// 添加集合
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void Insert(IEnumerable<TEntity> list)
    {
        repository.Insert(list);
    }

    /// <summary>
    /// 添加集合(超大集合)
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void BulkInsert(IEnumerable<TEntity> list)
    {
        repository.BulkInsert(list);
    }

    #endregion

    #region 修改对象/集合

    /// <summary>
    /// 修改对象
    /// </summary>
    /// <param name="entity">对象</param>
    public virtual void Update(TEntity entity)
    {
        repository.Update(entity);
    }

    /// <summary>
    /// 修改集合
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void Update(IEnumerable<TEntity> list)
    {
        repository.Update(list);
    }

    /// <summary>
    /// 修改集合(超大集合)
    /// </summary>
    /// <param name="list">集合</param>
    public void BulkUpdate(IEnumerable<TEntity> list)
    {
        repository.BulkUpdate(list);
    }

    #endregion

    #region 删除对象/集合

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="id">id</param>
    public virtual void Delete(TKey id)
    {
        repository.Delete(id);
    }

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="entity">对象</param>
    public virtual void Delete(TEntity entity)
    {
        repository.Delete(entity);
    }

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void Delete(IEnumerable<TEntity> list)
    {
        repository.Delete(list);
    }

    /// <summary>
    /// 删除集合(超大集合)
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void BulkDelete(IEnumerable<TEntity> list)
    {
        repository.BulkDelete(list);
    }

    /// <summary>
    /// 删除集合
    /// </summary>
    /// <param name="predicate">表达式</param>
    public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        repository.Delete(predicate);
    }

    #endregion

    #region 移除对象/集合

    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="id">id</param>
    public virtual void Remove(TKey id)
    {
        repository.Remove(id);
    }

    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="entity">对象</param>
    public virtual void Remove(TEntity entity)
    {
        repository.Remove(entity);
    }

    /// <summary>
    /// 移除集合
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void Remove(IEnumerable<TEntity> list)
    {
        repository.Remove(list);
    }

    /// <summary>
    /// 移除集合(超大集合)
    /// </summary>
    /// <param name="list">集合</param>
    public virtual void BulkRemove(IEnumerable<TEntity> list)
    {
        repository.BulkRemove(list);
    }

    /// <summary>
    /// 移除集合
    /// </summary>
    /// <param name="predicate">表达式</param>
    public virtual void Remove(Expression<Func<TEntity, bool>> predicate)
    {
        repository.Remove(predicate);
    }


    #endregion

    #region 获取对象/集合

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="id">Id</param>
    public virtual TEntity Get(TKey id)
    {
        return repository.Get(id);
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="predicate">表格式</param>
    /// <returns>TEntity 对象 or null</returns>
    public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
    {
        return repository.Get(predicate);
    }

    /// <summary>
    /// 获取集合
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="sorting">排序</param>
    /// <returns>IQueryable[TEntity] 集合 or null </returns>
    public virtual List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting = null)
    {
        return repository.Query(predicate, sorting);
    }

    /// <summary>
    /// 获取集合
    /// </summary>
    /// <param name="top">数量</param>
    /// <param name="predicate">条件</param>
    /// <param name="sorting">排序</param>
    /// <returns>IQueryable[TEntity] 集合 or null </returns>
    public virtual List<TEntity> Query(int top, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting)
    {
        return repository.Query(top, predicate, sorting);
    }


    /// <summary>
    /// 获取集合
    /// </summary>
    /// <param name="queryModel">查询请求实体</param>
    /// <param name="predicate">条件</param>
    /// <param name="count">数据总数</param>
    /// <returns></returns>
    public virtual List<TEntity> Query(RequestQueryModel queryModel, Expression<Func<TEntity, bool>> predicate, out long count)
    {
        int pageIndex = 0;
        int pageSize = 10;
        List<KeyValuePair<string, string>> sorting = null;
        if (!VerifyHelper.IsNull(queryModel))
        {
            pageIndex = queryModel.PageIndex;
            pageSize = queryModel.PageSize;
            sorting = queryModel.Sorting;
        }
        return Query(pageIndex, pageSize, predicate, sorting, out count);
    }

    /// <summary>
    /// 获取集合
    /// </summary>
    /// <param name="pageIndex">获取第几页 从0开始(小于0时强制为0)</param>
    /// <param name="pageSize">每页数量 为0时所有</param>
    /// <param name="predicate">条件</param>
    /// <param name="sorting">排序</param>
    /// <param name="count">条件数据总数</param>
    /// <returns>IQueryable[TEntity] 集合 or null </returns>
    public virtual List<TEntity> Query(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting, out long count)
    {
        return repository.Query(pageIndex, pageSize, predicate, sorting, out count);
    }

    #endregion

}
}



**/

#endregion
