using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrienCircleRankingPlayerInfoItemAwakeSystem : AwakeSystem<FrienCircleRankingPlayerInfoItem, GameObject, RanKingPlayerInfo>
    {
        public override void Awake(FrienCircleRankingPlayerInfoItem self, GameObject go, RanKingPlayerInfo data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FrienCircleRankingPlayerInfoItem : BaseItem<RanKingPlayerInfo>
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Text mOfficeNumberText;
        private Text mTotalScoreText;
        private Button mExamineMilitaryBtn;
        private Text mNameText;
        private Text mIdText;
        public override void Awake(GameObject go, RanKingPlayerInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mOfficeNumberText = rc.Get<GameObject>("OfficeNumberText").GetComponent<Text>();
            mTotalScoreText = rc.Get<GameObject>("TotalScoreText").GetComponent<Text>();
            mExamineMilitaryBtn = rc.Get<GameObject>("ExamineMilitaryBtn").GetComponent<Button>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            User user=await mData.GetUser();
            mHeadImage.sprite = await user.GetHeadSprite();
            mNameText.text = user.Name;
            mIdText.text ="ID:"+ user.UserId;
            mOfficeNumberText.text = $"总局数:<color=#EC6D36FF>{mData.TotalNumber}</color>";
            mTotalScoreText.text= $"总积分:<color=#FFFF00FF>{mData.TotalScore}</color>";
            mExamineMilitaryBtn.Add(ExamineMilitaryBtnEvent);
        }

        public void ExamineMilitaryBtnEvent()
        {
           // Log.Debug("DOTO 显示战绩待实现");
            UIComponent.GetUiView<MilitaryPanelComponent>().ShowMilitary(mData.UserId, FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId);
        }
    }
}
