//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找已本地化的字符串等等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 Visual Studio 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Common {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Common() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Common", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   为使用此强类型资源类的所有资源查找重写当前线程的 CurrentUICulture 
        /// 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似  登录 的本地化字符串。
        /// </summary>
        internal static string DengLu {
            get {
                return ResourceManager.GetString("DengLu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 快速查找 的本地化字符串。
        /// </summary>
        internal static string KuaiSuChaZhao {
            get {
                return ResourceManager.GetString("KuaiSuChaZhao", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 首页 的本地化字符串。
        /// </summary>
        internal static string ShouYe {
            get {
                return ResourceManager.GetString("ShouYe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 添加 的本地化字符串。
        /// </summary>
        internal static string TianJia {
            get {
                return ResourceManager.GetString("TianJia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 退出 的本地化字符串。
        /// </summary>
        internal static string TuiChu {
            get {
                return ResourceManager.GetString("TuiChu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 修改 的本地化字符串。
        /// </summary>
        internal static string XiuGai {
            get {
                return ResourceManager.GetString("XiuGai", resourceCulture);
            }
        }
    }
}
