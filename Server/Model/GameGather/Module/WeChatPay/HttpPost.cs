using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ETModel
{
  public static class HttpPost
    {
        /// <summary>
        /// POST整个字符串到URL地址中
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="jsonParas"></param>
        /// <returns></returns>
        public static string Post(string Url, string jsonParas)
        {
            string strURL = Url;

          //  Log.Debug("Post参数:");
           // Log.Debug(jsonParas);
            
            //创建一个HTTP请求 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式 
            request.Method = "POST";
            //内容类型
            //request.ContentType = "application/x-www-form-urlencoded";

            //设置参数，并进行URL编码

            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas); 

            byte[] payload;
            //将Json字符串转化为字节 
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength 
            request.ContentLength = payload.Length;
            //发送请求，获得请求流

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
            //String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();

            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            return postContent;//返回Json数据
        }
    }
}
