using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using ETModel;

namespace ETHotfix
{
  public  class MonitorWxPayHttp
    {
        private HttpListener MyListerner;
        public  MonitorWxPayHttp()
        {

            Thread t1 = new Thread(Start);
            t1.Start();
            //  Start();
        }

        public  void Start()
        {
            MyListerner = new HttpListener();
            while (true)
            {
                try
                {
                    MyListerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                    ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
                    HttpListenerAddressConfig httpListenerAddressConfig = (HttpListenerAddressConfig)configComponent.Get(typeof(HttpListenerAddressConfig), 0);
                    Log.Debug("服务器地址......"+ httpListenerAddressConfig.Address);
                    MyListerner.Prefixes.Add(httpListenerAddressConfig.Address);
                    MyListerner.Start();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex+"服务器启动失败......");
                    break;
                }
                Log.Debug("服务器启动成功......");

                //线程池
                int minThreadNum;
                int portThreadNum;
                int maxThreadNum;
                ThreadPool.GetMaxThreads(out maxThreadNum, out portThreadNum);
                ThreadPool.GetMinThreads(out minThreadNum, out portThreadNum);
                Log.Debug($"最大线程数：{maxThreadNum}");
                Log.Debug($"最小空闲线程数：{minThreadNum}");
                //ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc1), x);

                Log.Debug("\n等待客户连接中......");
                while (true)
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = MyListerner.GetContext();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
                }
            }
        }
        void TaskProc(object obj)
        {
            HttpListenerContext ctx = (HttpListenerContext)obj;

            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码

            //接收POST参数
            Stream stream = ctx.Request.InputStream;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            string body = reader.ReadToEnd();
            string postBody = HttpUtility.UrlDecode(body);
            Log.Debug("收到POST数据：\r\n" + postBody);

            var replyMsg = "0";// ProcessMessage(body);

            //使用Writer输出http响应代码,UTF8格式
            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
            {
                writer.Write(replyMsg);
                writer.Close();
                ctx.Response.Close();
            }

            try
            {
                if (string.IsNullOrEmpty(postBody))
                {
                    return;
                }
                XElement xe = XElement.Parse(postBody);
                string outTradeNo = xe.Element("out_trade_no").Value;
                TopUpComponent.Ins.FinishPay(outTradeNo);
            }
            catch (Exception e)
            {
                Log.Error("解析post出错"+e);
                throw;
            }
         
        }

      
    }
}
