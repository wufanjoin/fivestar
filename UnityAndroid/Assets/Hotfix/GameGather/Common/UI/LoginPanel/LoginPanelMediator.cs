using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIMediator(UIType.LoginPanel)]
    public class LoginPanelMediator:UIMediator<LoginPanelComponent,LoginPanelMediator>
    {
        public override void Awake()
        {
            Log.Debug("成功运行LoginPanelMediator");
            base.Awake();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}