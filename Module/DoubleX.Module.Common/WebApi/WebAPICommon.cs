using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace DoubleX.Module.Common.WebApi
{
    public static class WebAPICommon
    {
        public static string GetWebRequest(string sendData)
        {
            StreamReader sr = null;
            WebResponse resp = null;
            string line;
            string result = null;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(sendData);
            myReq.Method = "GET";
            myReq.ContentType = "application/x-www-form-urlencoded";

            try
            {
                resp = myReq.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }

            if (sr == null)
            {
                return null;
            }


            while ((line = sr.ReadLine()) != null)
            {
                result = result + line;
            }


            sr.Close();
            resp.Close();
            return result;
        }

        public static string PostWebRequest(string url, string data, string type = "application/json")
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = type;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 120000;//超时限制两分钟
            byte[] btBodys = Encoding.UTF8.GetBytes(data);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            string responseContent = string.Empty;
            HttpWebResponse httpWebResponse;
            StreamReader streamReader;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                responseContent = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                responseContent = ex.Message + ex.Status;
                return responseContent;
            }

            return responseContent;
        }

        public static string Get(string url, string parameter, string type = "application/json")
        {
            url = url + parameter;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = type;
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 120000;//超时限制两分钟

            string responseContent = string.Empty;
            HttpWebResponse httpWebResponse;
            StreamReader streamReader;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                responseContent = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                responseContent = ex.Message + ex.Status;
                return responseContent;
            }

            return responseContent;
        }

        public static byte[] GetBytes(string url, string parameter, string type = "application/json")
        {
            url = url + parameter;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = type;
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 120000;//超时限制两分钟

            List<byte> responseContent = new List<byte>();
            HttpWebResponse httpWebResponse;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var stream = httpWebResponse.GetResponseStream();
                while (true)
                {
                    var item = stream.ReadByte();
                    if (-1 == item)
                        break;

                    responseContent.Add((byte)item);
                }
                stream.Close();

                return responseContent.ToArray();
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        public static string PostByUrlParas(string url, string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(url);
            objWebRequest.Method = "POST";
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            objWebRequest.ContentLength = byteArray.Length;
            Stream newStream = objWebRequest.GetRequestStream();
            // Send the data. 
            newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string textResponse = sr.ReadToEnd(); // 返回的数据

            return textResponse;
        }
    }
}
