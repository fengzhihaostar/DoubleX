using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Framework.Web.Binder
{
    /// <summary>
    /// ApiRequest请求信息Binder
    /// </summary>
    public class ApiRequestBind : HttpParameterBinding
    {
        private struct AsyncVoid { }

        public ApiRequestBind(HttpParameterDescriptor desc) : base(desc) { }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            RequestModel model = new RequestModel();
            model.CurrentContext = WebHelper.GetContext();

            JObject obj = new JObject();
            if (actionContext.Request.RequestUri.Query != null)
            {
                obj = JsonHelper.Deserialize<JObject>(StringHelper.Get(JsonHelper.FormatQueryString(actionContext.Request.RequestUri.Query)), isNewObj: true);
            }
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                JObject postObj = JsonHelper.Deserialize<JObject>(actionContext.Request.Content.ReadAsStringAsync().Result);
                if (postObj != null && postObj.Properties().Count() > 0)
                {
                    foreach (JProperty item in postObj.Properties())
                    {
                        obj[item.Name] = item.Value;
                    }
                }

            }
            //异步
            //var type = binding.ParameterBindings[0].Descriptor.ParameterType;
            //actionContext.Request.Content.ReadAsStringAsync().ContinueWith((task) =>
            //{
            //    string postJson = task.Result;
            //    if (!VerifyHelper.IsEmpty(postJson))
            //    {
            //        request = JsonHelper.Deserialize<RequestModel>(postJson);
            //    }
            //});

            model.Obj = obj == null ? new JObject() : obj;
            SetValue(actionContext, model);
            TaskCompletionSource<AsyncVoid> task = new TaskCompletionSource<AsyncVoid>();
            task.SetResult(default(AsyncVoid));
            return task.Task;
        }
    }
}
