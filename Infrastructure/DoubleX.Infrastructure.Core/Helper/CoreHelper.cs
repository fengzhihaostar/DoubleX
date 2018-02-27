using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Caching;

namespace DoubleX.Infrastructure.Core
{
    /// <summary>
    /// 核心辅助类
    /// </summary>
    public class CoreHelper
    {
        /// <summary>
        /// 获取加密密码
        /// </summary>
        public static string GetPassword(string password)
        {
            if (VerifyHelper.IsEmpty(password))
                return "";
            return MD5Helper.Get(password);
        }

        #region IOC Container操作

        public static void SetContainer(IContainer container)
        {
            CachingHelper<HttpRuntimeCaching>.Set(KeyModel.Cache.Container, container);
        }

        public static IContainer GetContainer()
        {
            var obj = CachingHelper<HttpRuntimeCaching>.Get(KeyModel.Cache.Container);
            if (VerifyHelper.IsNull(obj))
            {
                throw new DefaultException(" The IOC Containe Is null");
            }
            IContainer container = obj as IContainer;
            if (VerifyHelper.IsNull(container))
            {
                throw new DefaultException(" The IOC Containe Is null");
            }
            return container;
        }

        public static TEntity GetResolve<TEntity>()
        {
            return GetContainer().Resolve<TEntity>();
        }

        #endregion

        #region 网络请求操作返回ResultModel

        /// <summary>
        /// Http请求并返回ResultModel消息对象(根据Url && Post &&....)
        /// </summary>
        public static ResultModel GetHttpResult(string url, string post = null, string method = null, Encoding encoding = null, string contentType = null)
        {
            ResultModel result = new ResultModel();
            try
            {
                HttpItem item = new HttpItem();
                if (VerifyHelper.IsEmpty(encoding))
                {
                    encoding = Encoding.UTF8;
                }
                if (VerifyHelper.IsEmpty(method))
                {
                    method = "POST";
                    if (VerifyHelper.IsEmpty(post))
                    {
                        method = "GET";
                    }
                }
                if (VerifyHelper.IsEmpty(contentType))
                {
                    contentType = KeyModel.ContentType.JSON;
                }

                item.Postdata = post;
                item.URL = url;
                item.Encoding = encoding;
                item.ContentType = contentType;
                item.Method = method;

                //不指定编辑可能导致接收不到数据(中文)
                if (item.Method.ToUpper() == "POST")
                {
                    item.PostEncoding = encoding;
                }
                result = GetHttpResult(item);
            }
            catch (Exception ex)
            {
                result.Code = EnumHelper.GetValue<EnumResultCode>(EnumResultCode.接口错误);
            }
            return result;
        }

        /// <summary>
        /// Http请求并返回ResultModel消息对象(根据请求HttpItem)
        /// </summary>
        public static ResultModel GetHttpResult(HttpItem item)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (item == null)
                {
                    result.Code = EnumHelper.GetValue(EnumResultCode.接口错误);
                    return result;
                }

                var httpResult = new HttpHelper().GetHtml(item);
                if (httpResult == null)
                {
                    result.Code = EnumHelper.GetValue(EnumResultCode.接口错误);
                    return result;
                }
                if (httpResult.StatusCode == System.Net.HttpStatusCode.NotFound
                    || httpResult.Html.Contains("无法连接到远程服务器")
                    || httpResult.StatusDescription.Contains("配置参数时出错"))
                {
                    result.Code = EnumHelper.GetValue(EnumResultCode.接口错误);
                    return result;
                }
                //result.StatusDescription = "配置参数时出错："
                if (httpResult.Html == null)
                {
                    result.Code = EnumHelper.GetValue(EnumResultCode.接口错误);
                    return result;
                }

                if (httpResult.Html.Contains("发生错误") || httpResult.Html.Contains("出现错误"))
                {
                    //"{\"Message\":\"出现错误。\",\"ExceptionMessage\":\"xxxxx\"}
                    result.Code = EnumHelper.GetValue(EnumResultCode.未知异常);
                    result.Obj = httpResult.Html;
                    return result;
                }

                //默认为成功
                result.Code = EnumHelper.GetValue(EnumResultCode.操作成功);
                result.Obj = httpResult.Html;
            }
            catch (Exception ex)
            {
                result.Code = EnumHelper.GetValue(EnumResultCode.接口错误);
            }
            return result;
        }

        #endregion

    }
}
