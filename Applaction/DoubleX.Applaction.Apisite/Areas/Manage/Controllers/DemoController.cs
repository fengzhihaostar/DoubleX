using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoubleX.Applaction.Apisite.Areas.Manage.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult List()
        {
            return View();
        }
        
        public ActionResult Detail()
        {
            return View();
        }
        
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }

        #region 数据操作ApiController示例
        /*
        /// <summary>
        /// 职员获取
        /// </summary>
        public HttpResponseMessage EmployeeGet(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid id = GuidHelper.Get(request.Obj["id"]);
                if (VerifyHelper.IsEmpty(id))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                result.Obj = employeeService.Get(x => x.Id == id);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员查询
        /// </summary>
        public HttpResponseMessage EmployeeQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var list = employeeService.Query(requestQuery, StringHelper.Get(request.Obj["Key"]), out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员添加
        /// </summary>
        public HttpResponseMessage EmployeeAdd(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = JsonHelper.Deserialize<EmployeeEntity>(request.JsonString);
                if (model == null)
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }
                //model.AppProjectId = Guid.NewGuid();
                //model.Descript = iEventsHelper.GetDescript(model.Content);
                //model.CreateId = ContextHelper.GetContext().AdminModel.Account;
                //model.CreateDt = DateTime.Now;
                //model.LastId = model.CreateId;
                //model.LastDt = model.CreateDt;
                employeeService.Insert(model);
                result.Obj = model;
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 职员修改
        /// </summary>
        public HttpResponseMessage EmployeeModify(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = JsonHelper.Deserialize<EmployeeEntity>(request.JsonString);
                if (VerifyHelper.IsEmpty(model) || (!VerifyHelper.IsNull(model) && VerifyHelper.IsEmpty(model.Id)))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }

                var entity = employeeService.Get(model.Id);
                if (VerifyHelper.IsEmpty(entity))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }

                //entity.Title = model.Title;
                //entity.ThumbId = model.ThumbId;
                //entity.Tag = model.Tag;
                //entity.Descript = iEventsHelper.GetDescript(model.Content);
                //entity.Content = model.Content;
                //entity.Sort = model.Sort;
                //entity.OnLine = model.OnLine;
                //entity.LastId = ContextHelper.GetContext().AdminModel.Account;
                entity.LastDt = DateTime.Now;

                employeeService.Update(entity);

                result.Obj = entity;
            }
            return WebApiHelper.ToHttpResponseMessage(result);

        }

        /// <summary>
        /// 职员移除
        /// </summary>
        public HttpResponseMessage EmployeeRemove(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                List<Guid> ids = JsonHelper.Deserialize<List<Guid>>(StringHelper.Get(request.Obj["ids"]));
                if (ids.Count == 0)
                {
                    throw new DefaultException(EnumResultCode.参数错误);
                }
                employeeService.Remove(x => ids.Contains(x.Id));
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }
        */
        #endregion

    }
}