using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Entity;
using DoubleX.Infrastructure.Core.Config;

namespace DoubleX.Infrastructure.Core.Repository
{
    /// <summary>
    /// Mongo数据持久化操作
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TKey">主键对象</typeparam>
    public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey>, IQueryable<TEntity> where TEntity : MongoEntity, IEntity<TKey>
    {
        #region 类属性(MongoCollection)

        /// <summary>
        /// Mongo集合 字段.
        /// </summary>
        protected internal IMongoCollection<TEntity> collection;

        /// <summary>
        ///  Mongo集合
        /// </summary>
        public IMongoCollection<TEntity> Collection
        {
            get { return this.collection; }
        }

        /// <summary>
        /// 当前集合过滤器
        /// </summary>
        public FilterDefinitionBuilder<TEntity> Filter
        {
            get
            {
                return Builders<TEntity>.Filter;
            }
        }

        ///// <summary>
        ///// projector for collection
        ///// </summary>
        //public ProjectionDefinitionBuilder<T> Project
        //{
        //    get
        //    {
        //        return Builders<T>.Projection;
        //    }
        //}

        /// <summary>
        /// 当前集合更新器
        /// </summary>
        public UpdateDefinitionBuilder<TEntity> Updater
        {
            get
            {
                return Builders<TEntity>.Update;
            }
        }

        #endregion

        #region 构造方法（属性设置）

        public MongoRepository()
            : this(SettingConfig.GetValue(KeyModel.Config.Setting.KeyMongoDefault, KeyModel.Config.Setting.GroupDatabase))
        {
        }

        public MongoRepository(string connectionString)
        {
            this.collection = MongoHelper.GetCollection<TEntity>(connectionString);
        }

        public MongoRepository(string connectionString, string collectionName)
        {
            this.collection = MongoHelper.GetCollection<TEntity>(connectionString, collectionName);
        }

        public MongoRepository(MongoUrl url)
        {
            this.collection = MongoHelper.GetCollection<TEntity>(url);
        }

        public MongoRepository(MongoUrl url, string collectionName)
        {
            this.collection = MongoHelper.GetCollection<TEntity>(url, collectionName);
        }

        #endregion

        #region 添加对象/集合

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Insert(TEntity entity)
        {
            this.collection.InsertOne(entity);
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Insert(IEnumerable<TEntity> list)
        {
            this.collection.InsertMany(list);
        }

        /// <summary>
        /// 添加集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkInsert(IEnumerable<TEntity> list)
        {
            var buldModes = new List<WriteModel<TEntity>>();
            foreach (var item in list)
            {
                buldModes.Add(new InsertOneModel<TEntity>(item));
            }
            this.collection.BulkWriteAsync(buldModes).Wait();
        }

        #endregion

        #region 修改对象/集合

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Update(TEntity entity)
        {
            var updated = Updater.Combine(GeneratorMongoUpdate(entity));
            this.collection.UpdateOne(Filter.Eq("_id", new ObjectId(entity.Id as string)), updated);
        }

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Update(IEnumerable<TEntity> list)
        {
            foreach (var item in list)
            {
                this.Update(item);
            }
        }

        /// <summary>
        /// 修改集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public void BulkUpdate(IEnumerable<TEntity> list)
        {
            var bulkModes = new List<WriteModel<TEntity>>();
            foreach (var item in list)
            {
                bulkModes.Add(new UpdateOneModel<TEntity>(Filter.Eq("_id", item.Id), Builders<TEntity>.Update.Combine(GeneratorMongoUpdate(item))));
            }
            this.collection.BulkWriteAsync(bulkModes).Wait();
        }

        #endregion

        #region 删除对象/集合

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(TKey id)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(MongoEntity)))
            {
                this.Delete(Filter.Eq("_id", new ObjectId(id as string)));
            }
            else
            {
                this.Delete(Filter.Eq("_id", BsonValue.Create(id)));
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Delete(TEntity entity)
        {
            this.Delete(entity.Id);
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Delete(IEnumerable<TEntity> list)
        {
            this.collection.DeleteMany(x => list.Select(y => y.Id).Contains(x.Id));
        }

        /// <summary>
        /// 删除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkDelete(IEnumerable<TEntity> list)
        {
            var buldModes = new List<WriteModel<TEntity>>();
            foreach (var item in list)
            {
                buldModes.Add(new DeleteOneModel<TEntity>(Filter.Eq(x => x.Id, item.Id)));
            }
            this.collection.BulkWriteAsync(buldModes).Wait();
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            this.Collection.DeleteMany(predicate);
        }


        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(ObjectId id)
        {
            this.Delete(Builders<TEntity>.Filter.Eq("_id", id));
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="filter">筛选器</param>
        public virtual void Delete(FilterDefinition<TEntity> filter)
        {
            this.collection.DeleteMany(filter);
        }

        #endregion

        #region 移除对象/集合

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Remove(TKey id)
        {

        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="entity">对象</param>
        public virtual void Remove(TEntity entity)
        {

        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void Remove(IEnumerable<TEntity> list)
        {
        }

        /// <summary>
        /// 移除集合(超大集合)
        /// </summary>
        /// <param name="list">集合</param>
        public virtual void BulkRemove(IEnumerable<TEntity> list)
        {
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="predicate">表达式</param>
        public virtual void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            this.Collection.UpdateMany(predicate, Builders<TEntity>.Update.Set("IsDelete", true));
        }


        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Remove(ObjectId id)
        {
            this.collection.UpdateOne(Builders<TEntity>.Filter.Eq("_id", id), Builders<TEntity>.Update.Set("IsDelete", true));
        }

        /// <summary>
        /// 移除集合
        /// </summary>
        /// <param name="filter">筛选器</param>
        public virtual void Remove(FilterDefinition<TEntity> filter)
        {
            this.collection.UpdateMany(filter, Builders<TEntity>.Update.Set("IsDelete", true));
        }

        #endregion

        #region 获取对象/集合

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public TEntity Get(TKey id)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(MongoEntity)))
            {
                return this.collection.Find(Filter.Eq("_id", new ObjectId(id as string))).FirstOrDefault();
            }
            else
            {
                return this.collection.Find(Filter.Eq("_id", BsonValue.Create(id))).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="predicate">表格式</param>
        /// <returns>TEntity 对象 or null</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            long count = 0;
            return Query(0, 1, predicate, null, out count).FirstOrDefault();
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string,string>> sorting = null)
        {
            long count = 0;
            return Query(0, 0, predicate, sorting, out count);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="top">数量</param>
        /// <param name="predicate">条件</param>
        /// <param name="sorting">排序</param>
        /// <returns>IQueryable[TEntity] 集合 or null </returns>
        public List<TEntity> Query(int top, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string,string>> sorting)
        {
            long count = 0;
            return Query(0, top, predicate, sorting, out count);
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
        public List<TEntity> Query(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, List<KeyValuePair<string,string>> sorting, out long count)
        {
            List<TEntity> list = new List<TEntity>();

            //默认条件
            if (predicate == null) { predicate = x => true; }
            predicate = predicate.And(x => x.IsDelete == false);

            //总数
            count = this.collection.Count(predicate);

            //初始
            var query = this.collection.Find(predicate);

            //排序
            if (sorting == null || (sorting != null && sorting.Count == 0))
            {
                SortDefinition<TEntity> sorts = new ObjectSortDefinition<TEntity>(new { });
                PropertyInfo property = typeof(TEntity).GetProperty("Sort");
                if (property != null)
                {
                    sorts = Builders<TEntity>.Sort.Descending("Sort");
                }
                sorts = sorts.Descending(x => x.CreateDt);
                query = query.Sort(sorts);
            }

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

            query = query.Skip(skipCount);

            //如果pageSize>0获取指定数量
            if (pageSize > 0)
            {
                query = query.Limit(pageSize);
            }

            #endregion

            list = query.ToList();

            if (list == null) { list = new List<TEntity>(); }

            return list;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 构建Mongo的更新表达式
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private List<UpdateDefinition<TEntity>> GeneratorMongoUpdate(TEntity item)
        {
            var fieldList = new List<UpdateDefinition<TEntity>>();
            
            var query = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => !(x.GetCustomAttributes(typeof(BsonIgnoreAttribute), true).Any()));
            foreach (var property in query)
            {
                GenerateRecursion(fieldList, property, property.GetValue(item), item, string.Empty);
            }
            return fieldList;
        }

        /// <summary>
        /// 递归构建Update操作串
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="property"></param>
        /// <param name="propertyValue"></param>
        /// <param name="item"></param>
        /// <param name="father"></param>
        private void GenerateRecursion(List<UpdateDefinition<TEntity>> fieldList, PropertyInfo property, object propertyValue, TEntity item, string father)
        {
            //复杂类型
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string) && propertyValue != null)
            {
                //集合
                if (typeof(IList).IsAssignableFrom(propertyValue.GetType()))
                {
                    foreach (var sub in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (sub.PropertyType.IsClass && sub.PropertyType != typeof(string))
                        {
                            var arr = propertyValue as IList;
                            if (arr != null && arr.Count > 0)
                            {
                                for (int index = 0; index < arr.Count; index++)
                                {
                                    foreach (var subInner in sub.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                                    {
                                        if (string.IsNullOrWhiteSpace(father))
                                            GenerateRecursion(fieldList, subInner, subInner.GetValue(arr[index]), item, property.Name + "." + index);
                                        else
                                            GenerateRecursion(fieldList, subInner, subInner.GetValue(arr[index]), item, father + "." + property.Name + "." + index);
                                    }
                                }
                            }
                        }
                    }
                }
                //实体
                else
                {
                    foreach (var sub in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {

                        if (string.IsNullOrWhiteSpace(father))
                            GenerateRecursion(fieldList, sub, sub.GetValue(propertyValue), item, property.Name);
                        else
                            GenerateRecursion(fieldList, sub, sub.GetValue(propertyValue), item, father + "." + property.Name);
                    }
                }
            }
            //简单类型
            else
            {
                //typeof(TEntity).GetProperties()[xx].GetCustomAttribute(typeof(BsonIdAttribute))
                if (property.GetCustomAttribute(typeof(BsonIdAttribute)) == null)//更新集中不能有实体键_id
                {
                    if (string.IsNullOrWhiteSpace(father))
                        fieldList.Add(Builders<TEntity>.Update.Set(property.Name, propertyValue));
                    else
                        fieldList.Add(Builders<TEntity>.Update.Set(father + "." + property.Name, propertyValue));
                }
            }
        }

        #endregion

        #region IQueryable<TEntity>
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<TEntity> GetEnumerator()
        {
            return this.collection.AsQueryable<TEntity>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable<TEntity>().GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQueryable is executed.
        /// </summary>
        public virtual Type ElementType
        {
            get { return this.collection.AsQueryable<TEntity>().ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of IQueryable.
        /// </summary>
        public virtual Expression Expression
        {
            get { return this.collection.AsQueryable<TEntity>().Expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public virtual IQueryProvider Provider
        {
            get { return this.collection.AsQueryable<TEntity>().Provider; }
        }
        #endregion
    }

    /// <summary>
    /// Mongo数据持久化操作
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    public class MongoRepository<TEntity> : MongoRepository<TEntity, string>, IRepository<TEntity> where TEntity : MongoEntity, IEntity<string>
    {
        public MongoRepository()
            : base()
        {
        }

        public MongoRepository(MongoUrl url)
            : base(url) { }

        public MongoRepository(MongoUrl url, string collectionName)
            : base(url, collectionName) { }

        public MongoRepository(string connectionString)
            : base(connectionString) { }

        public MongoRepository(string connectionString, string collectionName)
            : base(connectionString, collectionName) { }
    }
}
