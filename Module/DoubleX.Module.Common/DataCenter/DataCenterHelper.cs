using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Config;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Caching;

namespace DoubleX.Module.Common
{
    /// <summary>
    /// 数据中心操作辅助类
    /// </summary>
    public class DataCenterHelper
    {
        /// <summary>
        /// 获取结果(GET方式)
        /// </summary>
        /// <returns></returns>
        public static ResultModel Get(string url, string resultKey = null, string errorKey = null)
        {
            return Request(url, "", resultKey: resultKey, errorKey: errorKey);
        }

        /// <summary>
        /// 获取结果(Post方式)
        /// </summary>
        /// <returns></returns>
        public static ResultModel Post(string url, dynamic post, string resultKey = null, string errorKey = null)
        {
            return Post(url, JsonHelper.Serialize(post), resultKey: resultKey, errorKey: errorKey);
        }

        /// <summary>
        /// 获取结果(Post方式)
        /// </summary>
        /// <returns></returns>
        public static ResultModel Post(string url, string post, string resultKey = null, string errorKey = null)
        {
            JObject postObj = null;
            if (!VerifyHelper.IsEmpty(post))
            {
                postObj = JsonHelper.Deserialize<JObject>(post);
            }
            if (VerifyHelper.IsNull(postObj))
            {
                postObj = new JObject();
            }

            //公共post参数
            postObj["AppId"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyAppId);
            postObj["ClientId"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyClientId);
            postObj["Version"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyVersion);
            postObj["ip"] = BrowserHelper.GetClientIP();

            var passport = postObj.GetValue("passport", StringComparison.InvariantCultureIgnoreCase);
            if (VerifyHelper.IsNull(passport))
            {
                passport = GetTempUserPassport();
            }
            postObj["passport"] = passport;
            //postObj["culture"] = CommonHelper.GetContext().Culture;
            //postObj["security"] = "1xxxx";
            return Request(url, JsonHelper.Serialize(postObj), resultKey: resultKey, errorKey: errorKey);
        }

        /// <summary>
        /// 请求数据中心
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="post">POST数据</param>
        /// <param name="resultKey">结果Key</param>
        /// <param name="errorKey">错误消息Key</param>
        /// <returns></returns>
        protected static ResultModel Request(string url, string post, string resultKey = "", string errorKey = "")
        {
            //开始日志
            WriteLog();

            //请求并返回
            return SetReturnModel(CoreHelper.GetHttpResult(url, post, VerifyHelper.IsEmpty(post) ? "GET" : "POST"));
        }




        /// <summary>
        /// 获取临时用户登录Passport
        /// </summary>
        /// <returns></returns>
        protected static JObject GetTempUser()
        {
            var tempUser = CachingHelper<HttpRuntimeCaching>.Get<JObject>(KeyModel.Cache.DataCenterTempUser);
            if (VerifyHelper.IsEmpty(tempUser))
            {
                var tempUserAccount = StringHelper.Get(DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyApiTempUser)).Split('|');
                if (VerifyHelper.IsEmpty(tempUserAccount))
                {
                    throw new DefaultException("数据中心TempUser配置错误");
                }

                var data = new JObject();
                data["AppId"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyAppId);
                data["ClientId"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyClientId);
                data["Version"] = DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyVersion);
                data["ip"] = BrowserHelper.GetClientIP();
                data["Account"] = tempUserAccount.Length > 0 ? tempUserAccount[0] : "";
                data["Password"] = tempUserAccount.Length > 1 ? tempUserAccount[1] : "";

                var result = Request(DataCenterConfig.GetApiUrl(KeyModel.Config.DataCenter.KeyAccountLogin), JsonHelper.Serialize(data), resultKey: null, errorKey: null);
                if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功) && !VerifyHelper.IsEmpty(result.Obj))
                {
                    tempUser = result.Obj;
                }
            }
            return VerifyHelper.IsEmpty(tempUser) ? new JObject() : tempUser;
        }

        /// <summary>
        /// 获取临时用户登录Passport
        /// </summary>
        /// <returns></returns>
        protected static string GetTempUserPassport()
        {
            return StringHelper.Get(GetTempUser().GetValue("Passport"));
        }


        /// <summary>
        /// 写请求数据中心日志
        /// </summary>
        protected static void WriteLog()
        {
            bool isWriteLog = BoolHelper.Get(DataCenterConfig.GetValue(KeyModel.Config.DataCenter.KeyWriteLog));

            //if (BoolHelper.Get(SysModelConfigs.ApiLog.GetSysModelValue()))
            //{
            //    Log4NetHelper.Get(EnumHelper.GetName(EnumLogType.ApiLog)).WriteLogAsync(string.Format("--[Request]--\r\n{0}\r\n--[Response]--\r\n{1}\r\n", JsonHelper.Serialize(item), httpResult.Html));
            //}
        }

        /// <summary>
        /// 设置数据中心返回信息
        /// </summary>
        protected static ResultModel SetReturnModel(ResultModel model)
        {
            if (VerifyHelper.IsNull(model))
            {
                throw new DefaultException("数据中心请求结果(ResultModel)为NULL");
            }
            if (model.Code == EnumHelper.GetValue(EnumResultCode.操作成功) && !VerifyHelper.IsEmpty(model.Obj))
            {
                model.Obj = JsonHelper.Deserialize<JObject>(model.Obj);
            }
            return model;
        }
    }
}
