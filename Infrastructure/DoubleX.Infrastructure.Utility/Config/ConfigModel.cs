using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DoubleX.Infrastructure.Utility
{
    #region 配置的默认实现

    /// <summary>
    /// 默认节点实现(集合列表)
    /// </summary>
    public class DefaultConfigSection : ConfigModel<ConfigList<ConfigListItem>>
    {
    }

    /// <summary>
    /// 默认节点实现(集合分组)
    /// </summary>
    public class GroupSection : ConfigModel<ConfigGroup<ConfigGroupItem>>
    {
        [ConfigurationProperty("Groups")]
        public override ConfigGroup<ConfigGroupItem> Items
        {
            get { return this["Groups"] as ConfigGroup<ConfigGroupItem>; }
        }
    }

    #endregion

    #region 自定义配置实现

    /// <summary>
    /// 节点定义
    /// </summary>
    /// <typeparam name="TEntity">集合类型</typeparam>
    public class ConfigModel<TEntity> : ConfigurationSection
    {
        protected static readonly ConfigurationProperty defaultProperty =
            new ConfigurationProperty(string.Empty, typeof(TEntity), null,
                                ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public virtual TEntity Items
        {
            get
            {

                return (TEntity)base[defaultProperty];
            }
        }
    }

    /// <summary>
    /// 集合列表
    /// </summary>
    /// <typeparam name="TItem">集合对象/选项 类型</typeparam>
    public class ConfigList<TItem> : ConfigurationElementCollection where TItem : ConfigListItem, new()
    {
        // 基本上，所有的方法都只要简单地调用基类的实现就可以了。
        public ConfigList() : base(StringComparer.OrdinalIgnoreCase)// 忽略大小写
        {
        }

        // 下面二个方法中抽象类中必须要实现的。
        protected override ConfigurationElement CreateNewElement()
        {
            return new TItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TItem)element).Key;
        }

        // 其实关键就是这个索引器。但它也是调用基类的实现，只是做下类型转就行了。
        new public TItem this[string name]
        {
            get
            {
                return (TItem)base.BaseGet(name);
            }
        }

        // 说明：如果不需要在代码中修改集合，可以不实现Add， Clear， Remove
        public void Add(TItem model)
        {
            ConfigurationElement element = model as ConfigurationElement;
            this.BaseAdd(element);
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
    }

    /// <summary>
    /// 集合选项(每个对象必须有Key Value 属性)
    /// </summary>
    public class ConfigListItem : ConfigurationElement
    {
        public ConfigListItem() { }

        [ConfigurationProperty("key")] //[ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"].ToString(); }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
    }

    /// <summary>
    /// 集合分组
    /// </summary>
    public class ConfigGroup<TItem> : ConfigurationElementCollection where TItem : ConfigGroupItem, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigGroupItem();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigGroupItem)element).Key;
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }
        protected override string ElementName
        {
            get { return "Item"; }
        }
    }

    /// <summary>
    /// 分组对象 选项(每个对象必须有Key 及 CofnigList)
    /// </summary>
    public class ConfigGroupItem : ConfigurationElement
    {
        protected static readonly ConfigurationProperty itemsProperty =
                new ConfigurationProperty(string.Empty, typeof(ConfigList<ConfigListItem>), null,
                                    ConfigurationPropertyOptions.IsDefaultCollection);

        public ConfigGroupItem() : base() { }

        [ConfigurationProperty("key")] //[ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"].ToString(); }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public virtual ConfigList<ConfigListItem> Items
        {
            get
            {

                return (ConfigList<ConfigListItem>)base[itemsProperty];
            }
        }
    }

    #endregion

    #region 配置示例

    //<ConfigSection>
    //    <add key = "a1" value="test1"></add>
    //    <add key = "a2" value="test2"></add>
    //    <add key = "a3" value="test3"></add>
    //    <add key = "a4" value="test4"></add>
    //  </ConfigSection>-->
    //  <ConfigSection>
    //    <Groups>
    //      <Item key = "KeyA" >
    //        < add key="a1" value="testA1"></add>
    //        <add key = "a2" value="testA2"></add>
    //      </Item>
    //      <Item key = "KeyB" >
    //        < add key="b1" value="testB1"></add>
    //        <add key = "b2" value="testB2"></add>
    //      </Item>
    //    </Groups>
    //  </ConfigSection>
    #endregion

}
