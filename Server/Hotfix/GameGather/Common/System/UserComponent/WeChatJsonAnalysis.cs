using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public class WeChatJsonData
    {
        public string openid;//id 针对当前应用
        public string nickname;//名字
        public int sex;//普通用户性别，1为男性，2为女性
        public string headimgurl;//头像链接
        public string unionid;//id 针对当前开发者账号

        public string language;//语言

        public string city;//普通用户个人资料填写的城市
        public string province;//普通用户个人资料填写的省份
        public string country;//国家，如中国为CN
        public string[] privilege;//用户特权信息，json数组，如微信沃卡用户为（chinaunicom）
    }

    public static class WeChatJsonAnalysis
    {
        //发起http请求用户信息
        public static WeChatJsonData HttpGetUserInfoJson(string accessTokenAndOpenidAnd)
        {
            string[] tokenOpeSplit = accessTokenAndOpenidAnd.Split('|');
            if (tokenOpeSplit.Length != 2)
            {
                return null;
            }
            string strURL = $@"https://api.weixin.qq.com/sns/userinfo?access_token={tokenOpeSplit[0]}&openid={tokenOpeSplit[1]}";
            HttpWebRequest request;
            // 创建一个HTTP请求  
            request = (HttpWebRequest)WebRequest.Create(strURL);
            //request.Method="get";  
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            return JsonToObject(responseText);
        }
        private static WeChatJsonData JsonToObject(string weChatJson)
        {
            if (weChatJson.Contains("openid"))
            {
                return JsonHelper.FromJson<WeChatJsonData>(weChatJson);
            }
            else
            {
                return null;
            }
        }
    }
}
