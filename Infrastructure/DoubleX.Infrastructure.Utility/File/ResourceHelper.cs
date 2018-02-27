using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 资资源文件处理辅助类
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        /// 资源文件转为Llist集合
        /// </summary>
        /// <param name="type">资源文件Type</param>
        /// <param name="manager">资源文件ResourceManager</param>
        /// <param name="culture">当前区域标识</param>
        /// <returns>返回List[ItemModel]</returns>
        public static List<KeyValuePair<string,string>> GetList(Type type, ResourceManager manager, CultureInfo culture = null)
        {

            if (type == null)
            {
                return new List<KeyValuePair<string, string>>();
            }

            if (culture == null)
            {
                culture = CultureHelper.GetCulture();
            }

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ToList().ForEach(x =>
            {
                list.Add(new KeyValuePair<string, string>(x.Name, manager != null ? manager.GetString(x.Name, culture) : x.GetValue(null,null).ToString()));
            });

            return list;
        }
    }
}
