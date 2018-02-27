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
    /// 模版内容配置文件操作
    /// </summary>
    public class TemplateConfig : AbsConfigs<TemplateConfigModel>
    {
        /// <summary>
        /// 初始操作
        /// </summary>
        static TemplateConfig()
        {
            Init(KeyModel.Config.Template.SectionName, KeyModel.Cache.Template);
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置key</param>
        /// <param name="groupKey">组Key</param>
        /// <returns>返回值</returns>
        public static string GetValue(string key)
        {
            var item = GetDefaultItem(KeyModel.Cache.Template, key, null);
            if (VerifyHelper.IsEmpty(item))
            {
                return null;
            }
            return VerifyHelper.IsEmpty(item.Value) ? "" : item.Value;
        }
    }

    /// <summary>
    /// 模版内容配置文件对象
    /// </summary>
    public class TemplateConfigModel : DefaultConfigSection
    {
    }
}
