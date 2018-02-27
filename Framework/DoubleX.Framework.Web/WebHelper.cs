using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DoubleX.Framework;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Entity;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Module.Organize;
using DoubleX.Module.Member;

namespace DoubleX.Framework.Web
{
    /// <summary>
    /// Web应用程序核心辅助类
    /// </summary>
    public class WebHelper : DoubleXHelper
    {
        #region 地址连接

        /// <summary>
        /// 获取站点Url地址
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetWebUrl(string path = "/")
        {
            var webPath = SettingConfig.GetValue(KeyModel.Config.Setting.KeyWebPath, KeyModel.Config.Setting.GroupWebsite);
            return StringHelper.FormatDefault(string.Format("{0}{1}", !VerifyHelper.IsEmpty(webPath) && webPath != "/" ? webPath : "", path));
        }

        /// <summary>
        /// 获取带版本号的Url地址
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public static string GetVersionUrl(string url)
        {
            if (VerifyHelper.IsEmpty(url))
            {
                return string.Format("/?v={0}", VersionNo);
            }
            return string.Format("{0}?{1}={2}", url, url.IndexOf("?") == -1 ? "?" : "&", VersionNo);
        }

        /// <summary>
        /// 获取静态资源地址
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetStaticUrl(string path)
        {
            return StringHelper.FormatDefault(
                string.Format("{0}{1}?v={2}",
                SettingConfig.GetValue(KeyModel.Config.Setting.KeyStaticUrl, KeyModel.Config.Setting.GroupWebsite),
                path,
                VersionNo));
        }



        /// <summary>
        /// JQUERYCDN地址
        /// </summary>
        public static string JQueryCDN
        {
            get
            {
                return StringHelper.FormatDefault(SettingConfig.GetValue(KeyModel.Config.Setting.KeyJQueryCDN, KeyModel.Config.Setting.GroupWebsite));
            }
        }

        /// <summary>
        /// BootstrapCDN地址
        /// </summary>
        public static string BootstrapCDN
        {
            get
            {
                return StringHelper.FormatDefault(SettingConfig.GetValue(KeyModel.Config.Setting.KeyBootstrapCDN, KeyModel.Config.Setting.GroupWebsite));
            }
        }

        /// <summary>
        /// 获取通用CDN资源地址
        /// </summary>
        public static string GetCDNUrl(string path = null)
        {
            return StringHelper.FormatDefault(string.Format("{0}{1}?v={2}", SettingConfig.GetValue(KeyModel.Config.Setting.KeyCDNUrl, KeyModel.Config.Setting.GroupWebsite), path, VersionNo));
        }


        /// <summary>
        /// 跳异常错误页
        /// </summary>
        public static string GetErrorUrl(EnumResultCode code, string message = null, string url = null)
        {
            return string.Format("/error?code={0}&msg={1}&url={2}", EnumHelper.GetValue(code), message, url);
        }

        /// <summary>
        /// 跳异常错误页
        /// </summary>
        public static string GetErrorUrl(ResultModel result)
        {
            return GetErrorUrl(EnumHelper.Get<EnumResultCode>(result.Code),  HttpUtility.UrlEncode(result.Message), "");
        }


        #endregion

        #region 客户端标识

        /// <summary>
        /// 设置访问客户端标识
        /// </summary>
        public static void SetVisitId()
        {
            if (!VerifyHelper.IsSupperHttpCookie())
            {
                return;
            }
            Guid visitId = GuidHelper.Get(CookieHelper.Get(KeyModel.Cookie.VisitTag));
            if (VerifyHelper.IsEmpty(visitId))
            {
                visitId = GenerateVisitId();
                CookieHelper.Set(KeyModel.Cookie.VisitTag, StringHelper.Get(visitId));
            }
        }

