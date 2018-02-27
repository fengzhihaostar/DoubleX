using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Module
{
    public interface IModule
    {
        /// <summary>
        /// 模块描述
        /// </summary>  
        ModuleDescriptor Descriptor { get; set; }

        /// <summary>
        /// 安装模块 
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载模块
        /// </summary>
        void Uninstall();
    }
}
