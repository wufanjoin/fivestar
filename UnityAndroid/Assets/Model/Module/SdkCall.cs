using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ETModel;
using UnityEngine;

public class SdkCall : MonoBehaviour
{

    public Action<string> WeChatLoginAction;
    public Action<string> LocationAction;
    public Action<string> UrlOpenAppAction;
    public static SdkCall Ins { get; private set; }

    void Awake()
    {
        Ins = this;
    }
    public void LocationCall(string message)
    {
        LocationAction?.Invoke(message);
        Log.Debug("收到定位回调"+ message);
    }

    public void WxLoginCall(string message)
    {
        WeChatLoginAction?.Invoke(message);
        Log.Debug("收到微信登陆回调" + message);
    }

    public void WxPayCall(string message)
    {
        Log.Debug("收到微信支付回调" + message);
    }

    public static string OpenAppUrl = string.Empty;//打开APP的Url
    public void UrlOpenAppCall(string message)
    {
        //如果没有 注册事件 就保存Url 如果有注册事件 就发起事件
        if (UrlOpenAppAction != null)
        {
            UrlOpenAppAction?.Invoke(message);
        }
        else
        {
            OpenAppUrl = message;
        }
        Log.Debug("收到链接打开APP回调" + message);
    }
}
