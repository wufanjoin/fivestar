using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class RightBtnView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mRuleBtn;
        private Button mSetBtn;
        private Button mAnnouncementBtn;
        private Button mServiceBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mRuleBtn = rc.Get<GameObject>("RuleBtn").GetComponent<Button>();
            mSetBtn = rc.Get<GameObject>("SetBtn").GetComponent<Button>();
            mAnnouncementBtn = rc.Get<GameObject>("AnnouncementBtn").GetComponent<Button>();
            mServiceBtn = rc.Get<GameObject>("ServiceBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mRuleBtn.Add(RuleBtnEvent);
            mSetBtn.Add(SetBtnEvent);
            mAnnouncementBtn.Add(AnnouncementBtnEvent);
            mServiceBtn.Add(ServiceBtnEvent);
        }

        public void RuleBtnEvent()
        {
            UIComponent.GetUiView<RulePanelComponent>().Show();
        }

        public void SetBtnEvent()
        {
            UIComponent.GetUiView<FiveStarSetPanelComponent>().Show();
        }

        public void AnnouncementBtnEvent()
        {
            UIComponent.GetUiView<ActivityAndAnnouncementPanelComponent>().ShowPanel(ActivityAndAnnouncementType.Announcement);
        }

        public async void ServiceBtnEvent()
        {
            RepeatedField<ServiceInfo> serviceInfos =
               await UIComponent.GetUiView<AgencyInvitePanelComponent>().GetServiceInfos();
            if (serviceInfos.Count > 0)
            {
                string serviceInfo = "游戏中遇到任何问题请联系客服" + serviceInfos[0].Type + ":" + serviceInfos[0].Number;
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow(serviceInfo, null, PopOptionType.Single);
            }
        }
    }
}
