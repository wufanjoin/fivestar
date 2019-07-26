using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using NPOI.SS.Formula.Functions;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

namespace ETHotfix
{
    [ObjectSystem]
    public class SmallPlayerInfoItemGoItemAwakeSystem : AwakeSystem<SmallPlayerInfoItemGoItem, GameObject, FiveStar_SmallPlayerResult>
    {
        public override void Awake(SmallPlayerInfoItemGoItem self, GameObject go, FiveStar_SmallPlayerResult data)
        {
            self.Awake(go, data, UIType.FiveStarSmallResultPanel);
        }
    }

    public class SmallPlayerInfoItemGoItem : BaseItem<FiveStar_SmallPlayerResult>
    {
        #region 脚本工具生成的代码
        private GameObject mSinglePlayerInfoGo;
        private Image mHeadImage;
        private GameObject mZiMoGo;
        private GameObject mHuPaiGo;
        private GameObject mFangPaoGo;
        private Text mNameText;
        private Text mIDText;
        private Text mSocreInfoText;
        private Text mHeJiText;
        private Text mDeFenText;
        private Text mTotalScoreText;
        private GameObject mCutoffGo;
        private GameObject mSingleHandGroupGo;
        private HorizontalLayoutGroup mHandGroup;
        public override void Awake(GameObject go, FiveStar_SmallPlayerResult data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mZiMoGo = rc.Get<GameObject>("ZiMoGo");
            mHuPaiGo = rc.Get<GameObject>("HuPaiGo");
            mFangPaoGo = rc.Get<GameObject>("FangPaoGo");
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIDText = rc.Get<GameObject>("IDText").GetComponent<Text>();
            mSocreInfoText = rc.Get<GameObject>("SocreInfoText").GetComponent<Text>();
            mHeJiText = rc.Get<GameObject>("HeJiText").GetComponent<Text>();
            mDeFenText = rc.Get<GameObject>("DeFenText").GetComponent<Text>();
            mTotalScoreText = rc.Get<GameObject>("TotalScoreText").GetComponent<Text>();
            mCutoffGo = rc.Get<GameObject>("CutoffGo");
            mSingleHandGroupGo = rc.Get<GameObject>("SingleHandGroupGo");
            mHandGroup = mSingleHandGroupGo.transform.parent.GetComponent<HorizontalLayoutGroup>();
            InitPanel();
        }
        #endregion

        private Transform _HandGroupParentTrm;
        public  void InitPanel()
        {
            _HandGroupParentTrm = mSingleHandGroupGo.transform.parent;
            for (int i = 0; i < 6; i++)
            {
                GameObject.Instantiate(mSingleHandGroupGo, _HandGroupParentTrm);
            }
        }
        CardFiveStarPlayer _CardFiveStarPlayer;
        //显示这个玩家的UI信息
        public void SetUI(FiveStar_SmallPlayerResult playerResult)
        {
            Show();
            mData = playerResult;
            if (CardFiveStarRoom.Ins != null)
            {
                _CardFiveStarPlayer = CardFiveStarRoom.Ins.GetFiveStarPlayer(mData.SeatIndex);
            }
            else
            {
                _CardFiveStarPlayer = FiveStarVideoRoom.Ins.GetFiveStarPlayer(mData.SeatIndex);
            }
            
            CutMathRoomCardUI();
            ShowUserInfo();
            ShowPengGangHnads();
            ShowScore();
            ShowHuPaiInfo();
        }

        private List<CardFiveStarCard> _Cards = new List<CardFiveStarCard>();

