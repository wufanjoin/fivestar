using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class FrienCircleRequestPlayerItemAwakeSystem : AwakeSystem<FrienCircleRequestPlayerItem, GameObject, User>
    {
        public override void Awake(FrienCircleRequestPlayerItem self, GameObject go, User data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FrienCircleRequestPlayerItem : BaseItem<User>
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Text mNameText;
        private Text mIdText;
        private Button mTurnBtn;
        private Button mConsentBtn;
        public override void Awake(GameObject go, User data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mTurnBtn = rc.Get<GameObject>("TurnBtn").GetComponent<Button>();
            mConsentBtn = rc.Get<GameObject>("ConsentBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion

        public async void InitPanel()
        {
            mHeadImage.sprite = await mData.GetHeadSprite();
            mNameText.text = mData.Name;
            mIdText.text ="ID:"+ mData.UserId;
            mTurnBtn.Add(TurnBtnEvnet);
            mConsentBtn.Add(ConsentBtnEvnet);
        }
        //拒绝
        public  void TurnBtnEvnet()
        {
            DispseApplyInfo(false);
        }
        //同意
        public  void ConsentBtnEvnet()
        {
            DispseApplyInfo(true);
        }

        public async void DispseApplyInfo(bool isconsent)
        {
            C2F_DisposeApplyInfo c2FDisposeApply = new C2F_DisposeApplyInfo();
            c2FDisposeApply.ApplyUserId = mData.UserId;
            c2FDisposeApply.FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId;
            c2FDisposeApply.IsConsent = isconsent;
            F2C_DisposeApplyInfo f2CDisposeApplyInfo = (F2C_DisposeApplyInfo)await SessionComponent.Instance.Call(c2FDisposeApply);
            if (!string.IsNullOrEmpty(f2CDisposeApplyInfo.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CDisposeApplyInfo.Message);
            }
            await FriendCirclePanelComponent.Ins.ManageView.RefreshViewInfo();//处理玩家信息后 刷新成员列表
            FriendCirclePanelComponent.Ins.ManageView.Hide();
            FriendCirclePanelComponent.Ins.RequestListView.InitRequestList();// 处理玩家信息后 刷新申请列表
        }
    }
}
