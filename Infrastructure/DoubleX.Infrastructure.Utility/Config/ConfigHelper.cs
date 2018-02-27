using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 配置操作辅助类
    /// </summary>
    public class ConfigHelper
    {
        #region 配置节点获取

        /// <summary>
        /// 获取信息节点
        /// </summary>
        /// <param name="sectionName">节点对象名</param>
        /// <returns>返回Obj</returns>
        public static object GetSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                return null;
            }
            return ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// 获取信息节点
        /// </summary>
        /// <typeparam name="TEntity">信息类型</typeparam>
        /// <param name="sectionName">节点对象名</param>
        /// <returns>返回TEntity</returns>
        public static TEntity GetSection<TEntity>(string sectionName) where TEntity : class, new()
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = (new TEntity()).GetType().Name;
            }
            var configObj = ConfigurationManager.GetSection(sectionName);
            if (configObj == null)
                configObj = new TEntity();
            return (TEntity)configObj;
        }

        #endregion

        #region 配置集合获取

        /// <summary>
        /// 获取集合列表
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<ConfigListItem> GetDefaultItems(string sectionName)
        {
            return GetDefaultItems<ConfigListItem>(sectionName);
        }

        /// <summary>
        /// 获取集合列表
        /// </summary>
        /// <typeparam name="TEntity">集合项</typeparam>
        /// <param name="sectionName">节点名称</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<TEntity> GetDefaultItems<TEntity>(string sectionName) where TEntity : ConfigListItem, new()
        {
            List<TEntity> list = new List<TEntity>();
            var section = GetSection(sectionName) as ConfigModel<ConfigList<TEntity>>;
            if (section != null)
            {
                return GetDefaultItems<ConfigModel<ConfigList<TEntity>>, TEntity>(section);
            }
            return list;
        }

        /// <summary>
        /// 获取集合列表
        /// </summary>
        /// <typeparam name="TSection">节点类型</typeparam>
        /// <param name="section">节点</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<ConfigListItem> GetDefaultItems<TSection>(TSection section)
        {
            return GetDefaultItems<TSection, ConfigListItem>(section);
        }

        /// <summary>
        /// 获取集合列表
        /// </summary>
        /// <typeparam name="TEntity">集合项</typeparam>
        /// <typeparam name="TSection">节点类型</typeparam>
        /// <param name="section">节点</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<TEntity> GetDefaultItems<TSection, TEntity>(TSection section) where TEntity : ConfigListItem, new()
        {
            if (section == null)
                return new List<TEntity>();

            var sectionModel = section as ConfigModel<ConfigList<TEntity>>;
            if (sectionModel != null)
            {
                return (from i in sectionModel.Items.Cast<TEntity>() select i).ToList();
            }
            return new List<TEntity>();
        }



        /// <summary>
        /// 获取分组集合列表
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="groupKey">组Key 默认全部</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<ConfigListItem> GetGroupItems(string sectionName, string groupKey = null)
        {
            return GetGroupItems<ConfigGroupItem, ConfigListItem>(sectionName, groupKey);
        }

        /// <summary>
        /// 获取分组集合列表
        /// </summary>
        /// <typeparam name="TGroup">集合组类型 默认ConfigGroupItem</typeparam>
        /// <typeparam name="TItem">集合项 默认ConfigListItem</typeparam>
        /// <param name="sectionName">节点名称</param>
        /// <param name="groupKey">组Key 默认全部</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<TItem> GetGroupItems<TGroup, TItem>(string sectionName, string groupKey = null)
            where TGroup : ConfigGroupItem, new()
            where TItem : ConfigListItem, new()
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                return new List<TItem>();
            }

            var section = GetSection(sectionName) as ConfigModel<ConfigGroup<TGroup>>;
            if (section != null)
            {
                return GetGroupItems<ConfigModel<ConfigGroup<TGroup>>, TGroup, TItem>(section, groupKey);
            }
            return new List<TItem>();
        }

        /// <summary>
        /// 获取分组集合列表
        /// </summary>
        /// <typeparam name="TSection">节点类型</typeparam>
        /// <typeparam name="TGroup">集合组类型 默认ConfigGroupItem</typeparam>
        /// <typeparam name="TItem">集合项 默认ConfigListItem</typeparam>
        /// <param name="section">节点</param>
        /// <param name="groupKey">组Key 默认全部</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static List<TItem> GetGroupItems<TSection, TGroup, TItem>(TSection section, string groupKey = null)
            where TGroup : ConfigGroupItem, new()
            where TItem : ConfigListItem, new()
        {
            if (section == null)
            {
                return new List<TItem>();
            }

            List<TItem> list = new List<TItem>();
            var nodes = section as ConfigModel<ConfigGroup<TGroup>>;
            if (nodes != null)
            {
                var groupQuery = from i in nodes.Items.Cast<ConfigGroupItem>() select i;
                if (!string.IsNullOrWhiteSpace(groupKey))
                {
                    groupQuery = from i in groupQuery where i.Key.ToLower() == groupKey.ToLower() select i;
                }
                groupQuery.ToList().ForEach(x =>
                {
                    list.AddRange(x.Items.Cast<TItem>().ToList());
                });
            }
            return list;
        }

        #endregion

        #region 配置选项获取

        /// <summary>
        /// 获取集合列表
        /// </summary>
        /// <typeparam name="TEntity">集合项</typeparam>
        /// <typeparam name="TSection">节点类型</typeparam>
        /// <param name="section">节点</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static TEntity GetDefaultItem<TSection, TEntity>(TSection section, string key) where TEntity : ConfigListItem, new()
        {
            if (section == null)
                return default(TEntity);
            if (string.IsNullOrWhiteSpace(key))
                return default(TEntity);

            var items = GetDefaultItems<TSection, TEntity>(section);
            if (items != null)
            {
                return items.Where(x => x.Key.ToLower() == key.ToLower()).Cast<TEntity>().FirstOrDefault();
            }
            return default(TEntity);
        }

        /// <summary>
        /// 获取组配置项
        /// </summary>
        /// <typeparam name="TSection">节点类型</typeparam>
        /// <typeparam name="TGroup">集合组类型 默认ConfigGroupItem</typeparam>
        /// <typeparam name="TItem">集合项 默认ConfigListItem</typeparam>
        /// <param name="section">节点</param>
        /// <param name="groupKey">组Key 默认全部</param>
        /// <param name="key">节点键</param>
        /// <returns>返回List[TEntity]集合列表</returns>
        public static TItem GetGroupItem<TSection, TGroup, TItem>(TSection section, string groupKey, string key)
            where TGroup : ConfigGroupItem, new()
            where TItem : ConfigListItem, new()
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(TItem);

            var items = GetGroupItems<TSection, ConfigGroupItem, ConfigListItem>(section, groupKey);
            if (items != null)
            {
                return items.Where(x => x.Key.ToLower() == key.ToLower()).Cast<TItem>().FirstOrDefault();
            }
            return default(TItem);
        }

        #endregion

    }
}