        /// <summary>
        /// 获取访问客户端标识
        /// </summary>
        public static Guid GetVisitId()
        {
            //访问标识读取顺序： cookie->querystring(_VisitId)->post(_VisitId);
            Guid visitId = GuidHelper.Get(CookieHelper.Get(KeyModel.Cookie.VisitTag));
            if (VerifyHelper.IsEmpty(visitId))
            {
                visitId = GuidHelper.Get(UrlsHelper.GetQueryValue("_VisitId"));
            }
            //post暂不考虑
            return visitId;
        }

        #endregion

        #region 区域/文化

        /// <summary>
        /// 设置区域/文化标识
        /// </summary>
        /// <param name="culture"></param>
        public static void SetCulture(string culture = null)
        {
            if (!VerifyHelper.IsSupperHttpCookie())
            {
                return;
            }
            if (VerifyHelper.IsEmpty(culture))
            {
                culture = GetCulture();
            }
            CookieHelper.Set(KeyModel.Cookie.Culture, culture);
        }

        /// <summary>
        /// 获取区域/文化标识
        /// </summary>
        /// <param name="culture"></param>
        public static string GetCulture()
        {
            var culture = CultureHelper.GetCulture().Name;
            if (VerifyHelper.IsSupperHttpCookie() && !VerifyHelper.IsEmpty(CookieHelper.Get(KeyModel.Cookie.Culture)))
            {
                culture = CookieHelper.Get(KeyModel.Cookie.Culture);
            }
            return culture;

        }

        #endregion

        #region 请求上下文

        /// <summary>
        /// 获取请求上下文
        /// </summary> 
        /// <returns></returns>
        public static ContextModel GetContext()
        {
            ContextModel model = new ContextModel();

            if (!VerifyHelper.IsSupperHttpCookie())
            {
                return model;
            }

            model = HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] as ContextModel;
            if (VerifyHelper.IsEmpty(model))
            {
                //model = new ContextModel();
                throw new DefaultException("Context is null");
            }
            return model;
        }

        /// <summary>
        /// 设置请求上下文(HttpContext.Current.Items中上下文为空)
        /// </summary>
        public static void SetContext(ContextModel model = null)
        {
            //是否支持上下文及Cookie
            if (!VerifyHelper.IsSupperHttpCookie())
            {
                return;
            }

            //设置传入的上下文对象;
            if (!VerifyHelper.IsEmpty(model))
            {
                HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] = model;
                return;
            }

            model = new ContextModel();

            //客户端标识
            model.VisitId = GetVisitId();

            //设置请求区域/文化信息
            model.Culture = GetCulture();

