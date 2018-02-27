using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 获取对象属性值 
        /// </summary>
        public static string GetObjectPropertyValue<TEntity>(TEntity model, string propertyName)
        {
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null) return "";
            object o = property.GetValue(model, null);
            if (o == null) return "";
            return o.ToString();
        }

        /// <summary>
        /// 判断类型subType对象，是 否继承于rootType
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="rootType">typeof(IRequestModel《》)</param>
        /// <returns></returns>
        public static bool IsAssignableFrom(Type subType, Type rootType)
        {
            if (subType == null || rootType == null)
                return false;

            Type[] types = null;
            if (subType.BaseType.IsGenericType && !subType.BaseType.IsGenericTypeDefinition)
            {
                types = subType.BaseType.GetGenericArguments();
            }
            else
            {
                types = Type.EmptyTypes;
            }



            //泛型及参数
            if (types!=null && types.Length > 0)
            {
                Type typeEntity = types[0];
                Type genericType = rootType.MakeGenericType(new[] { typeEntity });
                return genericType.IsAssignableFrom(subType);
            }

            Type genericType2 = rootType.MakeGenericType(new[] { subType.GetInterfaces()[0] });

            return Array.IndexOf(subType.GetInterfaces(), rootType) > -1 || subType.IsSubclassOf(rootType);
            //在这里详细说明一下，第一行代码是获取泛型参数的实例类型。用于第二行构造泛型类型。构造完成之后，我们就可以使用Type.IsAssignableFrom 方法判断了。代码如下：
            //Type _typeEntity = modelType.BaseType.GenericTypeArguments[0];
            //Type genericSourceType = typeof(IRequestModel<>).MakeGenericType(new[] { _typeEntity });
            //var c = typeof(IRequestModel<>).IsAssignableFrom(modelType); // true
            //if (!genericSourceType.IsAssignableFrom(modelType))
            //{
            //    //throw new ArgumentException("不是有效的数据传输对象！", "typeDTO");
            //}

            //IsSubclassOf 方法不能用于确定某个接口是否派生自另一个接口或某个类是否实现了某个接口。
            //使用 GetInterface 方法
            //if (Array.IndexOf(t.GetInterfaces(), superClassType) > -1 || t.IsSubclassOf(superClassType)) 
        }

        /// <summary>
        /// 获取指定属性信息（非String类型存在装箱与拆箱）
        /// eg:var p = GetPropertyInfo<T>(t => t.Age);//获取指定属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="select"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, dynamic>> select)
        {
            var body = select.Body;
            if (body.NodeType == ExpressionType.Convert)
            {
                var o = (body as UnaryExpression).Operand;
                return (o as MemberExpression).Member as PropertyInfo;
            }
            else if (body.NodeType == ExpressionType.MemberAccess)
            {
                return (body as MemberExpression).Member as PropertyInfo;
            }
            return null;
        }

        /// <summary>
        /// 获取指定属性信息（需要明确指定属性类型，但不存在装箱与拆箱）
        /// eg: var ps1 = GetPropertyInfos<T>(t => t);//获取类型所有属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="select"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T, TR>(Expression<Func<T, TR>> select)
        {
            var body = select.Body;
            if (body.NodeType == ExpressionType.Convert)
            {
                var o = (body as UnaryExpression).Operand;
                return (o as MemberExpression).Member as PropertyInfo;
            }
            else if (body.NodeType == ExpressionType.MemberAccess)
            {
                return (body as MemberExpression).Member as PropertyInfo;
            }
            return null;
        }

        /// <summary>
        /// 获取类型的所有属性信息
        /// eg:var ps2 = GetPropertyInfos<People>(t => new { t.Name, t.Age });//获取部份属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="select"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfos<T>(Expression<Func<T, dynamic>> select)
        {
            var body = select.Body;
            if (body.NodeType == ExpressionType.Parameter)
            {
                return (body as ParameterExpression).Type.GetProperties();
            }
            else if (body.NodeType == ExpressionType.New)
            {
                return (body as NewExpression).Members.Select(m => m as PropertyInfo).ToArray();
            }
            return null;
        }

    }
}
