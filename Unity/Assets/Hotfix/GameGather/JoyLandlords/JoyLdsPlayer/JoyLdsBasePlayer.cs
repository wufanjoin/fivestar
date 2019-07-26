using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class JoyLdsBasePlayer:Entity
    {
        public GameObject gameObject;//所对应对象
        public User mUser;//人物信息
        public Text mNameText;//人物名字
        public Text mBeansNumText;//豆子数
        public Image mFigureImage;//人物形象
        public Text mOperationResulText;//出牌结果文字
        public GameObject mPlayShowCardGo;//出牌显示的父对象
        public GameObject mAlarmClockGo;//计时闹钟父节点
        public Text mAlarmClockText;//计时闹钟
        public GameObject mLandlordIcon;//地主图标
        public GameObject mLandlordIconLocation;//地主图标的父节点坐标
        public GameObject mPrepareGo;//已经准备的对象
        public int pSeatServerIndex;//坐位在服务器的索引
        public int pSeatClinetIndex;//坐位在客户端显示的索引

        private List<JoyLandlordsCard> mShowPlayCards = new List<JoyLandlordsCard>();

        public virtual void Init()
        {
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.Deal, Deal);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.AnewDeal, AnewDeal);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.CanAddTwice, CanAddTwice);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.EnrerRoom, EnrerRoom);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.ConfirmCamp, ConfirmCamp);
            Log.Debug(gameObject.name);
        }
        protected virtual void OnShow()
        {

        }
        protected virtual void OnHide()
        {

        }
        //确定阵营
        public virtual void ConfirmCamp(params object[] objs)
        {
            HideOperationInfoGo();
        }
        //用户刚进入房间
        public virtual void EnrerRoom(params object[] objs)
        {
            HideOperationInfoGo();
            Hide();
        }
        //发牌
        public virtual async void Deal(params object[] objs)
        {
            mPrepareGo.SetActive(false);//隐藏准备状态
            HideOperationInfoGo();
        }
        //重新发牌
        public virtual void AnewDeal(params object[] objs)
        {

        }
        //可以加倍
        public virtual void CanAddTwice(params object[] objs)
        {

        }
        public virtual void Show()
        {
            mFigureImage.gameObject.SetActive(true);
            gameObject.SetActive(true);
            OnShow();
        }
        public virtual void Hide()
        {
            mFigureImage.gameObject.SetActive(false);
            gameObject.SetActive(false);
            OnHide();
        }

        //玩家加倍
        public virtual void AddTwice(bool result)
        {
            if (result)
            {
                ShowOperationResulText("加倍");
            }
            else
            {
                ShowOperationResulText("不加倍");
            }
        }


        //玩家叫地主
        public virtual void CallLanlord(bool result)
        {
            if (result)
            {
                ShowOperationResulText("叫地主");
            }
            else
            {
                ShowOperationResulText("不叫");
            }
        }

        //玩家可以叫地主
        public virtual void CanCallLanlord(int residueTime)
        {
        }
        //玩家可以出牌
        public virtual void CanPlayCard(bool isFirst, int residueTime)
        {
        }
        //玩家可以抢地主
        public virtual void CanRobLanlord(int residueTime)
        {
        }
        //不出
        public virtual void DontPlay()
        {
            ShowOperationResulText("不出");
        }
        //玩家出牌
        public virtual void PlayCard(RepeatedField<int> cards, RepeatedField<int> hands)
        {
            ShowPlayCards(cards);
        }
        //玩家准备
        public virtual void Prepare()
        {
            mPrepareGo.SetActive(false);
        }
        //抢地主
        public virtual void RobLanlord(bool result)
        {
            if (result)
            {
                ShowOperationResulText("抢地主");
            }
            else
            {
                ShowOperationResulText("不抢");
            }
            
        }
        //成为地主
        private static GameObject LandlordIconPrefabGo;
        public virtual void TurnLandlord(RepeatedField<int> hands, RepeatedField<int> threeHand)
        {
            //显示地主图标
            if (LandlordIconPrefabGo == null)
            {
                LandlordIconPrefabGo = ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel, "LandlordIcon") as GameObject;
            }
            if (mLandlordIcon == null)
            {
                mLandlordIcon = GameObject.Instantiate(LandlordIconPrefabGo, mLandlordIconLocation.transform);
            }
            mLandlordIcon.SetActive(true);
        }
        //隐藏地主图标
        public void HideLandlordIcon()
        {
            mLandlordIcon?.SetActive(false);
        }
        //显示文字操作结果
        public virtual void ShowOperationResulText(string content)
        {
            HideOperationInfoGo();
            mOperationResulText.gameObject.SetActive(true);
            mOperationResulText.text = content;
            HideAlarmClock();
        }
        //显示出牌结果
        public virtual void ShowPlayCards(RepeatedField<int> cards)
        {
            HideOperationInfoGo();
            DestroyPlayCards();
            mPlayShowCardGo.SetActive(true);
            for (int i = 0; i < cards.Count; i++)
            {
                 mShowPlayCards.Add(JoyLandlordsCardPool.Ins.Create(JoyLandlordsCardPrefabType.Mid, cards[i], mPlayShowCardGo.transform));
            }
        }

        public void DestroyPlayCards()
        {
            foreach (var playCard in mShowPlayCards)
            {
                playCard.Destroy();
            }
            mShowPlayCards.Clear();
        }
        //隐藏所有操作结果
        public void HideOperationInfoGo()
        {
            HideAlarmClock();
            mPlayShowCardGo.SetActive(false);
            mOperationResulText.gameObject.SetActive(false);
        }
        public virtual void HideAlarmClock()
        {
            //怎么隐藏计时器  由子类实现
        }

        //还原一下
        public virtual void RestoreUI()
        {
            mNameText.text = mUser.Name;
            mBeansNumText.text = mUser.Beans.ToString();
            HideLandlordIcon();//隐藏地主图标
            HideOperationInfoGo();//隐藏所有操作结果
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            if (mUser.Sex==1)//女
            {
                mFigureImage.sprite=resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "womanAppearance") as Sprite;
            }
            else//男
            {
                mFigureImage.sprite = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "manAppearance") as Sprite;
            }
            mFigureImage.SetNativeSize();
        }


    }
}
