using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Framework.Warmup.Hosting
{
    /// <summary>
    /// 服务默认实现
    /// </summary>
    public class DoubleXDefaultHosting : IDoubleXHosting
    {
        //写法：直接实例后不能调用实现接口方法
        //IDoubleXHost.xxxxx

        /// <summary>
        /// 语言列表
        /// </summary>
        public List<CultureInfo> Languages
        {
            get
            {
                if (VerifyHelper.IsEmpty(_languages))
                {
                    _languages = new List<CultureInfo>();
                }
                if (_languages.Count == 0)
                {
                    var str = SettingConfig.GetValue(KeyModel.Config.Setting.KeySystemLanguage);
                    if (!VerifyHelper.IsEmpty(str))
                    {
                        str.Split(',').ToList().ForEach(x =>
                        {
                            _languages.Add(new CultureInfo(x));
                        });
                    }
                }
                if (_languages.Count == 0)
                {
                    _languages.Add(new CultureInfo("en-us"));
                }
                return _languages;
            }
            set { _languages = value; }
        }
        protected List<CultureInfo> _languages;

        /// <summary>
        /// 应用初始（资源的操作）
        /// </summary>
        public virtual void OnInit(HttpApplication applaction) { }

        /// <summary>
        /// 应用启动(框架的绑定配置)
        /// </summary>
        public virtual void OnStart(HttpApplication applaction) { }

        /// <summary>
        /// 应用请求开始
        /// </summary>
        public virtual void OnBegin(HttpApplication applaction) { }

        /// <summary>
        /// 应用请求结束
        /// </summary>
        public virtual void OnEnd(HttpApplication applaction) { }
    }
}
