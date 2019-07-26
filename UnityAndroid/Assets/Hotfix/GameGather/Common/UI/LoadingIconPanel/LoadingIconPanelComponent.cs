using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LoadingIconPanel)]
    public class LoadingIconPanelComponent : ErrorUIView
    {
        #region 脚本工具生成的代码
        private Image mloadingIconImage;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mloadingIconImage = rc.Get<GameObject>("loadingIconImage").GetComponent<Image>();
            InitPanel();
        }
        #endregion

        public override void GameInit()
        {
            base.GameInit();
            EventMsgMgr.RegisterEvent(CommEventID.CallRequest, CallRequestEvent);
            EventMsgMgr.RegisterEvent(CommEventID.CallResponse, CallResponseEvent);
        }

        public void CallRequestEvent(params object[] objs)
        {
            Show();
        }
        public void CallResponseEvent(params object[] objs)
        {
            Hide();
        }
        public void InitPanel()
        {
        }
    }
}
