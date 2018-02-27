using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;


namespace DoubleX.Framework.Web.Binder
{

    //Global注册的两种方式
    //ModelBinderProviders.BinderProviders.Insert(0, new XmlModelBinderProvider());
    //ModelBinders.Binders.Add(typeof(RequestModel), new RequestBinder());

    /// <summary>
    /// MvcRequest请求信息Binder
    /// </summary>
    public class MvcRequestBindProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            //自定义模型绑定
            if (modelType == typeof(RequestModel))
            {
                return new RequestModelBind();
            }
            // 还原默认模型绑定
            return new DefaultModelBinder();
        }
    }

    /// <summary>
    /// MvcRequest请求信息Binder
    /// </summary>
    public class RequestModelBind : IModelBinder
    {
        /// <summary>
        /// 自定义请求实体
        /// </summary>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            RequestModel model = new RequestModel() {  };
            model.CurrentContext = WebHelper.GetContext();

            if (VerifyHelper.IsEmpty(controllerContext.RequestContext.HttpContext.Request))
            {
                return model;
            }

            var request = controllerContext.RequestContext.HttpContext.Request;
            var requestObj = new JObject();

            //QueryString
            if (request.QueryString != null && request.QueryString.Count > 0)
            {
                requestObj = JsonHelper.Deserialize<JObject>(StringHelper.Get(JsonHelper.FormatQueryString(request.QueryString.ToString())));
            }

            //Post
            if (StringHelper.Get(request.HttpMethod).ToLower() == "post")
            {
                JObject postObj = null;

                var contentType = StringHelper.Get(request.ContentType).ToLower();

                //json格式
                if (contentType.Contains("application/json"))
                {
                    postObj = JsonHelper.Deserialize<JObject>(StringHelper.Get(request.InputStream));
                }

                //form表单
                if (contentType.Contains("application/x-www-form-urlencoded"))
                {
                    postObj = JsonHelper.Deserialize<JObject>(JsonHelper.FormatQueryString(StringHelper.Get(request.Form)));
                }

                //处理(合并)
                if (postObj != null && postObj.Properties().Count() > 0)
                {
                    foreach (JProperty item in postObj.Properties())
                    {
                        requestObj[item.Name] = item.Value;
                    }
                }
            }
            model.Obj = requestObj;
            return model;
        }
    }
}
