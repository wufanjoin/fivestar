using System;
using System.Collections;
using System.IO;
using ETHotfix;
using ETModel;
using UnityEngine;

namespace ETModel
{
    public class WxShareSceneType
    {
        public const int Friend = 1; //好友
        public const int Circle = 2; //朋友圈
    }

    public class NetworkType
    {
        public const int None = 0; //没有网络
        public const int Wifi = 1; //wifi
        public const int Mobile = 2; //移动网络
    }




    public class SdkMgr : Single<SdkMgr>
    {
        private IBaseSdk mSdk;

        public SdkMgr()
        {

#if   UNITY_ANDROID&&!UNITY_EDITOR
            Log.Debug("安卓平台");
        mSdk = new AndroidSdk();
#elif UNITY_IPHONE&&!UNITY_EDITOR
        mSdk = new IOSSdk();
                Log.Debug("苹果平台");
#else
            mSdk = new DefaultSdk();
                Log.Debug("电脑平台");
#endif


        }
        //微信登陆
        public void WeChatLogin()
        {
            mSdk.WeChatLogin();
        }

        //获取定位地址
        public void GetLocation()
        {
            mSdk.GetLocation();
        }

        //微信分享截屏
        public void WeChatShareScreen()
        {
            CoroutineMgr.StartCoroutinee(CaptureScreen(ScreenFinsh));
        }

        //截屏完成
        private void ScreenFinsh(string imagePath)
        {
            WeChatShareImage(imagePath, "", "", WxShareSceneType.Friend);
        }

        //打开一个APK包 只有安卓端才有用
        public void installApk(string fileFullPath)
        {
            mSdk.InstallApk(fileFullPath);
        }

        //微信分享连接
        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            mSdk.WeChatShareUrl(url, title, description, shareType);
        }

        //微信分享图片
        public void WeChatShareImage(string imagePath,string title,string desc,int sharteType)
        {
            mSdk.WeChatShareImage(imagePath, title, desc, sharteType);
        }


        //获取电量
        public int GetBatteryElectric()
        {
            return mSdk.GetBatteryElectric();
        }

        //支付宝支付
        public void Alipay(string info)
        {
            mSdk.Alipay(info);
        }

        //微信支付
        public void WeChatpay(string prepayId, string nonceStr)
        {
            mSdk.WeChatpay(prepayId, nonceStr);
        }


        //复制文字到剪贴板
        public void CopyText(string info)
        {
            mSdk.CopyClipBoard(info);
        }

        //获取网络类型
        public int GetNetworkInfo()
        {
            int networkType = NetworkType.None;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                networkType = NetworkType.None;
            }
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                networkType = NetworkType.Wifi;
            }
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                networkType = NetworkType.Mobile;
            }
            return networkType;
        }

        //根据screenshotRectTransform 进行截图 他的坐标点必须为0.0 不然会有偏差
        public IEnumerator CaptureScreen(Action<string> ScreenshotCall)
        {
            string ssname = "lastss.png";
            string sspath = Path.Combine(Application.temporaryCachePath, ssname);

            if (File.Exists(sspath))
            {
                File.Delete(sspath);
            }
            Texture2D texture1;
            byte[] bytes;
            // Wait for screen rendering to complete
            yield return new WaitForEndOfFrame();
            Rect rect = GetScreenshotRect(null);
            texture1 = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            texture1.ReadPixels(rect, 0, 0);
            texture1.Apply();
            yield return 0;
            bytes = texture1.EncodeToPNG();
            File.WriteAllBytes(sspath, bytes);
            ScreenshotCall.Invoke(sspath);
        }

        //获取截图区域对应的Rect
        private Rect GetScreenshotRect(RectTransform screenshotRectTransform)
        {
            return new Rect(Screen.width * 0f, Screen.height * 0f, Screen.width * 1f, Screen.height * 1f); //直接截全屏的图
            Rect m_Rect;
            float originalVillegas = 1280.00f / 720.00f;
            float gap = 0.66f;
            float des = Screen.width - originalVillegas * Screen.height;
            m_Rect = new Rect();
            Vector3[] fourCornersArray = new Vector3[4];
            screenshotRectTransform.GetWorldCorners(fourCornersArray);
            m_Rect.width = fourCornersArray[2].x - fourCornersArray[0].x;
            m_Rect.height = fourCornersArray[2].y - fourCornersArray[0].y;
            m_Rect.width = screenshotRectTransform.sizeDelta.x * (Screen.width - des) / 1000.00f * gap;
            m_Rect.height = screenshotRectTransform.sizeDelta.y * Screen.height / 1000.00f * 1.05f;
            m_Rect.x = Screen.width * 0.5f - m_Rect.width / 2;
            m_Rect.y = Screen.height * 0.5f - m_Rect.height / 2;
            return m_Rect;
        }
    }
}