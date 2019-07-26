using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.JoyLdsRoomUpFramePanel)]
    public class JoyLdsRoomUpFramePanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private Button mExitRoomBtn;
        private Image mWifiImage;
        private Text mTimeText;
        private Button mExchangeTableBtn;
        private Button mSettingBtn;
        private Button mRuleBtn;
        private Button mCollocationBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mExitRoomBtn = rc.Get<GameObject>("ExitRoomBtn").GetComponent<Button>();
            mWifiImage = rc.Get<GameObject>("WifiImage").GetComponent<Image>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mExchangeTableBtn = rc.Get<GameObject>("ExchangeTableBtn").GetComponent<Button>();
            mSettingBtn = rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            mRuleBtn = rc.Get<GameObject>("RuleBtn").GetComponent<Button>();
            mCollocationBtn = rc.Get<GameObject>("CollocationBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mExitRoomBtn.Add(ExitRoomEvnet);
        }

        public void ExitRoomEvnet()
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.RequestOutRoom);
        }
    }
}
