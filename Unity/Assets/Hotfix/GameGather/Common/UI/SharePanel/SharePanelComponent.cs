using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.SharePanel)]
    public class SharePanelComponent:PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mFriendShareBtn;
        private Button mCircleShareBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mFriendShareBtn=rc.Get<GameObject>("FriendShareBtn").GetComponent<Button>();
            mCircleShareBtn=rc.Get<GameObject>("CircleShareBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
        }
    }
}
