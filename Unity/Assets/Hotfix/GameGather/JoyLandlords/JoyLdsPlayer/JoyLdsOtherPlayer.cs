using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class JoyLdsOtherPlayer : JoyLdsBasePlayer
    {
        public GameObject mRediduePaiNumLocationGo;//剩余牌数父节点对象
        public GameObject mFigureInfoParentGo;//人物信息对象父节点
        public GameObject mTimerLocation;//计时器的父节点坐标

        //计时器闹钟对象
        private AlarmClock _alarmClock;
        private AlarmClock AarmClock
        {
            get
            {
                if (_alarmClock == null)
                {
                    _alarmClock=AlarmClockFactory.Create(mTimerLocation.transform);
                }
                return _alarmClock;
            }
        }
        //剩余牌数对象
        private Text _rediduePaiNumText;
        public Text RediduePaiNumText
        {
            get
            {
                if (_rediduePaiNumText == null)
                {
                  GameObject go=ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel, "RediduePaiNum") as GameObject;
                  GameObject  redidueNumGo=GameObject.Instantiate(go, mRediduePaiNumLocationGo.transform);
                  redidueNumGo.transform.localPosition = Vector3.zero;
                   _rediduePaiNumText = redidueNumGo.transform.GetChild(0).gameObject.GetComponent<Text>();
                }
                return _rediduePaiNumText;
            }
        }
        public override void Init()
        {
            base.Init();

            UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().SetFigureAppearanceParent(mFigureImage.gameObject);

           
        }
        public override void AnewDeal(params object[] objs)
        {
            mRediduePaiNumLocationGo.SetActive(false);
        }
        //玩家出牌
        public override void PlayCard(RepeatedField<int> cards, RepeatedField<int> hands)
        {
            base.PlayCard(cards, hands);
            ShowRediduePaiNumText(hands[0]);
        }

        //玩家可以叫地主
        public override void CanCallLanlord(int residueTime)
        {
            base.CanCallLanlord(residueTime);
            ShowAlarmResidue(residueTime);
        }
        //玩家可以出牌
        public override void CanPlayCard(bool isFirst, int residueTime)
        {
            base.CanPlayCard(isFirst, residueTime);
            ShowAlarmResidue(residueTime);
        }
        //玩家可以抢地主
        public override void CanRobLanlord(int residueTime)
        {
            base.CanRobLanlord(residueTime);
            ShowAlarmResidue(residueTime);
        }

        //显示计时器显示剩余时间
        public void ShowAlarmResidue(int residue)
        {
            HideOperationInfoGo();
            AarmClock.Show(residue);
        }
        //直接隐藏闹钟
        public override void HideAlarmClock()
        {
            AarmClock.Hide();
        }
        //成为地主事件
        public override void TurnLandlord(RepeatedField<int> hands, RepeatedField<int> threeHand)
        {
            base.TurnLandlord(hands,threeHand);
            ShowRediduePaiNumText(hands[0]);
        }
        public override async void Deal(params object[] objs)
        {
            base.Deal(objs);
            mRediduePaiNumLocationGo.SetActive(true);
            RepeatedField<int> cards = objs[0] as RepeatedField<int>;
            for (int i = 0; i < cards.Count; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(200);
                ShowRediduePaiNumText(i + 1);
            }
        }

        public void ShowRediduePaiNumText(int num)
        {
            RediduePaiNumText.text = num.ToString();
        }
        public override void Show()
        {
            base.Show();
           
        }
        public override void Hide()
        {
            base.Hide();
            
        }
    }
}
