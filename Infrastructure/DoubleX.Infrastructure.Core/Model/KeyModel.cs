using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Core.Model
{
    /// <summary>
    /// 键对象
    /// </summary>
    public static class KeyModel
    {
        /// <summary>
        /// 请求内容类型
        /// </summary>
        public static class ContentType
        {
            /// <summary>
            /// 表单提交数据
            /// </summary>
            public const string Form = "application/x-www-form-urlencoded";

            /// <summary>
            /// JSON格式
            /// </summary>
            public const string JSON = "application/json";

            /// <summary>
            /// XML格式
            /// </summary>
            public const string Xml = "text/xml";
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        public static class Cache
        {
            /// <summary>
            /// 容器KEY
            /// </summary>
            public const string Container = "_APP_CACHE_DOUBLEX_CONTAINER";

            /// <summary>
            /// 项目设置对象缓存Key
            /// </summary>
            public const string Setting = "_APP_CACHE_DOUBLEX_SETTING";

            /// <summary>
            /// 数据中心设置对象缓存Key
            /// </summary>
            public const string DataCenter = "_APP_CACHE_DOUBLEX_DATACENTER";

            /// <summary>
            /// 临时用户信息缓存Key
            /// </summary>
            public const string DataCenterTempUser = "_APP_CACHE_DOUBLEX_DATACENTERTEMPUSER";

            /// <summary>
            /// 模版内容配置对象缓存Key
            /// </summary>
            public const string Template = "_APP_CACHE_DOUBLEX_TEMPLATE";
        }

        /// <summary>
        /// 配置文件键
        /// </summary>
        public static class Config
        {
            /// <summary>
            /// 项目设置对象配置文件Key
            /// </summary>
            public static class Setting
            {
                /// <summary>
                /// 节点名称
                /// </summary>
                public const string SectionName = "DoubleXSetting";

                #region 系统配置组

                /// <summary>
                /// 系统配置节点组
                /// </summary>
                public const string GroupSystem = "System";

                /// <summary>
                /// 应用程序名称
                /// </summary>
                public const string KeyApplaction = "applaction";

                /// <summary>
                /// 调式模式配置
                /// </summary>
                public const string KeyDebug = "debug";

                /// <summary>
                /// 语言配置Key
                /// </summary>
                public const string KeySystemLanguage = "language";

                #endregion

                #region 数据连接字符串

                /// <summary>
                /// 数据库连接配置组 
                /// </summary>
                public const string GroupDatabase = "Database";

                /// <summary>
                /// Mongo默认连接串
                /// </summary>
                public const string KeyMongoDefault = "MongoDefault";

                /// <summary>
                /// Redis默认连接串
                /// </summary>
                public const string KeyRedisDefault = "RedisDefault";

                /// <summary>
                /// SQLite 默认连接串
                /// </summary>
                public const string KeySQLiteDefault = "SQLiteDefault";

                /// <summary>
                /// EntityFramework默认连接串
                /// </summary>
                public const string KeyEntityFrameworkDefault = "EntityFrameworkDefault";

                /// <summary>
                /// SqlServer默认连接串
                /// </summary>
                public const string KeySqlServerDefault = "SqlServerDefault";

                /// <summary>
                /// MySql默认连接串
                /// </summary>
                public const string KeyMySqlDefault = "MySqlDefault";

                #endregion

                #region  站点配置

                /// <summary>
                /// 站点配置节点组
                /// </summary>
                public const string GroupWebsite = "Website";

                /// <summary>
                /// 站点路径Key
                /// </summary>
                public const string KeyWebPath = "webPath";


                /// <summary>
                /// 静态资源Key
                /// </summary>
                public const string KeyStaticUrl = "staticUrl";

                /// <summary>
                /// 通用CDN地址
                /// </summary>
                public const string KeyCDNUrl = "cdnUrl";

                /// <summary>
                /// JQueryCDN地址
                /// </summary>
                public const string KeyJQueryCDN = "jqueryCDN";

                /// <summary>
                /// BootstrapCDN地址
                /// </summary>
                public const string KeyBootstrapCDN = "bootstrapCDN";

                /// <summary>
                /// 脚本使用资源Json文件目录
                /// </summary>
                public const string KeyCulturePath = "culturePath";

                /// <summary>
                /// 管理中心路径
                /// </summary>
                public const string KeyManagePath = "managePath";

                /// <summary>
                /// 会员中心路径
                /// </summary>
                public const string KeyMemberPath = "memberPath";

                #endregion

                #region 资源文件配置组

                /// <summary>
                /// 资源文件组
                /// </summary>
                public const string GroupResource = "Resources";

                /// <summary>
                /// 需生成脚本的资源文件Key
                /// </summary>
                public const string KeyResourceScript = "scriptResource";

                /// <summary>
                /// 需缓存的资源文件Key
                /// </summary>
                public const string KeyResourceCache = "cacheResource";

                #endregion

                #region 通用的设置信息

                /// <summary>
                /// 设置信息组
                /// </summary>
                public const string GroupOption = "Options";

                /// <summary>
                /// 默认描述长度
                /// </summary>
                public const string KeyDescriptLength = "descriptLength";

                /// <summary>
                /// 上传文件路径
                /// </summary>
                public const string KeyUploadPath = "uploadPath";

                #endregion
            }

            /// <summary>
            /// 数据中心配置Key
            /// </summary>
            public static class DataCenter
            {
                /// <summary>
                /// 节点名称
                /// </summary>
                public const string SectionName = "DataCetner";

                #region 基本配置

                /// <summary>
                /// 应用程序Id
                /// </summary>
                public const string KeyAppId = "appId";

                /// <summary>
                /// 应用程序客户端Id
                /// </summary>
                public const string KeyClientId = "clientId";

                /// <summary>
                /// 数据中心版本
                /// </summary>
                public const string KeyVersion = "version";

                /// <summary>
                /// 数据中心日志(开启后每次请求都记日志)
                /// </summary>
                public const string KeyWriteLog = "writeLog";

                /// <summary>
                /// Api访问临时用户
                /// </summary>
                public const string KeyApiTempUser = "apiTempUser";

                /// <summary>
                /// 数据中心地址
                /// </summary>
                public const string KeyApiAddress = "apiAddress";

                #endregion

                #region 接口地址

                #region 基本数据

                /// <summary>
                /// 语言列表
                /// </summary>
                public const string KeyBasLanguage = "basLanguage";

                /// <summary>
                /// 领域列表
                /// </summary>
                public const string KeyBasDomain = "basDomain";

                #endregion

                #region 公共功能

                /// <summary>
                /// 短信发送
                /// </summary>
                public const string KeySmsSend = "smsSend";

                /// <summary>
                /// 邮件发送
                /// </summary>
                public const string KeyEmailSend = "emailSend";

                #endregion

                #region 账号操作

                /// <summary>
                /// 帐号登录
                /// </summary>
                public const string KeyAccountLogin = "accountLogin";

                #endregion

                #endregion
            }

            /// <summary>
            /// 模块内容配置Key
            /// </summary>
            public static class Template
            {

                /// <summary>
                /// 节点名称
                /// </summary>
                public const string SectionName = "Template";

                #region 注册模版

                public const string KeyRegistMobile = "registMobile";
                public const string KeyRegistEmail = "registEmail";

                #endregion

                #region 绑定模版

                public const string KeyBindMobile = "bindMobile";
                public const string KeyBindEmail = "bindEmail";

                #endregion

                #region 找回模版

                public const string KeyForgetPwdMobile = "forgetPwdMobile";
                public const string KeyForgetPwdEmail = "forgetPwdEmail";

                #endregion
            }
        }

        /// <summary>
        /// 上下文键
        /// </summary>
        public static class Context
        {
            /// <summary>
            /// 当前Http请求上下文
            /// </summary>
            public const string OriginalHttpContext = "originalHttpContext";
        }

        /// <summary>
        /// Cookies键
        /// </summary>
        public static class Cookie
        {
            /// <summary>
            /// 客户端标识
            /// </summary>
            public const string VisitTag = "_id";

            /// <summary>
            /// 区域/文化信息
            /// </summary>
            public const string Culture = "_culture";

            ///// <summary>
            ///// 管理员信息
            ///// </summary>
            //public const string KeyAdmin = "_admin";

            ///// <summary>
            ///// 管理员标识
            ///// </summary>
            //public const string KeyAdminTag = "_admin_tag";
        }

        /// <summary>
        /// 会话键
        /// </summary>
        public static class Session
        {
            /// <summary>
            /// 租户信息
            /// </summary>
            public const string FormatTenant = "_{0}_tenant";

            /// <summary>
            /// 职员信息
            /// </summary>
            public const string FormatEmployee = "_{0}_employee";

            /// <summary>
            /// 会员信息
            /// </summary>
            public const string FormatMember = "_{0}_member";

            /// <summary>
            /// 验证码
            /// </summary>
            public const string FormatVerifyCode = "_verifycode_{0}_";
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        public static class Log
        {
            /// <summary>
            /// 异常日志
            /// </summary>
            public const string ExceptionName = "ExceptionLog";

            /// <summary>
            /// 业务日志
            /// </summary>
            public const string ServiceName = "ServiceLog";

            /// <summary>
            /// 支付通知日志
            /// </summary>
            public const string PaySubmit = "PaySubmitLog";

            /// <summary>
            /// 支付通知日志
            /// </summary>
            public const string PayNotification = "PayNotificationLog";
        }
    }
}