            //存入上下文
            HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] = model;


            //设置请求租户
            SetTenant();

            //设置请求职员
            SetEmployee();

            //设置请求用户
            SetMember();

        }

        /// <summary>
        /// 设置租户信息
        /// </summary>
        /// <param name="model"></param>
        public static void SetTenant(TenantModel model = null)
        {
            ContextModel context = HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] as ContextModel;
            if (VerifyHelper.IsEmpty(context))
            {
                throw new DefaultException("Context is null");
            }

            if (VerifyHelper.IsNull(model))
            {
                model = new TenantModel();
                if (StringHelper.FormatDefault(HttpContext.Current.Request.Url).Contains(GetManagePath()))
                {
                    model.TenantType = EnumTenantType.管理系统;
                }
                else
                {
                    model.TenantType = EnumTenantType.租户系统;
                }
            }

            var key = string.Format(KeyModel.Session.FormatTenant, context.VisitId);
            RedisHelper.Set<TenantModel>(key, model);
            context.Tenant = model;
            HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] = context;
        }

        /// <summary>
        /// 设置职员信息
        /// </summary>
        /// <param name="model"></param>
        public static void SetEmployee(EmployeeEntity model)
        {
            if (VerifyHelper.IsNull(model))
                return;

            EmployeeModel empModel = new EmployeeModel();
            empModel.EmployeeId = StringHelper.Get(model.Id);
            empModel.EmployeeType = EnumEmployeeType.租户管理;
            empModel.Account = model.Account;
            empModel.Password = model.Password;
            empModel.LoginCount = model.LoginCount;
            empModel.RoleName = "管理员";
            SetEmployee(empModel);
        }

        /// <summary>
        /// 设置职员信息
        /// </summary>
        /// <param name="model"></param>
        public static void SetEmployee(EmployeeModel model = null, bool isClear = false)
        {
            ContextModel context = HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] as ContextModel;
            if (VerifyHelper.IsEmpty(context))
            {
                throw new DefaultException("Context is null");
            }

            var key = string.Format(KeyModel.Session.FormatEmployee, context.VisitId);
            if (VerifyHelper.IsNull(model) && !isClear)
            {
                model = RedisHelper.Get<EmployeeModel>(key);
            }
            RedisHelper.Set<EmployeeModel>(key, model);
            context.Employee = model;
            HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] = context;
        }


        /// <summary>
        /// 设置职员信息
        /// </summary>
        /// <param name="model"></param>
        public static void SetMember(MemberEntity model)
        {
            if (VerifyHelper.IsNull(model))
                return;

            MemberModel meModel = new MemberModel();
            meModel.MemberId = StringHelper.Get(model.Id);
            meModel.MemeberType = EnumMemberType.登录会员;
            meModel.Account = model.Account;
            meModel.NameTag = model.NameTag;
            meModel.Password = model.Password;
            meModel.LastLoginIP = model.LastLoginIP;
            meModel.LastLoginDt = model.LastLoginDt;
            SetMember(meModel);
        }

        /// <summary>
        /// 设置会员信息
        /// </summary>
        /// <param name="member"></param>
        public static void SetMember(MemberModel model = null, bool isClear = false)
        {
            ContextModel context = HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] as ContextModel;
            if (VerifyHelper.IsEmpty(context))
            {
                throw new DefaultException("Context is null");
            }

            var key = string.Format(KeyModel.Session.FormatMember, context.VisitId);
            if (VerifyHelper.IsNull(model) && !isClear)
            {
                model = RedisHelper.Get<MemberModel>(key);
            }
            RedisHelper.Set<MemberModel>(key, model);

            context.Member = model;
            HttpContext.Current.Items[KeyModel.Context.OriginalHttpContext] = context;
        }

        #endregion

        #region 管理中心

        /// <summary>
        /// 根据路径获取管理中心地址
        /// </summary>
        /// <param name="pathList">路径</param>
        /// <returns></returns>
        public static string GetManagePageTitleByPath(List<KeyValuePair<string, string>> pathList)
        {
            if (VerifyHelper.IsEmpty(pathList))
                return "";
            StringBuilder build = new StringBuilder();
            foreach (var item in pathList)
            {
                build.AppendFormat("-{0}", item.Value);
            }
            return build.ToString();
        }

        /// <summary>
        /// 获取管理中心路径
        /// </summary>
        /// <returns></returns>
        public static string GetManagePath(bool isRouteFormat = false)
        {
            var path = StringHelper.Get(SettingConfig.GetValue(KeyModel.Config.Setting.KeyManagePath, KeyModel.Config.Setting.GroupWebsite));
            while (path.IndexOf("//") > -1)
            {
                path = path.Replace("//", "/");
            }
            if (isRouteFormat)
            {
                path = path.Replace("/", "|");
            }
            return path;
        }

        /// <summary>
        /// 获取管理中主Url地址
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetManageUrl(string path = null)
        {
            return StringHelper.FormatDefault(string.Format("{0}{1}", GetManagePath(), path));
        }

        #endregion

        #region 会员中心

        /// <summary>
        /// 获取会员中心路径
        /// </summary>
        /// <returns></returns>
        public static string GetMemberPath(bool isRouteFormat = false)
        {
            var path = StringHelper.Get(SettingConfig.GetValue(KeyModel.Config.Setting.KeyMemberPath, KeyModel.Config.Setting.GroupWebsite));
            while (path.IndexOf("//") > -1)
            {
                path = path.Replace("//", "/");
            }
            if (isRouteFormat)
            {
                path = path.Replace("/", "|");
            }
            return path;
        }

        /// <summary>
        /// 获取会员中主Url地址
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetMemberUrl(string path = null)
        {
            return StringHelper.FormatDefault(string.Format("{0}{1}", GetMemberPath(), path));
        }

        /// <summary>
        /// 获取会员Id(在需要调用会员信息的时)
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static Guid GetMemberId(RequestModel request, string requestKey = "id")
        {
            var memberId = Guid.Empty;
            var context = WebHelper.GetContext();
            if (!VerifyHelper.IsNull(context))
            {
                if (context.IsMemberLogin)
                {
                    memberId = GuidHelper.Get(context.Member.MemberId);
                }
                if (context.IsEmployeeLogin && !VerifyHelper.IsNull(request) && !VerifyHelper.IsNull(request.Obj))
                {
                    var requestMemberId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, requestKey));
                    if (!VerifyHelper.IsEmpty(requestMemberId))
                    {
                        memberId = requestMemberId;
                    }
                }
            }
            return memberId;
        }

        /// <summary>
        /// 获取信息所有都条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select"></param>
        /// <param name="request"></param>
        /// <param name="requestKey"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> GetOwnerPredicate<TEntity>(Expression<Func<TEntity, dynamic>> select,
            RequestModel request, string requestKey = "id")
        {
            var ownerPredicate = ExpressionPredicateExtend.False<TEntity>();
            var context = WebHelper.GetContext();
            if (VerifyHelper.IsNull(context))
            {
                return ownerPredicate;
            }

            List<Guid> ids = new List<Guid>();
            if (context.IsMemberLogin)
            {
                ids = new List<Guid>() { GuidHelper.Get(context.Member.MemberId) };
            }
            else if (context.IsEmployeeLogin)
            {
                var requestMemberId = GuidHelper.Get(JsonHelper.GetValue(request.Obj, requestKey));
                if (!VerifyHelper.IsEmpty(requestMemberId))
                {
                    ids = new List<Guid>() { GuidHelper.Get(context.Member.MemberId) };
                }
            }
            if (ids.Count > 0)
            {
                var propertyInfo = ReflectionHelper.GetPropertyInfo<TEntity>(select);

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var left = Expression.Constant(ids);
                var predicate = left.Call("Contains", parameter.Property(propertyInfo.Name));

                ownerPredicate = predicate.ToLambda<Func<TEntity, bool>>(parameter);
            }

            return ownerPredicate;
        }

        #endregion

        #region Web相关

        ///// <summary>
        ///// 生成安全值
        ///// </summary>
        //public static string GenerateSafetyValue(string str)
        //{
        //    //AppId,AppKey,AppSafetyKey,UserId->MD5->BASE64
        //    return str;
        //}

        ///// <summary>
        ///// 设置客户端员工信息
        ///// </summary>
        ///// <param name="model">员工信息</param>
        //public static void SetAdminClient(EmployeeEntity model)
        //{
        //    CookieHelper.Set(KeyModel.Cookie.KeyAdmin, Base64Helper.Encode(JsonHelper.Serialize(model)));
        //    CookieHelper.Set(KeyModel.Cookie.KeyAdminTag, Base64Helper.Encode(WebHelper.GenerateSafetyValue(model.Account)));
        //}

        ///// <summary>
        ///// 移除客户端员工信息
        ///// </summary>
        //public static void ClearAdminClient() {
        //    CookieHelper.Set(KeyModel.Cookie.KeyAdmin, null, DateTime.Now.AddDays(-1));
        //    CookieHelper.Set(KeyModel.Cookie.KeyAdminTag, null, DateTime.Now.AddDays(-1));
        //}

        #endregion
    }
}
