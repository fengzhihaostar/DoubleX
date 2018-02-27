using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// JSON工具类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 获取JObject的Value
        /// </summary>
        /// <param name="obj">JObject对象</param>
        /// <param name="key">值Key</param>
        /// <returns></returns>
        public static string GetValue(JObject obj, string key)
        {
            if (!VerifyHelper.IsNull(obj) && !VerifyHelper.IsEmpty(key))
            {
                return StringHelper.Get(obj.GetValue(key, StringComparison.InvariantCultureIgnoreCase));
            }
            return "";
        }

        ///// <summary>
        ///// 获取JObject的Value
        ///// </summary>
        ///// <typeparam name="TEntity">值类</typeparam>
        ///// <param name="obj">JObject对象</param>
        ///// <param name="key">值Key</param>
        ///// <returns></returns>
        //public static TEntity GetValue<TEntity>(JObject obj, string key) where TEntity : class
        //{
        //    TEntity returnValue = default(TEntity);
        //    if (!VerifyHelper.IsNull(obj) && !VerifyHelper.IsEmpty(key))
        //    {
        //        returnValue = obj.TryGetValue(key, StringComparison.InvariantCultureIgnoreCase) as TEntity;
        //    }
        //    return returnValue;
        //}

        /// <summary>
        /// 将字符串序列化成对象
        /// </summary>
        /// <typeparam name="TEntity">对象类型</typeparam>
        /// <param name="jsonStr">字符串</param>
        /// <param name="isNewObj">为空时是否new 新对象</param>
        /// <returns></returns>
        public static TEntity Deserialize<TEntity>(string jsonStr, bool isNewObj = false) where TEntity : new()
        {
            TEntity returnObj = default(TEntity);
            if (!string.IsNullOrWhiteSpace(jsonStr))
            {
                JsonSerializerSettings jsetting = new JsonSerializerSettings();

                //空值
                jsetting.NullValueHandling = NullValueHandling.Ignore;

                //GUID
                jsetting.Converters.Add(new GuidConverter());

                //时间
                //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                //jsetting.Converters.Add(timeFormat);

                returnObj = JsonConvert.DeserializeObject<TEntity>(jsonStr, jsetting);//Formatting.Indented, 
            }
            if (returnObj == null && isNewObj)
            {
                returnObj = new TEntity();
            }
            return returnObj;
        }

        /// <summary>
        /// 将对象返序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="formatting">时间格式</param>
        /// <param name="settings">返序列化设置</param>
        /// <returns>json 字符串</returns>
        public static string Serialize(object obj, Formatting? formatting = null)
        {
            string jsonStr = "";
            if (obj != null)
            {
                JsonSerializerSettings jsetting = new JsonSerializerSettings();

                //空值
                jsetting.NullValueHandling = NullValueHandling.Ignore;

                //GUID
                jsetting.Converters.Add(new GuidConverter());

                //时间
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                jsetting.Converters.Add(timeFormat);

                try
                {
                    if (formatting == null)
                    {
                        jsonStr = JsonConvert.SerializeObject(obj, jsetting);
                    }
                    else
                    {
                        jsonStr = JsonConvert.SerializeObject(obj, formatting.Value, jsetting);
                    }
                }
                catch (Exception ex) {
                    var cc = ex;
                }
            }
            return jsonStr;
        }

        /// <summary>
        /// 将QueryString字符串转为Json字符串
        /// </summary>
        /// <param name="queryStr">QueryString字符串</param>
        /// <returns>Json字符串</returns>
        public static string FormatQueryString(string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr))
                return "";
            StringBuilder build = new StringBuilder();
            NameValueCollection list = System.Web.HttpUtility.ParseQueryString(queryStr);

            for (int i = 0; i < list.Count; i++)
            {
                build.AppendFormat("\"{0}\":\"{1}\",", list.GetKey(i), list.Get(i));
            }
            if (build.Length > 0)
            {
                build.Remove(build.Length - 1, 1);
                return string.Format("{{{0}}}", build.ToString());
            }
            return "";
        }

        /// <summary>
        /// 合并两个JObject对象(将Json字符串序列成JObject)(只支持第一级差异对比)
        /// </summary>
        /// <param name="obj">源JObject对象</param>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>合并后的JObject</returns>
        public static JObject Build(JObject obj, string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return obj;
            }
            var obj2 = Deserialize<JObject>(jsonStr);
            if (obj2 == null)
                return obj;
            return Build(obj, obj2);
        }

        /// <summary>
        /// 合并两个JObject对象(将Json字符串序列成JObject)(只支持第一级差异对比)
        /// </summary>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <returns>合并后的JObject</returns>
        public static JObject Build(JObject obj1, JObject obj2)
        {
            if (obj1 == null)
                obj1 = new JObject();
            if (obj2 == null)
                return obj1;

            var newObj = new JObject();

            //设置源
            foreach (JProperty item in obj1.Properties())
            {
                newObj.Add(item);
            }

            //设置目标
            foreach (JProperty item in obj1.Properties())
            {
                var sObj = newObj.Properties().FirstOrDefault(x => x.Name == item.Name);
                if (sObj != null)
                {
                    sObj.Value = item.Value;
                }
                else
                {
                    newObj.Add(item.Name, item.Value);
                }
            }
            return newObj;
        }

    }


    /// <summary>
    /// JSON.NET Guid //http://www.cnblogs.com/Leo_wl/p/4805925.html
    /// [JsonConverter(typeof(GuidConverter))]  
    /// public Guid Id { get; set; }  
    /// </summary>
    public class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Guid));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //try
            //{
            //    if (reader == null || (reader != null && reader.Value.ToString() == ""))
            //    {
            //        return Guid.Empty;
            //    }
            //    var value = Guid.Empty;
            //    Guid.TryParse(reader.Value.ToString(), out value);
            //    return value;
            //}
            //catch
            //{
            //    //如果传进来的值造成异常，则赋值一个初值  
            //    return Guid.Empty;
            //}
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return Guid.Empty;
                case JsonToken.String:
                    string str = reader.Value as string;
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        Guid returnValue=Guid.Empty;
                        if (Guid.TryParse(str, out returnValue))
                            return returnValue;
                        return Guid.Empty;
                    }
                default:
                    throw new ArgumentException("JSON GUID invalid token type");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //serializer.Serialize(writer, value);
            if (Guid.Empty.Equals(value))
            {
                writer.WriteValue("");
            }
            else
            {
                writer.WriteValue((Guid)value);
            }
        }
    }
}
