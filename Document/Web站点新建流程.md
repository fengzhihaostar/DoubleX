01.新建项目，更新Nuget包，移除Bootstrap包，并移除多余代码及文件(Content,View,Controller)，置为一个干净的项止
02.移除App_Data,App_Start文件夹
03.新建文件夹,Config、Log、App_GlobalResources、App_GlobalResources/Script.resx、App_GlobalResources/Code.resx、Content/css、Content/images
04.清空Gloable.asax文件，App_Start方法中的Code 
05.安装Nuget包 log4net,
06.添加引用 DoubleX.Infrastructure.Utility/DoubleX.Infrastructure.Utility.Net45,DoubleX.Infrastructure.Core
07.Web.Config 文件
configSections-> 新增
	<configSections>
		<sectionGroup name="system.web.webPages.razor" type="System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35">
			<section name="host" type="System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" requirePermission="false" />
			<section name="pages" type="System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" requirePermission="false" />
		</sectionGroup>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
		<section name="DoubleXSetting" type="DoubleX.Infrastructure.Core.Config.SettingConfigModel,DoubleX.Infrastructure.Core" />
	</configSections>
appsettings->新增
	<add key="vs:EnableBrowserLink" value="false" />

新增节点：
	<log4net configSource="Config\Log4Net.config"></log4net>

新增节点(并配置以下信息)：
	<DoubleXSetting>
		<Groups>
		  <Item key="System">
			<add key="debug" value="1"></add>
			<add key="language" value="en-us,zh-cn"></add>
		  </Item>
		  <Item key="Database">
			<add key="MongoDefault" value="mongodb://139.196.229.64:27017/TestDB"></add>
			<add key="RedisDefault" value="Reads=139.196.229.64:6379;Writes=139.196.229.64:6379;MaxWritePoolSize=60;MaxReadPoolSize=60;AutoStart=true"></add>
			<add key="SQLiteDefault" value=""></add>
			<add key="EntityFrameworkDefault" value=""></add>
			<add key="SqlServerDefault" value=""></add>
			<add key="MySqlDefault" value=""></add>
		  </Item>
		  <Item key="Website">
			<add key="webPath" value="/"></add>
			<add key="managePath" value="/manage"></add>
			<add key="staticUrl" value=""></add>
			<add key="cdnUrl" value=""></add>
			<add key="jqueryCDN" value=""></add>
			<add key="bootstrapCDN" value=""></add>
		  </Item>
		  <Item key="Resources">
			<!--脚本资源文件值示例：Resources.Script|DoubleX.Culture,... (资源文件|JSON对象)-->
			<add key="scriptResource" value="Resources.Script|DoubleX.Culture"></add>
			<add key="cacheResource" value="Resources.Code"></add>
		  </Item>
		  <Item key="Options">
			<add key="descriptLength" value="200"></add>
			<add key="uploadPath" value="/upload"></add>
			<!--http://106.14.37.116/api http://192.168.1.104/api-->
			<add key="imgUrl" value="http://192.168.1.104"></add>
		  </Item>
		</Groups>
	</DoubleXSetting>

新增节点(页面namespace)
	<system.web.webPages.razor>
	    <host factoryType="System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" />
	    <pages pageBaseType="System.Web.Mvc.WebViewPage">
	      <namespaces>
	        <add namespace="System.Web.Mvc" />
	        <add namespace="System.Web.Mvc.Ajax" />
	        <add namespace="System.Web.Mvc.Html" />
	        <add namespace="System.Web.Routing" />
	        <add namespace="System.Web.Optimization" />
	        <add namespace="DoubleX.Infrastructure.Utility" />
	        <add namespace="DoubleX.Framework.Web.Helper" />
	      </namespaces>
	    </pages>
	</system.web.webPages.razor>

08.配置Error
   
   system.web->新增 404处理方式,建议开发时Off,测试或生成改为On
   //404使用了特性路由，注册时必须开启特性路由
   <customErrors mode="Off">
      <error statusCode="404" redirect="/error/404" />
   </customErrors>


   //新增Error控制器
   public class ErrorController : Controller
    {
        [Route("error")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("error/404")]
        public ActionResult NotFound()
        {
            return View();
        }
	}

	Index.cshtml
	@{
		Layout = null;
	}
	<!DOCTYPE html>
	<html>
	<head>
		<meta name="viewport" content="width=device-width" />
		<title>异常错误</title>
	</head>
	<body>
		<p>异常错误：</p>
		<p>
			Code：@UrlsHelper.GetQueryValue("Code");
		</p>
		<p>
			消息：@UrlsHelper.GetQueryValue("msg");
		</p>
	</body>
	</html>

	NotFound.cshtml
	@{
		Layout = null;
	}
	<!DOCTYPE html>
	<html>
	<head>
		<meta name="viewport" content="width=device-width" />
		<title>404错误</title>
	</head>
	<body>
		<div> 
			404错误,打不到页面
		</div>
	</body>
	</html>




09.修改Gloable.asax代码为
	/// <summary>
    /// 运行工作器
    /// </summary>
    private static DoubleXWorking<DoubleXMvcHosting> worker;

    /// <summary>
    /// 开始运行
    /// </summary>
    protected void Application_Start()
    {
        worker = new DoubleXWorking<DoubleXMvcHosting>(this);
        worker.ApplactionInit(this);
        worker.ApplactionStart(this);
    }

    /// <summary>
    /// 请求开始
    /// </summary>
    protected void Application_BeginRequest()
    {
        worker.RequestBegin(this);
    }

    /// <summary>
    /// 请求结束
    /// </summary>
    protected void Application_EndRequest()
    {
        worker.RequestEnd(this);
    }

10.运行程序

11.HomeController 新增 JSON测试
	
	//MVC JSON返回
	public ActionResult JsonTest() {
		return MvcHelper.ToJsonResult(new { name = "123" });
	}

	//程序500错误提示
	public ActionResult ErrorTest()
    {
        var i = 0;
        var c = 0 / i;
        return MvcHelper.ToJsonResult(new { name = "123" });
    }

	//开启CustomerError为On 输入一个不存在的地址测试404错误

12.新增Api Areas测试
	//protected readonly IAppProjectService appProjectService;
    //public AppController(IAppProjectService iAppProjectService)
    //{
    //    appProjectService = iAppProjectService;
    //}
    [HttpGet, HttpPost]
    public HttpResponseMessage TestApi(RequestModel request)
    {
        var result = new ResultModel<string>();
        if (VerifyHelper.IsEmpty(request))
        {
            throw new DefaultException(EnumResultCode.请求错误);
        }
        result.Obj = JsonHelper.Serialize(request);
        return WebApiHelper.ToHttpResponseMessage(result);
    }
    //http://localhost:11301/api/App/testapi?id=1&id2=2 (可移除?id=1&id2=2进行测试)

