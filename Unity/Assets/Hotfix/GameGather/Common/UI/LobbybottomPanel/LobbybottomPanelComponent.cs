using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LobbybottomPanel)]
    public class LobbybottomPanelComponent:ChildUIView
    {
        #region 脚本工具生成的代码
        private Button mShoppingBtn;
        private Button mNoticeBtn;
        private Button mFriendBtn;
        private Button mActivityBtn;
        private Button mMailBtn;
        private Button mHongBaoBtn;
        private Button mSettingBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mShoppingBtn=rc.Get<GameObject>("ShoppingBtn").GetComponent<Button>();
            mNoticeBtn=rc.Get<GameObject>("NoticeBtn").GetComponent<Button>();
            mFriendBtn=rc.Get<GameObject>("FriendBtn").GetComponent<Button>();
            mActivityBtn=rc.Get<GameObject>("ActivityBtn").GetComponent<Button>();
            mMailBtn=rc.Get<GameObject>("MailBtn").GetComponent<Button>();
            mHongBaoBtn=rc.Get<GameObject>("HongBaoBtn").GetComponent<Button>();
            mSettingBtn=rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mShoppingBtn.Add(ShoppingBtnEvent);
            mNoticeBtn.Add(NoticeBtnEvent);
            mFriendBtn.Add(FriendBtnEvent);
            mActivityBtn.Add(ActivityBtnEvent);
        }
        public void ActivityBtnEvent()
        {
            mUIComponent.Show(UIType.ActivityLobbyPanel);
            SessionComponent.Instance.Send(new Actor_JoyLds_CallLanlord() { Result = true });
        }
        public void ShoppingBtnEvent()
        {
            UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Jewel);
        }
        public async void NoticeBtnEvent()
        {
            L2C_GetAnnouncement g2CLoginGate = (L2C_GetAnnouncement)await SessionComponent.Instance.Session.Call(new C2L_GetAnnouncement());

            UIComponent.GetUiView<AnnouncementPanelComponent>()
                .ChangAnnouncementContent(g2CLoginGate.Message);
        }
        public void FriendBtnEvent()
        {
         
        }
        
    }
}
