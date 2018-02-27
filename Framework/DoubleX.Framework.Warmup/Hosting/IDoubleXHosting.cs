using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DoubleX.Framework.Warmup.Hosting
{
    /// <summary>
    /// 应用主服务接口
    /// </summary>
    public interface IDoubleXHosting
    {
        /// <summary>
        /// 应用初始（资源的操作）
        /// </summary>
        void OnInit(HttpApplication applaction);

        /// <summary>
        /// 应用启动(框架的绑定配置)
        /// </summary>
        void OnStart(HttpApplication applaction);

        /// <summary>
        /// 应用请求开始
        /// </summary>
        void OnBegin(HttpApplication applaction);

        /// <summary>
        /// 应用请求结束
        /// </summary>
        void OnEnd(HttpApplication applaction);

    }
}
