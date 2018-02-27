using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Mongo操作辅助类
    /// </summary>
    public static class MongoHelper
    {
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="url">MongoUrl</param>
        /// <returns>IMongoDatabase</returns>
        public static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>IMongoDatabase</returns>
        public static IMongoDatabase GetDatabaseFromUrl(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// 获取集合(表)
        /// </summary>
        /// <typeparam name="TEntity">集合对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> GetCollection<TEntity>(string connectionString) where TEntity : class
        {
            var url = new MongoUrl(connectionString);
            return GetCollection<TEntity>(url);
        }

        /// <summary>
        /// 获取集合(表)
        /// </summary>
        /// <typeparam name="TEntity">集合对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns>IMongoCollection</returns>
        public static IMongoCollection<TEntity> GetCollection<TEntity>(string connectionString, string collectionName) where TEntity : class
        {
            var url = new MongoUrl(connectionString);
            return GetCollection<TEntity>(url, collectionName);
        }

        /// <summary>
        /// 获取集合(表)
        /// </summary>
        /// <typeparam name="TEntity">集合对象</typeparam>
        /// <param name="url">MongoUrl</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> GetCollection<TEntity>(MongoUrl url) where TEntity : class
        {
            return GetCollection<TEntity>(url, GetCollectionName<TEntity>());
        }

        /// <summary>
        /// 获取集合(表)
        /// </summary>
        /// <typeparam name="TEntity">集合对象</typeparam>
        /// <param name="url">MongoUrl</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns></returns>
        public static IMongoCollection<TEntity> GetCollection<TEntity>(MongoUrl url, string collectionName) where TEntity : class
        {
            return GetDatabaseFromUrl(url).GetCollection<TEntity>(collectionName);
        }


        /// <summary>
        /// 根据集合对象获取集合名称
        /// </summary>
        /// <typeparam name="TEntity">集合对象</typeparam>
        /// <returns></returns>
        private static string GetCollectionName<TEntity>() where TEntity : class
        {
            string collectionName = GetCollectioNameFromInterface<TEntity>(); ;

            if (string.IsNullOrEmpty(collectionName)) {
                collectionName = typeof(TEntity).Name;
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collectionname from.</typeparam>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string GetCollectioNameFromInterface<TEntity>()
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(typeof(TEntity), typeof(MongoCollectionName));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((MongoCollectionName)att).Name;
            }
            else
            {
                collectionname = typeof(TEntity).Name;
            }

            return collectionname;
        }


        /// <summary>
        /// 根据集合类型获取集合名称
        /// </summary>
        /// <param name="entitytype">集合类型</param>
        /// <returns></returns>
        private static string GetCollectionNameFromType<ITEntity>(Type entitytype)
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(entitytype, typeof(MongoCollectionName));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((MongoCollectionName)att).Name;
            }
            else
            {
                if (typeof(ITEntity).IsAssignableFrom(entitytype))
                {
                    // No attribute found, get the basetype
                    while (!entitytype.BaseType.Equals(typeof(ITEntity)))
                    {
                        entitytype = entitytype.BaseType;
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }

    }
}
