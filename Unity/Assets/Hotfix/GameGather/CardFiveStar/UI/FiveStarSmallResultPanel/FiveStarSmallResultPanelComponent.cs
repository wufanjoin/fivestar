using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.FiveStarSmallResultPanel)]
    public class FiveStarSmallResultPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mGoOnBtn;
        private Button mShareBtn;
        private Button mReturnBtn;
        private Text mPlayerTitleText;
        private GameObject mHeJiGo;
        private GameObject mDeFenGo;
        private GameObject mTotalScoreGo;
        private GameObject mPlayerInfoItemGo;
        private GameObject mMaPaiPointGo;
        private GameObject mMaiMaGo;
        private Button mAnewPlayBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mGoOnBtn = rc.Get<GameObject>("GoOnBtn").GetComponent<Button>();
            mShareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mReturnBtn = rc.Get<GameObject>("ReturnBtn").GetComponent<Button>();
            mAnewPlayBtn = rc.Get<GameObject>("AnewPlayBtn").GetComponent<Button>();
            mPlayerTitleText = rc.Get<GameObject>("PlayerTitleText").GetComponent<Text>();
            mHeJiGo = rc.Get<GameObject>("HeJiGo");
            mDeFenGo = rc.Get<GameObject>("DeFenGo");
            mTotalScoreGo = rc.Get<GameObject>("TotalScoreGo");
            mPlayerInfoItemGo = rc.Get<GameObject>("PlayerInfoItemGo");
            mMaPaiPointGo = rc.Get<GameObject>("MaPaiPointGo");
            mMaiMaGo = rc.Get<GameObject>("MaiMaGo");
            InitPanel();
        }
        #endregion

        private List<SmallPlayerInfoItemGoItem> _smallPlayerInfos = new List<SmallPlayerInfoItemGoItem>();
        public void InitPanel()
        {
            InitSmallPlayerList();
            mShareBtn.Add(ShareBtnEvent);
            mGoOnBtn.Add(GoOnBtnEvent);
            mReturnBtn.Add(ReturnBtnEvent);
            mAnewPlayBtn.Add(AnewPlayBtnEvent);
        }

        public void AnewPlayBtnEvent()
        {
            FiveStarVideoRoom.Ins.AnewStart();
        }
        public void ShareBtnEvent()
        {
            SdkMgr.Ins.WeChatShareScreen();
        }

        public override void MaskClickEvent()
        {
         
        }
        public async void GoOnBtnEvent()
        {
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().PlayMusic(CardFiveRoomMusicType.RoomBg); //播放正常的音乐
            if (CardFiveStarRoom.Ins._RoomType==RoomType.Match)
            {
                bool isMatchSucceed = await UIComponent.GetUiView<MatchingRoomPanelComponent>().StartMatch(CardFiveStarRoom.Ins._RoomId);
                if(isMatchSucceed)
                {
                    Hide();
                    UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutMatchingUI();//没有错误就 切换匹配中的UI
                }
                else
                {
                    //ReturnBtnEvent();
                    return;
                }
                
            }
            else if (CardFiveStarRoom.Ins._RoomType == RoomType.RoomCard)
            {
                if (CardFiveStarRoom.Ins._CuurRoomOffice== CardFiveStarRoom.Ins._config.MaxJuShu)
                {
                    UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().Show();
                    Hide();
                    return;
                }
                UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutReadyInUI();
                UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().Hide();
                SessionComponent.Instance.Send(new Actor_FiveStar_PlayerReady());
                Hide(); 
            }
            CardFiveStarRoom.Ins.LittleRoundClearData();//清空小局应该清空的数据
        }
        public void ReturnBtnEvent()
        {
            Hide();
            if (CardFiveStarRoom.Ins == null)
            {
                FiveStarVideoRoom.Ins.OutPlay();
                return;
            }
            Game.Scene.GetComponent<ToyGameComponent>().EndGame();
        }
        //初始化小结算玩家信息
        public void InitSmallPlayerList()
        {
            Transform playerParentTrm = mPlayerInfoItemGo.transform.parent;
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(mPlayerInfoItemGo, playerParentTrm);
            }
            FiveStar_SmallPlayerResult result = new FiveStar_SmallPlayerResult();
            for (int i = 0; i < playerParentTrm.childCount; i++)
            {
                _smallPlayerInfos.Add(playerParentTrm.GetChild(i).AddItemIfHaveInit<SmallPlayerInfoItemGoItem, FiveStar_SmallPlayerResult>(result));
            }

        }

        public static int CurrRoomType = RoomType.Match;
        //显示小结算玩家信息
        public void ShowSmallResult(Actor_FiveStar_SmallResult smallResult)
        {
            Show();
            PlayMusic(smallResult);//根据情况播放音乐
            if (CardFiveStarRoom.Ins != null)//如果room的Ins等于空 说明在录像模式
            {
                CurrRoomType = CardFiveStarRoom.Ins._RoomType;
            }
            else
            {
                CurrRoomType= RoomType.RoomCard;//录像只能是房卡模式
            }
            if (CurrRoomType == RoomType.Match)
            {
                ShowMatchUI();
            }
            else if (CurrRoomType == RoomType.RoomCard)
            {
                ShowRoomCardUI();
            }
            for (int i = 0; i < smallResult.SmallPlayerResults.Count; i++)
            {
                _smallPlayerInfos[i].SetUI(smallResult.SmallPlayerResults[i]);
            }
            for (int i = smallResult.SmallPlayerResults.Count; i < _smallPlayerInfos.Count; i++)
            {
                _smallPlayerInfos[i].Hide();
            }
            ShowMaiMaPai(smallResult.MaiMaCard);
        }
        //播放音乐
        public void PlayMusic(Actor_FiveStar_SmallResult smallResult)
        {
         
            for (int i = 0; i < smallResult.SmallPlayerResults.Count; i++)
            {
                CardFiveStarPlayer cardFiveStarPlayer;
                if (CardFiveStarRoom.Ins != null)
                {
                    cardFiveStarPlayer = CardFiveStarRoom.Ins.GetFiveStarPlayer(smallResult.SmallPlayerResults[i].SeatIndex);
                }
                else
                {
                    cardFiveStarPlayer = FiveStarVideoRoom.Ins.GetFiveStarPlayer(smallResult.SmallPlayerResults[i].SeatIndex);
                }
                if (cardFiveStarPlayer._user.UserId== UserComponent.Ins.pUserId)
                {
                    if (smallResult.SmallPlayerResults[i].GetScore > 0)
                    {
                        UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().PlayMusic(CardFiveRoomMusicType.Win); //播放胜利背景音乐
                        return;
                    }
                }
            }
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().PlayMusic(CardFiveRoomMusicType.Lose); //播放失败背景音乐

        }
        //显示买马的牌
        private CardFiveStarCard _MaiMaCard;
        public void ShowMaiMaPai(int maCard)
        {
            if (maCard <=0)
            {
                mMaiMaGo.SetActive(false);
                return;
            }
            mMaiMaGo.SetActive(true);
            if (_MaiMaCard == null)
            {
                _MaiMaCard = CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, mMaPaiPointGo.transform, 0.5f);
                _MaiMaCard.LocalPositionZero();
            }
            _MaiMaCard.SetCardUI(maCard);
        }

        private void ShowMatchUI()
        {
            mHeJiGo.SetActive(false);
            mTotalScoreGo.SetActive(false);
            mDeFenGo.SetActive(true);

            mReturnBtn.gameObject.SetActive(true);
            mAnewPlayBtn.gameObject.SetActive(false);
            mShareBtn.gameObject.SetActive(true);
            mGoOnBtn.gameObject.SetActive(true);
            mPlayerTitleText.text = "玩家";
        }

        private void ShowRoomCardUI()
        {
            mHeJiGo.SetActive(true);
            mTotalScoreGo.SetActive(true);
            mDeFenGo.SetActive(false);
     
            int currOffice = 0;
            if (CardFiveStarRoom.Ins != null)
            {
                mReturnBtn.gameObject.SetActive(false);
                mAnewPlayBtn.gameObject.SetActive(false);
                mShareBtn.gameObject.SetActive(true);
                mGoOnBtn.gameObject.SetActive(true);
                currOffice = CardFiveStarRoom.Ins._CuurRoomOffice;
            }
            else
            {
                mReturnBtn.gameObject.SetActive(true);
                mAnewPlayBtn.gameObject.SetActive(true);
                mShareBtn.gameObject.SetActive(false);
                mGoOnBtn.gameObject.SetActive(false);
                currOffice = FiveStarVideoRoom.Ins._CurrRoomOffice;
            }
            mPlayerTitleText.text = $"玩家({currOffice}局)";
        }
    }
}
