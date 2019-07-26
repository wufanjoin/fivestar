using System;
using UnityEngine;
namespace ETModel
{
    public interface IBaseSdk
    {
        //调用第三方登录
        void WeChatLogin();
        //微信分享图片
        void WeChatShareImage(string path, string title, string desc, int shareType);
        //微信分享url链接
        void WeChatShareUrl(string url, string title, string description, int shareType);

        //获取电量
        int GetBatteryElectric();
        //复制文字到剪贴板
        void CopyClipBoard(string info);

        //获取定位地址
        void GetLocation();

        //支付宝支付
        void Alipay(string info);

        //微信支付
        void WeChatpay(string prepayId, string nonceStr);

        //打开一个APP 没有安装就下载 苹果跳APP Store

        void OpenApp(string packageName, string appName, string versionUrl);
        //打开一个APK安卓包 此方法只有安卓端才有用
        void InstallApk(string fileFullPath);
    }
}