using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FriendCircleOptionLisView : BaseView
    {
        #region 脚本工具生成的代码
        private Toggle mRoomListToggle;
        private Button mMilitaryBtn;
        private Toggle mRankingToggle;
        private Toggle mManageToggle;
        private Toggle mApplyToggle;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mRoomListToggle = rc.Get<GameObject>("RoomListToggle").GetComponent<Toggle>();
            mMilitaryBtn = rc.Get<GameObject>("MilitaryBtn").GetComponent<Button>();
            mRankingToggle = rc.Get<GameObject>("RankingToggle").GetComponent<Toggle>();
            mManageToggle = rc.Get<GameObject>("ManageToggle").GetComponent<Toggle>();
            mApplyToggle = rc.Get<GameObject>("ApplyToggle").GetComponent<Toggle>();
            InitPanel();
        }
        #endregion

        public FriendCirclePanelComponent _CirclePanelComponent;
        public void InitPanel()
        {
            _CirclePanelComponent = UIComponent.GetUiView<FriendCirclePanelComponent>();
            mRoomListToggle.Add(RoomListToggleEvent);
            mMilitaryBtn.Add(MilitaryToggleEvent,false);
            mRankingToggle.Add(RankingToggleEvent);
            mManageToggle.Add(ManageToggleEvent);
            mApplyToggle.Add(ApplyToggleEvent);
        }


        public void RoomListToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _CirclePanelComponent.HideManageView();
                _CirclePanelComponent.RoomListView.Show();
            }
        }
        public void MilitaryToggleEvent()
        {
            UIComponent.GetUiView<MilitaryPanelComponent>().ShowMilitary(0, FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId);
        }
        public void RankingToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _CirclePanelComponent.HideManageView();
                _CirclePanelComponent.RankingListView.Show();
            }
        }
        public void ManageToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _CirclePanelComponent.HideManageView();
                _CirclePanelComponent.ManageView.Show();
            }
        }
        public void ApplyToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _CirclePanelComponent.HideManageView();
                _CirclePanelComponent.RequestListView.Show();
            }
        }

        public void ShowStateSet()
        {
            if (FrienCircleComponet.Ins.CuurSelectFriendsCircle.ManageUserIds.Contains(UserComponent.Ins.pUserId))
            {
                Show();
                mRoomListToggle.isOn = true;
            }
            else
            {
                Hide();
            }
          
        }
    }
}
