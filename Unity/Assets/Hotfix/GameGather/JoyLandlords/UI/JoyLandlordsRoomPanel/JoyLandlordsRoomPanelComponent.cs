using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.JoyLandlordsRoomPanel)]
    public class JoyLandlordsRoomPanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private Text mRoomBriefText;
        private Button mPurhchaseBtn;
        private Button mChatBtn;
        private Text mMultipleText;
        private Button mLayOneCardsStartBtn;
        private Button mStartGameBtn;
        private GameObject mFigureAppearanceParentGo;
        private GameObject mAxialCardsGo;
        public GameObject mSelfHandCardGroupGo;
        private GameObject mOperationBtnParentGo;
        public GameObject mPlayerLocationSelfGo;
        public GameObject mWaitAddTwiceGo;
        private Text mMathInText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mRoomBriefText = rc.Get<GameObject>("RoomBriefText").GetComponent<Text>();
            mMathInText = rc.Get<GameObject>("MathInText").GetComponent<Text>();
            mPurhchaseBtn = rc.Get<GameObject>("PurhchaseBtn").GetComponent<Button>();
            mChatBtn = rc.Get<GameObject>("ChatBtn").GetComponent<Button>();
            mMultipleText = rc.Get<GameObject>("MultipleText").GetComponent<Text>();
            mLayOneCardsStartBtn = rc.Get<GameObject>("LayOneCardsStartBtn").GetComponent<Button>();
            mStartGameBtn = rc.Get<GameObject>("StartGameBtn").GetComponent<Button>();
            mAxialCardsGo = rc.Get<GameObject>("AxialCardsGo");
            mFigureAppearanceParentGo= rc.Get<GameObject>("FigureAppearanceParentGo");
            mSelfHandCardGroupGo = rc.Get<GameObject>("SelfHandCardGroupGo");
            mPlayerLocationSelfGo = rc.Get<GameObject>("PlayerLocationSelfGo");
            mOperationBtnParentGo = rc.Get<GameObject>("OperationBtnParentGo");
            mWaitAddTwiceGo = rc.Get<GameObject>("WaitAddTwiceGo");
            InitPanel();
        }
        #endregion

        public void InitEnterRoomUI()
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.EnrerRoom);
            ShowUserPlayerInfo();
            JoyLandlordsOperationControl.Ins.ShowBtnType(JoyLandlordsOperationType.StartGame);
            mMathInText.gameObject.SetActive(false);
            DetroyLandlordThreeCard();
           
        }
        public void InitPanel()
        {
            this.InitChildView(UIType.JoyLdsRoomUpFramePanel);
            JoyLandlordsOperationControl.Ins.Init(mOperationBtnParentGo, mStartGameBtn);
        }

        public void SetFigureAppearanceParent(GameObject figureAppearance)
        {
            figureAppearance.transform.SetParent(mFigureAppearanceParentGo.transform);
        }
        //匹配成功
        public void MathSucceed()
        {
            mMathInText.gameObject.SetActive(false);
            mathTime = 0;
        }
        //开始匹配
        int mathTime = 31;

        private bool beTimeIn = false;
        public async void StartMath()
        {
            mMathInText.gameObject.SetActive(true);
            JoyLandlordsOperationControl.Ins.HideBtnAll();
            if (beTimeIn)
            {
                mathTime = 31;
                mMathInText.text = "正在匹配小伙伴...  " + 31;
                return;
            }
            beTimeIn = true;
            while (--mathTime>=0)
            {     
                mMathInText.text = "正在匹配小伙伴...  " + mathTime;
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
             }
            beTimeIn = false;
        }
        //显示地主的三张牌
        private List<JoyLandlordsCard> landlordThreeCard = new List<JoyLandlordsCard>();
        public void ShowLandlordThreeCard(RepeatedField<int> threeCard)
        {
            DetroyLandlordThreeCard();
            foreach (var card in threeCard)
            {
                landlordThreeCard.Add(JoyLandlordsCardPool.Ins.Create(JoyLandlordsCardPrefabType.Small, card, mAxialCardsGo.transform));
            }
        }

        //销毁地主的三张牌
        public void DetroyLandlordThreeCard()
        {
            foreach (var card in landlordThreeCard)
            {
                card.Destroy();
            }
            landlordThreeCard.Clear();
        }
        //显示或者隐藏等待加倍
        public void ShowOrHideWaitAddTwice(bool active)
        {
            mWaitAddTwiceGo.SetActive(active);
        }
        public JoyLdsUserPlayer ShowUserPlayerInfo(int seatServerIndex = 0)
        {

            JoyLdsUserPlayer userPlayer = JoyLdsPlayerFactory.CreateUserPlayer(Game.Scene.GetComponent<UserComponent>().pSelfUser, mPlayerLocationSelfGo.GetComponent<ReferenceCollector>(), seatServerIndex);
            userPlayer.RestoreUI();
            return userPlayer;
        }
        public JoyLdsOtherPlayer ShowOtherPlayer(User user, int seatServerIndex)
        {

            JoyLdsOtherPlayer player = JoyLdsPlayerFactory.CreateOtherPlayer(user, seatServerIndex);
            player.RestoreUI();
            return player;
        }
    }
}
