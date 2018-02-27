01.�½���Ŀ������Nuget�����Ƴ�Bootstrap�������Ƴ�������뼰�ļ�(Content,View,Controller)����Ϊһ���ɾ�����ֹ
02.�Ƴ�App_Data,App_Start�ļ���
03.�½��ļ���,Config��Log��App_GlobalResources��App_GlobalResources/Script.resx��App_GlobalResources/Code.resx��Content/css��Content/images
04.���Gloable.asax�ļ���App_Start�����е�Code 
05.��װNuget�� log4net,
06.������� DoubleX.Infrastructure.Utility/DoubleX.Infrastructure.Utility.Net45,DoubleX.Infrastructure.Core
07.Web.Config �ļ�
configSections-> ����
	<configSections>
		<sectionGroup name="system.web.webPages.razor" type="System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35">
			<section name="host" type="System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" requirePermission="false" />
			<section name="pages" type="System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" requirePermission="false" />
		</sectionGroup>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
		<section name="DoubleXSetting" type="DoubleX.Infrastructure.Core.Config.SettingConfigModel,DoubleX.Infrastructure.Core" />
	</configSections>
appsettings->����
	<add key="vs:EnableBrowserLink" value="false" />

�����ڵ㣺
	<log4net configSource="Config\Log4Net.config"></log4net>

�����ڵ�(������������Ϣ)��
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
			<!--�ű���Դ�ļ�ֵʾ����Resources.Script|DoubleX.Culture,... (��Դ�ļ�|JSON����)-->
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

�����ڵ�(ҳ��namespace)
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

08.����Error
   
   system.web->���� 404����ʽ,���鿪��ʱOff,���Ի����ɸ�ΪOn
   //404ʹ��������·�ɣ�ע��ʱ���뿪������·��
   <customErrors mode="Off">
      <error statusCode="404" redirect="/error/404" />
   </customErrors>


   //����Error������
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
		<title>�쳣����</title>
	</head>
	<body>
		<p>�쳣����</p>
		<p>
			Code��@UrlsHelper.GetQueryValue("Code");
		</p>
		<p>
			��Ϣ��@UrlsHelper.GetQueryValue("msg");
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
		<title>404����</title>
	</head>
	<body>
		<div> 
			404����,�򲻵�ҳ��
		</div>
	</body>
	</html>




09.�޸�Gloable.asax����Ϊ
	/// <summary>
    /// ���й�����
    /// </summary>
    private static DoubleXWorking<DoubleXMvcHosting> worker;

    /// <summary>
    /// ��ʼ����
    /// </summary>
    protected void Application_Start()
    {
        worker = new DoubleXWorking<DoubleXMvcHosting>(this);
        worker.ApplactionInit(this);
        worker.ApplactionStart(this);
    }

    /// <summary>
    /// ����ʼ
    /// </summary>
    protected void Application_BeginRequest()
    {
        worker.RequestBegin(this);
    }

    /// <summary>
    /// �������
    /// </summary>
    protected void Application_EndRequest()
    {
        worker.RequestEnd(this);
    }

10.���г���

11.HomeController ���� JSON����
	
	//MVC JSON����
	public ActionResult JsonTest() {
		return MvcHelper.ToJsonResult(new { name = "123" });
	}

	//����500������ʾ
	public ActionResult ErrorTest()
    {
        var i = 0;
        var c = 0 / i;
        return MvcHelper.ToJsonResult(new { name = "123" });
    }

	//����CustomerErrorΪOn ����һ�������ڵĵ�ַ����404����

12.����Api Areas����
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
            throw new DefaultException(EnumResultCode.�������);
        }
        result.Obj = JsonHelper.Serialize(request);
        return WebApiHelper.ToHttpResponseMessage(result);
    }
    //http://localhost:11301/api/App/testapi?id=1&id2=2 (���Ƴ�?id=1&id2=2���в���)

