using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Entity;
using System.Reflection;

namespace DoubleX.Infrastructure.Core.Repository
{
    public class EFRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityFrameworkEntity, IEntity<TKey>
    {
        #region 类属性(MongoCollection)

        /// <summary>
        /// EF数据上下文
        /// </summary>
        protected internal DbContext dbContext;
        
        #endregion

        #region 构造方法（属性设置）

        public EFRepository(DbContext context)
        {
            dbContext = context;
        }

        #endregion

        #region 添加对象/集合

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">对象</param>
        public void Insert(TEntity entity)
        {
            if (!VerifyHelper.IsEmpty(dbContext))
            {
                this.dbContext.Entry<TEntity>(entity).State = EntityState.Added;
                // this.dbContext.Set<TEntity>().Add(entity);此方法同上方法
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="list">集合</param>
        public void Insert(IEnumerable<TEntity> list)
        {
            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(list))
            {
                foreach (var item in list)
                {
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Added;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 添加集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkInsert(IEnumerable<TEntity> list) { }

        #endregion

        #region 修改对象/集合

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity">对象</param>
        public void Update(TEntity entity)
        {
            if (!VerifyHelper.IsEmpty(dbContext))
            {
                this.dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>void</returns>
        public void Update(IEnumerable<TEntity> list)
        {
            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(list))
            {
                foreach (var item in list)
                {
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Modified;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 修改集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkUpdate(IEnumerable<TEntity> list) { }

        #endregion

        #region 删除对象/集合

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">Id</param>
        public void Delete(TKey id)
        {
            Guid entityId = GuidHelper.Get(id);
            if (!VerifyHelper.IsEmpty(entityId))
            {
                var entity = this.dbContext.Set<TEntity>().FirstOrDefault(X => X.Id == entityId);
                if (!VerifyHelper.IsEmpty(entity))
                {
                    this.dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                    this.dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public void Delete(TEntity entity)
        {

            if (!VerifyHelper.IsEmpty(dbContext))
            {
                this.dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="list">集合</param>
        public void Delete(IEnumerable<TEntity> list)
        {

            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(list))
            {
                foreach (var item in list)
                {
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Deleted;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {

            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(predicate))
            {
                foreach (var item in this.dbContext.Set<TEntity>().Where(predicate))
                {
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Deleted;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkDelete(IEnumerable<TEntity> list) { }

        #endregion

        #region 移除对象/集合

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="id">Id</param>
        public void Remove(TKey id)
        {
            Guid entityId = GuidHelper.Get(id);
            if (!VerifyHelper.IsEmpty(entityId))
            {
                var entity = this.dbContext.Set<TEntity>().FirstOrDefault(X => X.Id == entityId);
                if (!VerifyHelper.IsEmpty(entity))
                {
                    entity.IsDelete = true;
                    this.dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
                    this.dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public void Remove(TEntity entity)
        {
            if (!VerifyHelper.IsEmpty(dbContext))
            {
                entity.IsDelete = true;
                this.dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="list">集合</param>
        public void Remove(IEnumerable<TEntity> list)
        {
            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(list))
            {
                foreach (var item in list)
                {
                    item.IsDelete = true;
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Modified;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            if (!VerifyHelper.IsEmpty(dbContext) && !VerifyHelper.IsEmpty(predicate))
            {
                var query = this.dbContext.Set<TEntity>().Where(predicate);
                foreach (var item in query)
                {
                    item.IsDelete = true;
                    this.dbContext.Entry<TEntity>(item).State = EntityState.Modified;
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 移除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkRemove(IEnumerable<TEntity> list) { }

        #endregion

        #region 获取对象/集合

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">Id</param>
        public TEntity Get(TKey id)
        {
            Guid entityId = GuidHelper.Get(id);
            if (!VerifyHelper.IsEmpty(entityId))
            {
                    return this.dbContext.Set<TEntity>().FirstOrDefault(x => x.Id == entityId);
            }
            return null;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns>TEntity 对象 or null</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            long count = 0;
            return Query(0, 1, predicate, null, out count).FirstOrDefault();
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting = null)
        {
            long count = 0;
            return Query(0, 0, predicate, sorting, out count);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="top">数量</param>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public List<TEntity> Query(int top, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting)
        {
            long count = 0;
            return Query(0, top, predicate, sorting, out count);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="pageIndex">获取第几页 从0开始(小于0时强制为0)</param>
        /// <param name="pageSize">每页数量 为0时所有</param>
        /// <param name="predicate">表达式</param>
        /// <param name="sorting">排序</param>
        /// <param name="count">数据总数</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public List<TEntity> Query(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string, string>> sorting, out long total)
        {
            IQueryable<TEntity> query = this.dbContext.Set<TEntity>();

            List<TEntity> list = new List<TEntity>();

            //条件
            if (VerifyHelper.IsEmpty(predicate))
            {
                query = query.Where(x => true);
            }
            else
            {
                query = query.Where(predicate);
            }
            query = query.Where(x => x.IsDelete == false);

            //总数
            total = query.Count();

            //排序
            if (sorting == null || (sorting != null && sorting.Count == 0))
            {
                sorting = new List<KeyValuePair<string, string>>();
                PropertyInfo property = typeof(TEntity).GetProperty("Sort");
                if (property != null)
                {
                    sorting.Add(new KeyValuePair<string, string>("Sort", "desc"));
                }
                sorting.Add(new KeyValuePair<string, string>("LastDt", "desc"));
            }
            query = LinqHelper.Sorting<TEntity>(query, sorting);

            #region  排序完整的先注释

            //var idx = 0;
            //sorting.ForEach(x =>
            //{
            //    idx++;
            //    if (idx == 1)
            //    {
            //        query = sort.IsAscending ? query.SortBy(x=>x.CreateDt) : data.SortByDescending(sort.Field);
            //    }
            //    else
            //    {
            //        var sortByData = data as IOrderedFindFluent<T, T>;
            //        if (sortByData == null)
            //        {
            //            continue;
            //        }

            //        data = sort.IsAscending ? sortByData.ThenBy(sort.Field) : sortByData.ThenByDescending(sort.Field);
            //    }

            //});

            #endregion

            #region  分页,默认从0开始，pageSize=0时所有

            //计算跳过数
            int skipCount = 0;
            if (pageIndex <= 0) { pageIndex = 0; }
            if (pageSize > 0) { skipCount = pageIndex * pageSize; }
            if (skipCount > 0)
            {
                query = query.Skip(skipCount);
            }

            //如果pageSize>0获取指定数量
            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }

            #endregion

            list = query.ToList();

            if (list == null) { list = new List<TEntity>(); }

            return list;
        }

        #endregion
        
    }
}
