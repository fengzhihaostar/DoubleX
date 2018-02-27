using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Caching;

namespace DoubleX.Framework.Warmup
{

    /// <summary>
    /// 应用程序(Warmup)辅助类
    /// </summary>
    public class WarmupHelper
    {
        /// <summary>
        /// 多语言资源文件初始
        /// </summary>
        /// <param name="languages"></param>
        public static void LanguageResourceInit(List<CultureInfo> languages)
        {
            Assembly asm = Assembly.Load("App_GlobalResources");
            foreach (var item in StringHelper.ToArray(SettingConfig.GetValue(KeyModel.Config.Setting.KeyResourceScript), new string[] { "," }))
            {
                #region 脚本资源文件处理

                var itemArr = StringHelper.ToArray(item, new string[] { "|" });
                if (!(itemArr != null && itemArr.Length == 2))
                {
                    continue;
                }

                Type resourceType = asm.GetType(itemArr[0]);
                ResourceManager resourceMgr = new ResourceManager(itemArr[0], asm);

                if (VerifyHelper.IsEmpty(resourceType) || VerifyHelper.IsEmpty(resourceMgr))
                {
                    continue;
                }

                foreach (var culture in languages)
                {
                    string context = ScriptsHelper.GetJSONStringByList(ResourceHelper.GetList(resourceType, resourceMgr, culture), itemArr[1]);
                    if (!VerifyHelper.IsEmpty(context))
                    {
                        FileHelper.WriterFile(string.Format("{0}/{1}.{2}.js", HttpContext.Current.Server.MapPath(SettingConfig.GetValue(KeyModel.Config.Setting.KeyCulturePath, KeyModel.Config.Setting.GroupWebsite)), itemArr[1], culture), context);
                    }
                }

                #endregion
            }
            foreach (var name in StringHelper.ToArray(SettingConfig.GetValue(KeyModel.Config.Setting.KeyResourceCache), new string[] { "," }))
            {
                #region 缓存资源文件处理

                Type resourceType = asm.GetType(name);
                ResourceManager resourceMgr = new ResourceManager(name, asm);

                if (VerifyHelper.IsEmpty(resourceType) || VerifyHelper.IsEmpty(resourceMgr))
                {
                    continue;
                }

                foreach (var culture in languages)
                {
                    var list = ResourceHelper.GetList(resourceType, resourceMgr, culture);
                    if (!VerifyHelper.IsEmpty(list))
                    {
                        CachingHelper.Set(string.Format("_RESOURCE_{0}_{1}", resourceType.FullName, culture), list);
                    }
                }

                #endregion
            }
        }
    }
}
