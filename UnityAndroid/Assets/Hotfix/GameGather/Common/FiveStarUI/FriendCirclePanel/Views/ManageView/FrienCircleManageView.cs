using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FrienCircleManageView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mAlterAnnouncementBtn;
        private Button mAlteWanFaBtn;
        private GameObject mMemberItemGo;
        private Button mSearchBtn;
        private InputField mSearchInputField;
        private Toggle mRecemmendStatuToggle;
        private GameObject mAlterAnnouncementViewGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mAlterAnnouncementBtn = rc.Get<GameObject>("AlterAnnouncementBtn").GetComponent<Button>();
            mAlteWanFaBtn = rc.Get<GameObject>("AlteWanFaBtn").GetComponent<Button>();
            mMemberItemGo = rc.Get<GameObject>("MemberItemGo");
            mSearchBtn = rc.Get<GameObject>("SearchBtn").GetComponent<Button>();
            mSearchInputField = rc.Get<GameObject>("SearchInputField").GetComponent<InputField>();
            mRecemmendStatuToggle = rc.Get<GameObject>("RecemmendStatuToggle").GetComponent<Toggle>();
            mAlterAnnouncementViewGo = rc.Get<GameObject>("AlterAnnouncementViewGo");
            InitPanel();
        }
        #endregion

        private FrienCirleAlterAnnouncementView _alterAnnouncementView;
        public void InitPanel()
        {
            _alterAnnouncementView = mAlterAnnouncementViewGo.AddItem<FrienCirleAlterAnnouncementView>();
            mAlterAnnouncementBtn.Add(AlterAnnouncementBtnEvent);
            mAlteWanFaBtn.Add(AlteWanFaBtnEvent);
            mSearchBtn.Add(SearchBtnEvent);
        }
        public async void RecemmendStatuToggleEvent(bool isOn)
        {
            F2C_AlterIsRecommend f2CAlterIsRecommend = (F2C_AlterIsRecommend)await SessionComponent.Instance.Call(
                new C2F_AlterIsRecommend() { FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId,IsRecommend = isOn });
            if (string.IsNullOrEmpty(f2CAlterIsRecommend.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("修改成功");

            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CAlterIsRecommend.Message);
            }
            
        }

        public override async void Show()
        {
            base.Show();
            if (_CurrUIInFriendsCircleId != FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId)//如果当前UI对应的亲友圈ID是实际的亲友圈id不符就刷新
            {
               await RefreshViewInfo();//刷新亲友圈信息
                return;
            }
            if (_MemberUserList == null)
            {
                return;
            }
            for (int i = 0; i < _MemberUserList.Count; i++)
            {
                _MemberUserList[i].Show();
            }
        }
        //搜索成员
        public void SearchBtnEvent()
        {
            for (int i = 0; i < _MemberUserList.Count; i++)
            {
                if (_MemberUserList[i].mData.Name.Contains(mSearchInputField.text) || _MemberUserList[i].mData.UserId
                        .ToString().Contains(mSearchInputField.text))
                {
                    _MemberUserList[i].Show();
                }
                else
                {
                    _MemberUserList[i].Hide();
                }
            }

        }

        private int _CurrUIInFriendsCircleId = 0;//当前界面显示的信息对应的亲友圈ID
        public async Task RefreshViewInfo()
        {
            mRecemmendStatuToggle.onValueChanged.RemoveAllListeners();
            mRecemmendStatuToggle.isOn = FrienCircleComponet.Ins.CuurSelectFriendsCircle.IsRecommend;
            mRecemmendStatuToggle.Add(RecemmendStatuToggleEvent);
            await InitMemberList();
            _CurrUIInFriendsCircleId=FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId;
        }
        private List<FrienCircleUserMemberUserItem> _MemberUserList;
        private async Task InitMemberList()
        {
            F2C_GetMemberList f2CGetMemberList = (F2C_GetMemberList)await SessionComponent.Instance.Call(
                new C2F_GetMemberList() { FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId });
            RepeatedField<User> users = await UserComponent.Ins.GetUserInfo(f2CGetMemberList.MemberUserIdList);
            _MemberUserList = mMemberItemGo.transform.parent.CreatorChildAndAddItem<FrienCircleUserMemberUserItem, User>(users);
        }
        public void AlteWanFaBtnEvent()
        {
            UIComponent.GetUiView<CreateRoomPanelComponent>().ShowCraterRoomPanel(ShowCraterRoomPanelType.AlterWanFa, AlteWanFaCall);
        }

        public async void AlteWanFaCall(RepeatedField<int> config)
        {
            F2C_AlterWanFa f2CAlterWanFa = (F2C_AlterWanFa)await SessionComponent.Instance.Call(new C2F_AlterWanFa()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId,
                ToyGameId = ToyGameId.CardFiveStar,
                WanFaCofigs = config
            });
            if (string.IsNullOrEmpty(f2CAlterWanFa.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("修改成功");
                FrienCircleComponet.Ins.AlteWanFaCall(config);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CAlterWanFa.Message);
            }
        }
        public void AlterAnnouncementBtnEvent()
        {
            _alterAnnouncementView.Show();
        }
    }
}
