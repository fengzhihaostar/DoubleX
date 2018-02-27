using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    //重写默认实现类 所有输出将被重新解析成 json(WebApiConfig中使用重写)
    //var jsonFormatter = new JsonMediaTypeFormatter();
    //config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
    //
    //// 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
    //// 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
    //// 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
    ////config.EnableQuerySupport();
    //
    //// 若要在应用程序中禁用跟踪，请注释掉或删除以下代码行
    //// 有关详细信息，请参阅: http://www.asp.net/web-api
    //config.EnableSystemDiagnosticsTracing();
    public class WebApiJSONFormat : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public WebApiJSONFormat(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;

            //这里使用自定义日期格式
            var settings = _jsonFormatter.SerializerSettings;
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            settings.Converters.Add(timeConverter);
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }
}
