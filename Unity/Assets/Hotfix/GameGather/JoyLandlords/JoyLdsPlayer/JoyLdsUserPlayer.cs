using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    public class JoyLdsUserPlayer : JoyLdsBasePlayer
    {
        public GameObject mSlectHandFoulTextGo;//选牌犯规提升对象
        //public void SetSeatServerIndex(int seatServerIndex)
        //{
        //    pSeatServerIndex = seatServerIndex;
        //}
        //可以加倍
        public override void CanAddTwice(params object[] objs)
        {
            base.CanAddTwice(objs);
           ShowBtnType(JoyLandlordsOperationType.AddTwice);
        }

        public override void AddTwice(bool result)
        {
            base.AddTwice(result);
            UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().ShowOrHideWaitAddTwice(true);
        }
        //可以抢地主
        public override void CanCallLanlord(int residueTime)
        {
            base.CanCallLanlord(residueTime);
            ShowBtnType(JoyLandlordsOperationType.CallJoyLandlords, residueTime);
        }

        //玩家可以出牌
        public override void CanPlayCard(bool isFirst, int residueTime)
        {
            if (isFirst)
            {
                ShowBtnType(JoyLandlordsOperationType.MustPlayCard, residueTime);
            }
            else
            {
                ShowBtnType(JoyLandlordsOperationType.PlayCard, residueTime);
            }
            
        }
        //玩家可以抢地主
        public override void CanRobLanlord(int residueTime)
        {
            ShowBtnType(JoyLandlordsOperationType.RobJoyLandlords, residueTime);
        }
        //显示选项按钮
        public void ShowBtnType(int joyLandlordsOperationType, int residueTime = 0)
        {
            HideOperationInfoGo();
            JoyLandlordsOperationControl.Ins.ShowBtnType(joyLandlordsOperationType, residueTime);
        }
        //隐藏计时器
        public override void HideAlarmClock()
        {
            mSlectHandFoulTextGo.SetActive(false);
            JoyLandlordsOperationControl.Ins.HideBtnAll();
        }
        //显示提示犯规
        public void ShowSlectHandFoulTextGo()
        {
            mSlectHandFoulTextGo.SetActive(true);
        }
        //成为地主事件
        public override void TurnLandlord(RepeatedField<int> hands, RepeatedField<int> threeHand)
        {
            base.TurnLandlord(hands, threeHand);
            JoyLandlordsHandComponent.Ins.GetThreeHandCard(hands, threeHand);
        }

        //玩家出牌
        public override void PlayCard(RepeatedField<int> cards, RepeatedField<int> hands)
        {
            base.PlayCard(cards, hands);
            JoyLandlordsHandComponent.Ins.RefrshHand(hands);
        }
    }
}
