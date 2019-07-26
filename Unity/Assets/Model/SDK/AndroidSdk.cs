
using UnityEngine;

namespace ETModel
{



    public class AndroidSdk : IBaseSdk
    {
        private AndroidJavaClass jc;

        private AndroidJavaObject jo;

        private AndroidJavaObject Jc
        {
            get
            {
                if (jc == null)
                {
                    jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                }
                return jc;
            }
        }

        private AndroidJavaObject Jo
        {
            get
            {
                if (jo == null)
                {
                    jo = Jc.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return jo;
            }
        }

        public void WeChatLogin()
        {
            Jo.Call("WxLogin");
        }

        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            Jo.Call("WxShareUrl", url, title, description, shareType);
        }

        public void WeChatShareImage(string path, string title, string desc, int shareType)
        {

            Jo.Call("ShareImage", path, title, desc, shareType);

        }

        public int GetBatteryElectric()
        {
            return Jo.Call<int>("GetElectric");
        }



        public void CopyClipBoard(string info)
        {
            Jo.Call("CopyClipBoard", info);
        }

        private string alipayWebUrl = @"http://192.168.3.46:8888/alipay.trade.wap.pay-java-utf-8/index.html";

        //支付宝支付
        public void Alipay(string info)
        {
            Jo.Call("ZhiFuBaoPay", info);
        }

        public void WeChatpay(string prepayId, string nonceStr)
        {
            Jo.Call("WxPay", prepayId, nonceStr);
        }

        public void OpenApp(string packageName, string appName, string versionUrl)
        {

        }

        public void InstallApk(string fileFullPath)
        {
            Jo.Call("InstallApk", fileFullPath);
        }

        public void GetLocation()
        {
            Jo.Call("StartLocation");
        }
    }
}