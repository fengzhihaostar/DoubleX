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
    /// 项目设置配置文件操作
    /// </summary>
    public class SettingConfig : AbsConfigs<SettingConfigModel>
    {
        /// <summary>
        /// 初始操作
        /// </summary>
        static SettingConfig()
        {
            Init(KeyModel.Config.Setting.SectionName, KeyModel.Cache.Setting);
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置key</param>
        /// <param name="groupKey">组Key</param>
        /// <returns>返回值</returns>
        public static string GetValue(string key, string groupKey = null)
        {
            var item = GetGroupItem(KeyModel.Cache.Setting, key, groupKey);
            if (VerifyHelper.IsEmpty(item))
            {
                return null;
            }
            return VerifyHelper.IsEmpty(item.Value) ? "" : item.Value;
        }
    }

    /// <summary>
    /// 项目设置配置文件对象
    /// </summary>
    public class SettingConfigModel : GroupSection
    {
    }
}
