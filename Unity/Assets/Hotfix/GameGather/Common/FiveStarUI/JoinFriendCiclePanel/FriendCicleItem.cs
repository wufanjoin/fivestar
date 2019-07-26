using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FriendCicleItemAwakeSystem : AwakeSystem<FriendCicleItem, GameObject, FriendsCircleAndCreateUser>
    {
        public override void Awake(FriendCicleItem self, GameObject go, FriendsCircleAndCreateUser data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FriendCicleItem : BaseItem<FriendsCircleAndCreateUser>
    {
        #region 脚本工具生成的代码
        private Text mIdAndNameText;
        private Text mOriginatorNameText;
        private Text mAnnouncementText;
        private Button mApplyJoinBtn;
        private Image mHeadImage;
        public override void Awake(GameObject go, FriendsCircleAndCreateUser data, string uiType)
        {
            base.Awake(go, data, uiType);
            mIdAndNameText = rc.Get<GameObject>("IdAndNameText").GetComponent<Text>();
            mOriginatorNameText = rc.Get<GameObject>("OriginatorNameText").GetComponent<Text>();
            mAnnouncementText = rc.Get<GameObject>("AnnouncementText").GetComponent<Text>();
            mApplyJoinBtn = rc.Get<GameObject>("ApplyJoinBtn").GetComponent<Button>();
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            mIdAndNameText.text = mData.FriendsCircleInfo.Name + "\nID:" + mData.FriendsCircleInfo.FriendsCircleId;
            mOriginatorNameText.text = mData.CreateUser.Name;
            mAnnouncementText.text = mData.FriendsCircleInfo.Announcement;
            mHeadImage.sprite =await mData.CreateUser.GetHeadSprite();
            mApplyJoinBtn.Add(ApplyJoinBtnEvent);
            if (UIComponent.GetUiView<JoinFriendCiclePanelComponent>().CuurShowType == JoinFrienPanelShowType.Join)
            {
                mApplyJoinBtn.SetText("申请加入");
            }
            else
            {
                mApplyJoinBtn.SetText("切换亲友圈");
            }
        }

        public async void ApplyJoinBtnEvent()
        {
            if (UIComponent.GetUiView<JoinFriendCiclePanelComponent>().CuurShowType == JoinFrienPanelShowType.Join)
            {
                if (FrienCircleComponet.Ins.AlreadyJoinFrienCircleIds.Contains(mData.FriendsCircleInfo.FriendsCircleId))
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("已经在这个亲友圈中");
                    return;
                }
                F2C_ApplyJoinFriendsCircle f2CJoinFriendsCircle =(F2C_ApplyJoinFriendsCircle)await SessionComponent.Instance.Call(new C2F_ApplyJoinFriendsCircle()
                {
                    FriendsCrircleId= mData.FriendsCircleInfo.FriendsCircleId
                });
                if (string.IsNullOrEmpty(f2CJoinFriendsCircle.Message))
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("申请成功");
                }
                else
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CJoinFriendsCircle.Message);
                }
            }
            else
            {
                FrienCircleComponet.Ins.CutFrienCircle(mData.FriendsCircleInfo);//切换亲友圈
                UIComponent.GetUiView<JoinFriendCiclePanelComponent>().Hide();
            }
        }
    }
}
