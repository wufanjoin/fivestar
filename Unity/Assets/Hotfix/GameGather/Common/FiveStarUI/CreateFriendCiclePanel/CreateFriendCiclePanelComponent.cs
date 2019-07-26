using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.CreateFriendCiclePanel)]
    public class CreateFriendCiclePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Button mConfirmCreateBtn;
        private Button mSelectWanFaBtn;
        private Text mWanFaText;
        private InputField mFriendCicleNameInputField;
        private InputField mAnnouncementInputField;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mConfirmCreateBtn = rc.Get<GameObject>("ConfirmCreateBtn").GetComponent<Button>();
            mSelectWanFaBtn = rc.Get<GameObject>("SelectWanFaBtn").GetComponent<Button>();
            mWanFaText = rc.Get<GameObject>("WanFaText").GetComponent<Text>();
            mFriendCicleNameInputField = rc.Get<GameObject>("FriendCicleNameInputField").GetComponent<InputField>();
            mAnnouncementInputField = rc.Get<GameObject>("AnnouncementInputField").GetComponent<InputField>();
            InitPanel();
        }
        #endregion

        private RepeatedField<int> _CurrSelectWanFaConfigs;//当前选择的玩法
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            AlterWanfaFinshCall(UIComponent.GetUiView<CreateRoomPanelComponent>().GetDefaultConfigs());
            mSelectWanFaBtn.Add(SelectWanFaBtnEvent);
            mConfirmCreateBtn.Add(ConfirmCreateBtnEvent);
        }

        public override async void OnShow()
        {
            base.OnShow();
            mFriendCicleNameInputField.text = "";
            mAnnouncementInputField.text = "";
        }

        public async void ConfirmCreateBtnEvent()
        {
            C2F_CreatorFriendCircle   c2FCreatorFriendCircle=new C2F_CreatorFriendCircle();
            c2FCreatorFriendCircle.Name = mFriendCicleNameInputField.text;
            c2FCreatorFriendCircle.Announcement = mAnnouncementInputField.text;
            c2FCreatorFriendCircle.WanFaCofigs = _CurrSelectWanFaConfigs;
            c2FCreatorFriendCircle.ToyGameId = ToyGameId.CardFiveStar;
            if (string.IsNullOrEmpty(c2FCreatorFriendCircle.Name))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("亲友圈名字不能为空");
                return;
            }
            if (_CurrSelectWanFaConfigs==null||!RoomConfigIntended.IntendedRoomConfigParameter(c2FCreatorFriendCircle.WanFaCofigs, c2FCreatorFriendCircle.ToyGameId))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("玩法配置出错请重新选择");
                return;
            }

            F2C_CreatorFriendCircle f2CCreatorFriend=(F2C_CreatorFriendCircle)await SessionComponent.Instance.Call(c2FCreatorFriendCircle);
            if (string.IsNullOrEmpty(f2CCreatorFriend.Message))
            {
                 FrienCircleComponet.Ins.SucceedCreatorFriendsCircle(f2CCreatorFriend.FriendsCircle);
                Hide();
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CCreatorFriend.Message);
            }
        }

        public void SelectWanFaBtnEvent()
        {
            UIComponent.GetUiView<CreateRoomPanelComponent>().ShowCraterRoomPanel(ShowCraterRoomPanelType.AlterWanFa, AlterWanfaFinshCall);
        }

        public void AlterWanfaFinshCall(RepeatedField<int> configs)
        {
            _CurrSelectWanFaConfigs = configs;
            FiveStarRoomConfig fiveStarRoomConfig = FiveStarRoomConfigFactory.Create(_CurrSelectWanFaConfigs);
            mWanFaText.text = fiveStarRoomConfig.GetWanFaDesc(false);
            fiveStarRoomConfig.Dispose();
        }
    }
}
