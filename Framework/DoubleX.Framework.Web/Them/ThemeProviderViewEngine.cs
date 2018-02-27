using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace DoubleX.Framework.Web.Them
{
    /// <summary>
    /// 自定义风格驱动类
    /// </summary>
    public class ThemeProviderViewEngine: VirtualPathProviderViewEngine
    {
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            //if (controllerContext == null)
            //{
            //    throw new ArgumentNullException("controllerContext");
            //}
            //if (String.IsNullOrEmpty(partialViewName))
            //{
            //    throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            //}

            //string[] searched;
            //var theme = GetCurrentTheme();
            //string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            //string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, theme, CacheKeyPrefixPartial, useCache, out searched);

            //if (String.IsNullOrEmpty(partialPath))
            //{
            //    return new ViewEngineResult(searched);
            //}

            //return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);

            return base.FindPartialView(controllerContext, partialViewName, false);

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //if (controllerContext == null)
            //{
            //    throw new ArgumentNullException("controllerContext");
            //}
            //if (String.IsNullOrEmpty(viewName))
            //{
            //    throw new ArgumentException("View name cannot be null or empty.", "viewName");
            //}

            //string[] viewLocationsSearched;
            //string[] masterLocationsSearched;

            //var theme = GetCurrentTheme();
            //string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            //string viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats, "ViewLocationFormats", viewName, controllerName, theme, CacheKeyPrefixView, useCache, out viewLocationsSearched);
            //string masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats, "MasterLocationFormats", masterName, controllerName, theme, CacheKeyPrefixMaster, useCache, out masterLocationsSearched);

            //if (String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName)))
            //{
            //    return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            //}

            //return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
            return base.FindView(controllerContext, viewName, masterName, false);
        }


        //以下两个在NOPCommon中未实现
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            throw new NotImplementedException();
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            throw new NotImplementedException();
        }
    }
}
