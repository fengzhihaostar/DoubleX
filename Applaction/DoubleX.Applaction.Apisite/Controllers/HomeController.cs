using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoubleX.Infrastructure.Utility;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Organize;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DoubleX.Module.Common.WebApi;
using System.IO;

namespace DoubleX.Applaction.Apisite.Controllers
{
    public class HomeController : MvcBaseController
    {
        public ActionResult Index()
        {
            //MySql 测试
            //var service = new RoleService();
            //var model = new RoleEntity()
            //{
            //    Title = "test[" + Guid.NewGuid().ToString() + "]",
            //    IsDelete = true,
            //    CreateId = Guid.Empty,
            //    CreateDt = DateTime.Now,
            //    LastId = Guid.NewGuid(),
            //    LastDt = DateTime.Now
            //};
            //service.Insert(model);

            //Redis 测试
            //RedisHelper.Set("test", "aaaaaaa");
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public JsonResult JsonTest()
        {
            return MvcHelper.ToJsonResult(new { name = "123" });
        }

        public ActionResult ErrorTest()
        {
            var i = 0;
            var c = 0 / i;
            return MvcHelper.ToJsonResult(new { name = "123" });
        }

        public ActionResult Guidance()
        {
            return View();
        }

        public ActionResult Translate()
        {
            return View();
        }

        /// <summary>
        /// 内容分页
        /// </summary>
        /// <param name="iStart"></param>
        /// <param name="iCount"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTranslateText(string sl, string tl, string src)
        {
            try
            {
                JObject data = new JObject
                {
                    {"key",  System.Configuration.ConfigurationManager.AppSettings["ApiKey"]},
                    {"Source", sl},
                    {"Target",tl},
                    { "q",src}
                };
                var res = WebAPICommon.PostWebRequest(System.Configuration.ConfigurationManager.AppSettings["BaseAPIUrl"] + "/api/TranslateAPI", JsonConvert.SerializeObject(data));

                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult GetTranslateInfo()
        {
            try
            {
                string content = Request.Form["content"] ?? "";
                HttpPostedFileBase postFile = Request.Files["audioFile"];
                string source = Request.Form["source"] ?? "";
                string target = Request.Form["target"] ?? "";
                var obj = new JObject
                    {
                        {"Key" , System.Configuration.ConfigurationManager.AppSettings["ApiKey"] },
                        {"Source" , source },
                        {"Target", target },
                        {"Identify", 0 },
                        {"Rate", 16000 },
                        {"Spd" , 5 },
                        {"Vol" , 5 },
                        {"Per" , 0 },
                        {"Pit" , 5 }
                    };
                //纯文本
                if ((!string.IsNullOrEmpty(content)) && !content.StartsWith("http://") && !content.StartsWith("https://") && !content.StartsWith("gs://"))
                {
                    string param = "key=" + System.Configuration.ConfigurationManager.AppSettings["ApiKey"] + "&source=" + source + "&target=" + target + "&content=" + content;
                    return Redirect(System.Configuration.ConfigurationManager.AppSettings["BaseAPIUrl"] + "/api/TxtTranslate?" + param);
                }

                //链接
                else if (!string.IsNullOrEmpty(content) && (content.StartsWith("http://") || content.StartsWith("https://") || content.StartsWith("gs://")))
                {
                    obj.Add("Format", content.Split('.').Last());
                    obj.Add("Url", content);
                }
                //文件
                else if (postFile != null && postFile.ContentLength > 0)
                {
                    obj.Add("Len", postFile.ContentLength);
                    byte[] bytes = null;
                    using (var binaryReader = new BinaryReader(postFile.InputStream))
                    {
                        bytes = binaryReader.ReadBytes(postFile.ContentLength);
                    }
                    string base64String = Convert.ToBase64String(bytes);
                    base64String = base64String.Replace("=", "%3d");
                    obj.Add("Format", postFile.FileName.Split('.').Last());
                    obj.Add("Content", base64String);
                }
                //高清格式语音使用google识别
                if (obj.GetValue("Format").ToString().ToLower()== "flac")
                {
                    obj["Identify"] = 1;
                }
                var responseInfo = WebAPICommon.PostWebRequest(System.Configuration.ConfigurationManager.AppSettings["BaseAPIUrl"] + "/api/VoiceTranslate", obj.ToString(), "text/json");
                if (responseInfo != null)
                {
                    var objInfo = (JObject)JsonConvert.DeserializeObject(responseInfo);
                    if (objInfo.GetValue("Code") != null && objInfo.GetValue("Code").ToString() == "0")
                    {
                        var translateFile = objInfo.GetValue("AudioData").ToString();
                        byte[] imageBytes = Convert.FromBase64String(translateFile);
                        return File(imageBytes, "audio/mp3", DateTime.Now.ToString("yyyyMMddhhmmss") + "." + obj.GetValue("Format").ToString());
                    }
                    else
                    {
                        return Content((objInfo.GetValue("Message") != null ? objInfo.GetValue("Message").ToString() : ""));
                    }
                }
                return Content("无可翻译内容");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}