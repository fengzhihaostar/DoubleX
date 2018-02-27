using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Infrastructure.Core.Config
{
    /// <summary>
    /// 数据中心配置文件操作
    /// </summary>
    public class DataCenterConfig : AbsConfigs<DataCenterConfigModel>
    {
        /// <summary>
        /// 初始操作
        /// </summary>
        static DataCenterConfig()
        {
            Init(KeyModel.Config.DataCenter.SectionName, KeyModel.Cache.DataCenter);
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置key</param>
        /// <param name="groupKey">组Key</param>
        /// <returns>返回值</returns>
        public static string GetValue(string key)
        {
            var item = GetDefaultItem(KeyModel.Cache.DataCenter, key, null);
            if (VerifyHelper.IsEmpty(item))
            {
                return null;
            }
            return VerifyHelper.IsEmpty(item.Value) ? "" : item.Value;
        }

        /// <summary>
        /// 获取数据中心ApiUrl
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetApiUrl(string key)
        {
            var baseUrl = StringHelper.Get(GetValue(KeyModel.Config.DataCenter.KeyApiAddress));
            var keyValue = StringHelper.Get(GetValue(key));
            return string.Format("{0}{1}", baseUrl, keyValue);
        }
    }

    /// <summary>
    /// 项目设置配置文件对象
    /// </summary>
    public class DataCenterConfigModel : DefaultConfigSection
    {
    }
}
