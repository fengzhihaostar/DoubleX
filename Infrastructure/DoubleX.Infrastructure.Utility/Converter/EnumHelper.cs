
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 获取对象枚举项
        /// </summary>
        /// <typeparam name="TEntity">枚举</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>TEntity 返回枚举项</returns>
        public static TEntity Get<TEntity>(object obj)
        {
            if (obj != null)
            {
                return (TEntity)Enum.Parse(typeof(TEntity), obj.ToString());
            }
            return default(TEntity);
        }

        /// <summary>
        /// 获取字符串枚举项
        /// </summary>
        /// <typeparam name="TEntity">枚举</typeparam>
        /// <param name="obj">字符串</param>
        /// <returns>TEntity 返回枚举项</returns>
        public static TEntity Get<TEntity>(string value)
        {

            if (string.IsNullOrWhiteSpace(value))
            {
                return (TEntity)Enum.Parse(typeof(TEntity), value.Trim());
            }
            return default(TEntity);

        }

        /// <summary>
        /// 根据枚举项获取值
        /// </summary>
        /// <typeparam name="TEntity">枚举</typeparam>
        /// <param name="entity">枚举</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>int 返回值</returns>
        public static int GetValue<TEntity>(TEntity entity, int defaultValue = 0) where TEntity : struct
        {
            var value = defaultValue;

            try
            {
                value = (int)Enum.Parse(typeof(TEntity), entity.ToString());
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <typeparam name="TEnum">枚举</typeparam>
        /// <param name="obj">枚举</param>
        /// <returns>string 返回名称</returns>
        public static string GetName<TEnum>(TEnum obj)
        {
            return obj.ToString();
        }

        /// <summary>
        /// 获取枚举及值获取名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">值</param>
        /// <returns>string 返回名称</returns>
        public static string GetName(Type enumType, int value)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }

            string name = "";
            try
            {
                name = Enum.GetName(enumType, value);
            }
            catch (Exception)
            {

            }
            return name;
        }

        /// <summary>
        /// 获取枚举字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>Dictionary 集合</returns>
        public static Dictionary<int, string> GetToDictionary(Type enumType)
        {

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }

            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            foreach (Enum item in Enum.GetValues(enumType))
            {
                Int32 key = Convert.ToInt32(item);
                string value = Enum.GetName(enumType, item);
                enumDic.Add(key, value);
            }
            return enumDic;
        }
    }
}
