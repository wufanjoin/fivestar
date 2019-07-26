
using System;
using LitJson;
using UnityEngine;
namespace ETModel
{
    public class DefaultSdk : IBaseSdk
    {
        public void WeChatLogin()
        {
            Log.Debug("DefaultSdk---WeChatLogin");
        }

        public void WeChatShareImage(string path, string title, string desc, int scene)
        {
            Log.Debug("DefaultSdk---WeChatShareImage");
        }

        public void WeChatShareUrl(string url, string title, string description, int scene)
        {
            Log.Debug("DefaultSdk---WeChatShareUrl");
        }

        public void WeChatShareText(string content, string description, int scene)
        {
            Log.Debug("DefaultSdk---WeChatShareText");
        }

        public int GetBatteryElectric()
        {
            return 0;
        }

        public int GetWifiStrength()
        {
            return 0;
        }

        public void CopyClipBoard(string info)
        {
            TextEditor te = new TextEditor();
            te.content = new GUIContent(info);
            te.SelectAll();
            te.Copy();
        }
        private string alipayWebUrl = @"http://123.103.13.51:8010/zhifubao/index.html";
        public void Alipay(string info)
        {
            Log.Debug("DefaultSdk---Alipay");
        }

        public void WeChatpay(string prepayId, string nonceStr)
        {
            Log.Debug("DefaultSdk---WeChatpay");
        }

        public void OpenApp(string packageName, string appName, string versionUrl)
        {
            Log.Debug("DefaultSdk---OpenApp");
        }

        public void InstallApk(string fileFullPath)
        {
            Log.Debug("DefaultSdk---InstallApk");
        }

        public void OpenPhotoAlbum()
        {
            Log.Debug("DefaultSdk---OpenPhotoAlbum");
        }

        public void GetLocation()
        {
            Log.Debug("DefaultSdk---GetLocation");
        }
    }


}