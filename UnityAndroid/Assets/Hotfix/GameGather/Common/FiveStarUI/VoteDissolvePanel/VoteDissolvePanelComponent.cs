using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

namespace ETHotfix
{
    [UIComponent(UIType.VoteDissolvePanel)]
    public class VoteDissolvePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mConsentBtn;
        private Button mTurnBtn;
        private Text mTimeText;
        private Text mInitiatorText;
        private GameObject mVoteConditionItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mConsentBtn = rc.Get<GameObject>("ConsentBtn").GetComponent<Button>();
            mTurnBtn = rc.Get<GameObject>("TurnBtn").GetComponent<Button>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mInitiatorText = rc.Get<GameObject>("InitiatorText").GetComponent<Text>();
            mVoteConditionItemGo = rc.Get<GameObject>("VoteConditionItemGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mConsentBtn.Add(ConsentBtnEvent);
            mTurnBtn.Add(TurnBtnEvent);
           
        }

        public override void MaskClickEvent()
        {
        }
        public void ShowVoteDissolve(Actor_VoteDissolveRoomResult result)
        {
            Show();
            CardFiveStarPlayer cardFiveStarPlayer= CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(result.SponsorUserId);
            mInitiatorText.text = $"玩家“{cardFiveStarPlayer._user.Name}”申请解散房间，是否同意？";
            List<VoteConditionItem>  voteConditionItems=mVoteConditionItemGo.transform.parent.CreatorChildAndAddItem<VoteConditionItem, CardFiveStarPlayer>(CardFiveStarRoom.Ins._ServerSeatIndexInPlayer.Values.ToArray());
            SetPlayerVoteState(voteConditionItems, result);//设置玩家投票状态UI
            SetOptionBtnState(result);//设置显示投票按钮的状态
            VoteResultDispose(result); //投票结果处理
        }
        //投票结果处理
        private void VoteResultDispose(Actor_VoteDissolveRoomResult result)
        {

            if (result.Result == VoteResultType.Consent)
            {
                Hide();//隐藏界面
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow($"房间已解散", RoomDissolve, PopOptionType.Single);
               

            }
            else if (result.Result == VoteResultType.NoConsent)
            {
                Hide();//隐藏界面
                for (int i = 0; i < result.VoteInfos.Count; i++)
                {
                    if (!result.VoteInfos[i].IsConsent)//如果自己已经投票 就隐藏调
                    {
                        CardFiveStarPlayer noconsentPlayer = CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(result.VoteInfos[i].UserId);
                        UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow($"玩家{noconsentPlayer._user.Name}不同意解散房间", null, PopOptionType.Single);
                    }
                }
            }
        }
        //房间成功解散事件
        public void RoomDissolve(bool confirm)
        {
            if (CardFiveStarRoom.Ins._CuurRoomOffice > 1|| CardFiveStarRoom.Ins._RoomState == RoomStateType.ReadyIn)
            {
                UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().Show();//显示总结算
            }
            else
            {
                Game.Scene.GetComponent<ToyGameComponent>().EndGame();//直接结束游戏
            }
        }
        //设置显示投票按钮的状态
        private void SetOptionBtnState(Actor_VoteDissolveRoomResult result)
        {
            mConsentBtn.gameObject.SetActive(true);//默认显示两个 同意和拒绝按钮
            mTurnBtn.gameObject.SetActive(true);
            for (int i = 0; i < result.VoteInfos.Count; i++)
            {
                if (result.VoteInfos[i].UserId == UserComponent.Ins.pUserId)//如果自己已经投票 就隐藏调
                {
                    mConsentBtn.gameObject.SetActive(false);
                    mTurnBtn.gameObject.SetActive(false);
                }
            }
        }
        //设置玩家投票状态UI
        private void SetPlayerVoteState(List<VoteConditionItem> voteConditionItems, Actor_VoteDissolveRoomResult result)
        {
            //设置玩家的投票状态
            for (int i = 0; i < voteConditionItems.Count; i++)
            {
                voteConditionItems[i].SetVoteState(VoteResultType.BeingVote);//默认设置我考虑中
                for (int j = 0; j < result.VoteInfos.Count; j++)
                {
                    if (result.VoteInfos[j].UserId == voteConditionItems[i].mData._user.UserId)//如果有投票情况就设置为实际情况
                    {
                        if (result.VoteInfos[j].IsConsent)
                        {
                            voteConditionItems[i].SetVoteState(VoteResultType.Consent);
                        }
                        else
                        {
                            voteConditionItems[i].SetVoteState(VoteResultType.NoConsent);
                        }
                        break;
                    }
                }
            }
        }
        public override async void OnShow()
        {
            base.OnShow();
            int residueTime = FiveStarOverTime.DissolveOverTime;
            while (gameObject.activeInHierarchy)
            {
                mTimeText.text = $"({residueTime--}秒以后默认同意)";
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);

            }
        }

        public void ConsentBtnEvent()
        {
          SessionComponent.Instance.Send(new Actor_VoteDissolveSelect(){IsConsent = true});  
        }
        public void TurnBtnEvent()
        {
            SessionComponent.Instance.Send(new Actor_VoteDissolveSelect() { IsConsent = false });
        }
    }
}
