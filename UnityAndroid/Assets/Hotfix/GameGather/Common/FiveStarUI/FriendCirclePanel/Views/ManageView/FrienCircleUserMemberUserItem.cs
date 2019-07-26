using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrienCircleUserMemberUserItemAwakeSystem : AwakeSystem<FrienCircleUserMemberUserItem, GameObject, User>
    {
        public override void Awake(FrienCircleUserMemberUserItem self, GameObject go, User data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FrienCircleUserMemberUserItem : BaseItem<User>
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Text mNameText;
        private Text mIdText;
        private Button mKickOutBtn;
        private Button mManageSetBtn;
        public override void Awake(GameObject go, User data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mKickOutBtn = rc.Get<GameObject>("KickOutBtn").GetComponent<Button>();
            mManageSetBtn = rc.Get<GameObject>("ManageSetBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion

        private bool _CuurIsManage = false;
        public async void InitPanel()
        {
            if (mData.UserId == UserComponent.Ins.pUserId)//不显示自己
            {
                Hide();
                isNoShow = true;
                return;
            }
            mHeadImage.sprite =await mData.GetHeadSprite();
            mNameText.text = mData.Name;
            mIdText.text ="ID:"+ mData.UserId;
            mKickOutBtn.Add(KickOutBtnEvent);
            mManageSetBtn.Add(ManageSetBtnEvent);
            _CuurIsManage = FrienCircleComponet.Ins.CuurSelectFriendsCircle.ManageUserIds.Contains(mData.UserId);
            ManageSetBtnStatuSet();
        }

        public override void Show()
        {
            if (isNoShow)//不显示自己
            {
                Hide();
                return;
            }
            base.Show();
        }

        public void ManageSetBtnStatuSet()
        {
            mManageSetBtn.gameObject.SetActive(UserComponent.Ins.pUserId == FrienCircleComponet.Ins.CreateUser.UserId); 
            if (_CuurIsManage)
            {
                mManageSetBtn.SetText("取消管理");
            }
            else
            {
                mManageSetBtn.SetText("设为管理");
            }
        }

        private bool isNoShow = false;//是否不显示
        public async void KickOutBtnEvent()
        {
            F2C_KickOutFriendsCircle  f2CKickOutFriendsCircle= (F2C_KickOutFriendsCircle)await SessionComponent.Instance.Call(new C2F_KickOutFriendsCircle()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId,
                OperateUserId = mData.UserId
            });
            if (!string.IsNullOrEmpty(f2CKickOutFriendsCircle.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CKickOutFriendsCircle.Message);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("成功踢出");
                Hide();
                isNoShow = true;
            }
        }

        public async void ManageSetBtnEvent()
        {
            F2C_ManageJurisdictionOperate f2CManageJurisdiction = (F2C_ManageJurisdictionOperate)await SessionComponent.Instance.Call(new C2F_ManageJurisdictionOperate()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId,
                OperateUserId = mData.UserId,
                IsSetManage=!_CuurIsManage
            });
            if (!string.IsNullOrEmpty(f2CManageJurisdiction.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CManageJurisdiction.Message);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("操作成功");
                _CuurIsManage = !_CuurIsManage;
                ManageSetBtnStatuSet();
            }
        }
    }
}
