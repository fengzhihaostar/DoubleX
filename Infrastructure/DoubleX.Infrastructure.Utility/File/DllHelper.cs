using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 动态链接库(dll)文件辅助类
    /// </summary>
    public class DllHelper
    {
        /// <summary>
        /// 版本信息
        /// </summary>
        public static string GetVersion(string assemblyName)
        {
            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    return assembly.GetName().Version.ToString();
                }
            }
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        ///// <summary>
        ///// 动态读取DLL,执行其中的方法
        ///// </summary>
        //public void LoadAssembly()
        //{
        //    //DLL所在的绝对路径 
        //    Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "Entity.dll");
        //    //注意写法：程序集.类名  
        //    Type type = assembly.GetType("Entity.GetData");
        //    //获取类中的公共方法GetResule                                              
        //    MethodInfo methed = type.GetMethod("GetResule");
        //    //创建对象的实例
        //    object instance = System.Activator.CreateInstance(type);
        //    //执行方法  new object[]为方法中的参数
        //    object result = methed.Invoke(instance, new object[] { });
        //}
        ///// <summary>
        ///// //获取程序集信息
        ///// </summary>
        //public void GetAssemblyInfo()
        //{
        //    Type type = typeof(Program);
        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    Console.WriteLine("命名空间:{0}", type.Namespace);
        //    Console.WriteLine("程序集:{0}", type.Assembly);
        //    Console.WriteLine("类的名字{0}", type.Name);
        //    Console.WriteLine("类的全部名字{0}", type.FullName);
        //    Console.WriteLine("基类:{0}", type.BaseType);
        //    Console.WriteLine("----------------------------");
        //    Console.WriteLine("程序集的名称:{0}", assembly.GetName());
        //    Console.WriteLine("程序集的全名:{0}", assembly.FullName);
        //    Console.WriteLine("程序集的版本:{0}", assembly.GetName().Version);
        //    Console.WriteLine("程序集的位置:{0}", assembly.Location);
        //    Console.WriteLine("程序集所在目录:{0}", AppDomain.CurrentDomain.BaseDirectory);
        //}
    }
}
