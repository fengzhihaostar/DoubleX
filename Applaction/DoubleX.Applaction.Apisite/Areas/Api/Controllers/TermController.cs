using DoubleX.Framework.Web;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Utility;
using DoubleX.Module.Common.WebApi;
using DoubleX.Module.Term;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    public class TermController : ApiController
    {
        protected ITermService termService;
        public TermController(ITermService iTermService)
        {
            termService = iTermService;
        }

        /// <summary>
        /// 使用记录
        /// </summary>
        public HttpResponseMessage TermQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(JsonHelper.GetValue(request.Obj, "RequestModel"));
                var projectId = JsonHelper.GetValue(request.Obj, "ProjectId");
                var termSrcLang = JsonHelper.GetValue(request.Obj, "TermSrcLang");
                var termTgtLang = JsonHelper.GetValue(request.Obj, "TermTgtLang");
                var userId = JsonHelper.GetValue(request.Obj, "UserId");
                var projectGuid = Guid.Empty;
                if ("0" != projectId)
                {
                    projectGuid = Guid.Parse(projectId);
                    var list = new List<TermEntity>();
                    if ("-9999" == userId)
                        list = termService.Query(requestQuery, item => item.ProjectId == projectGuid
                            && item.TermSrcLang == termSrcLang
                            && item.TermTgtLang == termTgtLang, out total);
                    else
                        list = termService.Query(requestQuery, item => item.ProjectId == projectGuid
                            && item.TermSrcLang == termSrcLang
                            && item.TermTgtLang == termTgtLang
                            && item.UserId == userId, out total);
                    result.Obj = new ResultQueryModel(requestQuery, total, list);
                }
                else
                {
                    var projectIds = termService.GetProjectIdsByAccountId(WebHelper.GetContext().Member.MemberId);
                    var list = termService.Query(requestQuery, item => projectIds.Contains(item.ProjectId.ToString()) && item.TermSrcLang == termSrcLang
                            && item.TermTgtLang == termTgtLang, out total);
                    result.Obj = new ResultQueryModel(requestQuery, total, list);
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage UpdateTerm(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var id = Guid.Parse(JsonHelper.GetValue(request.Obj, "Id"));
                var tgt = JsonHelper.GetValue(request.Obj, "Tgt");
                var item = termService.Query(i => i.Id == id).FirstOrDefault();
                item.TermTgt = tgt;
                termService.Update(item);
                result.Message = "保存成功！";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage RemoveTerm(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var id = Guid.Parse(JsonHelper.GetValue(request.Obj, "Id"));
                var tgt = JsonHelper.GetValue(request.Obj, "Tgt");
                var item = termService.Query(i => i.Id == id).FirstOrDefault();
                termService.Delete(item);
                result.Message = "删除术语成功！";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage InsertTerm(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var projectId = JsonHelper.GetValue(request.Obj, "ProjectId");
                var projectGuid = Guid.Parse(projectId);
                var termSrc = JsonHelper.GetValue(request.Obj, "TermSrc");
                var termSrcLang = JsonHelper.GetValue(request.Obj, "TermSrcLang");
                var termTgt = JsonHelper.GetValue(request.Obj, "TermTgt");
                var termTgtLang = JsonHelper.GetValue(request.Obj, "TermTgtLang");
                var flag = JsonHelper.GetValue(request.Obj, "UserId");
                var list = termService.Query(item => item.ProjectId == projectGuid && item.TermSrc == termSrc && item.TermSrcLang == termSrcLang && item.TermTgtLang == termTgtLang);
                if (list != null && list.Count > 0)
                    result.Message = "已具有相同术语！";
                else
                {
                    TermEntity term = new TermEntity()
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = projectGuid,
                        CreateId = Guid.Parse(WebHelper.GetContext().Member.MemberId),
                        CreateDt = DateTime.Now,
                        CreateIP = BrowserHelper.GetClientIP(),
                        LastId = Guid.Parse(WebHelper.GetContext().Member.MemberId),
                        LastDt = DateTime.Now,
                        LastIP = BrowserHelper.GetClientIP(),
                        IsDelete = false,
                        TermSrc = termSrc,
                        TermSrcLang = termSrcLang,
                        TermTgt = termTgt,
                        TermTgtLang = termTgtLang,
                        PlatformCode = 0,
                        UserId = flag,
                        Type = 0,
                        Status = 0
                    };
                    termService.Insert(term);
                    result.Message = "新增术语成功！";
                }
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage StatisticsUserIds(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var projectId = JsonHelper.GetValue(request.Obj, "ProjectId");
                var projectGuid = Guid.Parse(projectId);
                result.Obj = termService.StatisticsUserIds(projectGuid);
                result.Message = "查询项目成功！";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage TermDemo(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var key = JsonHelper.GetValue(request.Obj, "Key");
                var sl = JsonHelper.GetValue(request.Obj, "Source");
                var tl = JsonHelper.GetValue(request.Obj, "Target");
                var q = JsonHelper.GetValue(request.Obj, "Q");
                var userId = JsonHelper.GetValue(request.Obj, "UserId");
                JObject data = new JObject
                {
                    {"Key", key},
                    {"Source", sl},
                    {"Target",tl},
                    { "Q",q},
                    {"UserId",userId}
                };
                var res = JsonConvert.DeserializeObject<JObject>(WebAPICommon.PostWebRequest(System.Configuration.ConfigurationManager.AppSettings["BaseAPIUrl"] + "/api/TranslateAPI", JsonConvert.SerializeObject(data)));
                result.Obj = res;
                result.Message = "翻译成功！";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }
    }
}
