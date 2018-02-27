using DoubleX.Framework.Web;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Utility;
using DoubleX.Module.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc.Html;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    public class ProjectController : ApiController
    {
        protected IProjectService projectService;
        public ProjectController(IProjectService iProjectService)
        {
            projectService = iProjectService;
        }

        /// <summary>
        /// 使用记录
        /// </summary>
        public HttpResponseMessage ProjectQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var predicate = WebHelper.GetOwnerPredicate<ProjectEntity>(x => x.AccountId, request);
                //predicate = predicate.And(x => x.Id == "");
                var list = projectService.Query(requestQuery, predicate, out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
                result.Message = "查询成功";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage UpdateProject(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = JsonHelper.Deserialize<ProjectEntity>(request.JsonString);
                projectService.Update(model);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        public HttpResponseMessage InsertProject(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == DoubleX.Infrastructure.Utility.EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var projectName = JsonHelper.GetValue(request.Obj, "ProjectName");
                ProjectEntity project = new ProjectEntity()
                {
                    Id = Guid.NewGuid(),
                    AccountId = Guid.Parse(WebHelper.GetContext().Member.MemberId),
                    CreateId = Guid.Parse(WebHelper.GetContext().Member.MemberId),
                    CreateDt = DateTime.Now,
                    CreateIP = BrowserHelper.GetClientIP(),
                    LastId = Guid.Parse(WebHelper.GetContext().Member.MemberId),
                    LastDt = DateTime.Now,
                    LastLoginIP = BrowserHelper.GetClientIP(),
                    IsDelete = false,
                    ProjectName = projectName,
                    State = 0,
                    ProjectType = (int)ProjectType.BillingOnWords,
                    SurplusWords = 0,
                    TranslateKey = Guid.NewGuid().ToString(),
                    Type = 0,
                    ValidityTime = DateTime.Now
                };
                projectService.Insert(project);
                result.Message = "新增项目成功！";
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }
    }
}