        private int _currShowCardCount = 0;
        private void ClearCards()
        {
            _currShowCardCount = 0;
            foreach (var card in _Cards)
            {
                card.Destroy();
            }
            _Cards.Clear();
        }
        //显示碰杠
        public void ShowPengGang(int card, int count)
        {
            int _childCount = _currShowCardCount++;
            for (int i = 0; i < count; i++)
            {
                CardFiveStarCard cardEntity=CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, card,
                    _HandGroupParentTrm.GetChild(_childCount), 0.45f);
                _Cards.Add(cardEntity);
            }

        }
        //显示手牌 和赢的牌
        public void ShowHand(RepeatedField<int> cards)
        {
            int _childCount = _currShowCardCount++;
            for (int i = 0; i < cards.Count; i++)
            {
                CardFiveStarCard cardEntity = CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, cards[i],
                    _HandGroupParentTrm.GetChild(_childCount), 0.45f);
                _Cards.Add(cardEntity);
            }
        }
        //显示用户信息
        private async void ShowUserInfo()
        {
            //名字
            mNameText.text = _CardFiveStarPlayer._user.Name;
            //ID
            mIDText.text = "ID:" + _CardFiveStarPlayer._user.GetShowUserId();
            //头像
            mHeadImage.sprite = await _CardFiveStarPlayer._user.GetHeadSprite();
            //胡牌标志的
            mZiMoGo.SetActive(mData.PlayerResultType == FiveStarPlayerResultType.ZiMoHu);
            mHuPaiGo.SetActive(mData.PlayerResultType == FiveStarPlayerResultType.HuFangChong);
            mFangPaoGo.SetActive(mData.PlayerResultType == FiveStarPlayerResultType.FangChong);
        }
        //显示碰杠信息
        private void ShowPengGangHnads()
        {
            ClearCards();//显示之前先清空一下牌
            //显示玩家的牌
            for (int i = 0; i < mData.PengGangInfos.Count; i++)
            {
                //碰杠的牌
                if (mData.PengGangInfos[i].OperateType == FiveStarOperateType.Peng)
                {
                    ShowPengGang(mData.PengGangInfos[i].Card, 3);
                }
                else if (mData.PengGangInfos[i].OperateType == FiveStarOperateType.MingGang ||
                         mData.PengGangInfos[i].OperateType == FiveStarOperateType.AnGang ||
                         mData.PengGangInfos[i].OperateType == FiveStarOperateType.CaGang)
                {
                    ShowPengGang(mData.PengGangInfos[i].Card, 4);
                }
            }
            //显示手牌
            ShowHand(mData.Hands);
            //如果是胡牌玩家还要显示赢的牌
            if (mData.PlayerResultType == FiveStarPlayerResultType.HuFangChong|| mData.PlayerResultType == FiveStarPlayerResultType.ZiMoHu)
            {
                ShowHand(new RepeatedField<int>() { mData.WinCard });
            }
            //因为 UI布局不是实时生效 所以先隐藏 在显示就OK了
            CoroutineMgr.StartCoroutinee(AwaitShowHandGroups());
        }


        public IEnumerator AwaitShowHandGroups()
        {
            mHandGroup.enabled = false;
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForFixedUpdate();
            }
          
            mHandGroup.enabled = true;
        }
        //显示得分情况
        private void ShowScore()
        {
            //玩家现在分数
            mHeJiText.text = mData.GetScore.ToString();
            mDeFenText.text = mData.GetScore.ToString();
            mTotalScoreText.text = mData.NowScore.ToString();
        }
        //显示胡牌信息
        private void ShowHuPaiInfo()
        {
            //胡牌信息
            string _huPaidesc = CardFiveStarHuPaiType.GetHuPaiTypesDesc(mData.HuPaiTypes, mData.PlayerResultType == FiveStarPlayerResultType.ZiMoHu);
            if (_CardFiveStarPlayer.IsLiangDao)
            {
                _huPaidesc += $"亮倒  ";
            }
            if (mData.SamllGangPaiScore != 0)
            {
                _huPaidesc += $"杠分({mData.SamllGangPaiScore})  ";
            }
            if (_CardFiveStarPlayer._PiaoSocreNum > 0)
            {
                _huPaidesc += $"漂{_CardFiveStarPlayer._PiaoSocreNum}分  ";
            }
            if (Actor_FiveStar_MaiMaHandler._IsMaima&& mData.PlayerResultType == FiveStarPlayerResultType.ZiMoHu)
            {
                _huPaidesc += $"买马({Actor_FiveStar_MaiMaHandler._MaiMaScore})";
            }
            mSocreInfoText.text = _huPaidesc;
        }
        //区分显示匹配和房卡模型UI
        private void CutMathRoomCardUI()
        {
            mDeFenText.gameObject.SetActive(FiveStarSmallResultPanelComponent.CurrRoomType == RoomType.Match);
            mHeJiText.gameObject.SetActive(FiveStarSmallResultPanelComponent.CurrRoomType == RoomType.RoomCard);
            mTotalScoreText.gameObject.SetActive(FiveStarSmallResultPanelComponent.CurrRoomType == RoomType.RoomCard);

        }


    }
}
