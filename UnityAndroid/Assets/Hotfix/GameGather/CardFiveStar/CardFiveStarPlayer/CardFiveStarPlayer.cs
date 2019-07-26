using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    public partial class CardFiveStarPlayer : Entity
    {
        public GameObject gameObject;
        public GameObject _HandGroup;
        public GameObject _NewHandPointGo;
        public GameObject _OnePengGang;
        public GameObject _ChuCardGroupsGo;
        public GameObject _LiangCardGroupGo;
        public GameObject _WinCardPointGo;
        public GameObject _HeadPointGo;
        public GameObject _ReadyPoint;
        public GameObject _RestHintGo;

        public Transform _OperationSpecialPoint;
        
        public Transform _PersonageInfoPoint;
        public int ClientSeatIndex;//玩家所在的客户端索引
        public int ServerSeatIndex;//玩家所在的服务器索引
        public User _user;
        public bool IsRestIn = false;//是否休息中 就是轮休中

        public bool IsLiangDao = false;//是否亮倒
        public int _PiaoSocreNum = 0;//漂分数

        public int _ZiMoCount = 0;//自摸次数
        public int _FangChongCount = 0;//放冲次数
        public int _GangPaiCount = 0;//杠牌次数
        public int _HuPaiCount = 0;//胡牌次数
        public long _NowSocre = 0;//现在的分数

        public virtual int ChuCardJianTouYAxleOffset//出牌箭头Y轴高度
        {
            get { return 30; }
        }
        public virtual int HandType { get; }
        public virtual int ChuCardType { get; }
        public virtual int LaingCardType { get; }
        public virtual int PengCardType { get; }
        public virtual int AnGangCardType { get; }



        public virtual void Init(GameObject go)
        {
            gameObject = go;
            Transform transform = gameObject.transform;
            _HandGroup = ETModel.GameObjectHelper.FindChild(transform, "HandsGroup").gameObject;
            _NewHandPointGo = ETModel.GameObjectHelper.FindChild(transform, "NewHandPointGo").gameObject;
            _OnePengGang = ETModel.GameObjectHelper.FindChild(transform, "PengGangGroup/OnePengGang").gameObject;
            _ChuCardGroupsGo = ETModel.GameObjectHelper.FindChild(transform, "ChuCardGroupsGo").gameObject;
            _LiangCardGroupGo = ETModel.GameObjectHelper.FindChild(transform, "LiangCardGroupGo").gameObject;
            _HeadPointGo = ETModel.GameObjectHelper.FindChild(transform, "HeadPointGo").gameObject;
            _ReadyPoint = ETModel.GameObjectHelper.FindChild(transform, "ReadyPoint").gameObject;
            _OperationSpecialPoint = ETModel.GameObjectHelper.FindChild(transform, "OperationSpecialPoint");
            _PersonageInfoPoint= ETModel.GameObjectHelper.FindChild(transform, "PersonageInfoPoint");
            _WinCardPointGo = ETModel.GameObjectHelper.FindChild(transform, "WinCardPointGo").gameObject;

            _RestHintGo = ETModel.GameObjectHelper.FindChild(transform, "RestHintGo").gameObject;


        }
        //每小局要清空的数据
        public void LittleRoundClearData()
        {
            IsLiangDao = false;//是否亮倒
            _PiaoSocreNum = 0;//漂分
            IsRestIn = false;//是否休息中
            _RestHintGo.SetActive(false);//轮休图标隐藏
        }
        public void ClearData()
        {
            LittleRoundClearData();
            _ZiMoCount = 0;//自摸次数
            _FangChongCount = 0;//放冲次数
            _GangPaiCount = 0;//杠牌次数
            _HuPaiCount = 0;//胡牌次数
        }
        //初始化UI
        public virtual void InitUI(User user)
        {
            ClearData();
            _user = user;
            gameObject.SetActive(true);
   
            ShowPlayerHead(user);//显示玩家头像
            HideAllCard();
            SetReadyState(false);
            _RestHintGo.SetActive(false);
        }

        //隐藏所有牌
        public void HideAllCard()
        {
            HideMoCard();//隐藏摸的牌
            HideWinCard();//隐藏赢的牌
            ClearHand();//清除手牌
            ClearLiangCards();//清除亮牌
            HidePengGangs();//隐藏碰杠牌
            HideChuCard();//隐藏所有出的牌
        }
        //隐藏玩家UI
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        //显示玩家头像
        public CardFiveStarPlayerHead _PlayerHead;
        public virtual void ShowPlayerHead(User user)
        {
            if (_PlayerHead == null)
            {
                _PlayerHead = CardFiveStarPlayerPartFactory.CreateHead(this, _HeadPointGo.transform);
                return;
            }
            _PlayerHead.InitHeadInfo(this);
        }
        //创建摸的牌
        private CardFiveStarCard _MoCard;
        public void CreateMoCard()
        {
            _MoCard = CardFiveStarCardPool.Ins.Create(HandType, _NewHandPointGo.transform);
            _MoCard.LocalPositionZero();//本地左边归0
        }
        //显示摸的牌
        public virtual void ShowMoCard(int card)
        {
            if (_MoCard == null)
            {
                CreateMoCard();
            }
            _MoCard.SetActive(true);
        }
        //隐藏摸的牌
        public virtual void HideMoCard()
        {
            if (_MoCard == null)
            {
                return;
            }
            _MoCard.SetActive(false);
        }
        //创建赢的牌
        private CardFiveStarCard _WinCard;
        public void CreateWinCard()
        {
            _WinCard = CardFiveStarCardPool.Ins.Create(LaingCardType, _WinCardPointGo.transform);
            _WinCard.LocalPositionZero();//本地左边归0
        }
        //显示赢的牌
        public void ShowWinCard(int cardSize)
        {
            if (_WinCard == null)
            {
                CreateWinCard();
            }
            _WinCard.SetCardUI(cardSize);
            _WinCard.SetActive(true);
        }
        //隐藏赢的牌
        public void HideWinCard()
        {
            if (_WinCard == null)
            {
                return;
            }
            _WinCard.SetActive(false);
        }

        //显示对应的手牌数量
        private List<CardFiveStarCard> _hands = new List<CardFiveStarCard>();
        public virtual void ShowHands(RepeatedField<int> cards)
        {
            ClearHand();
            
            for (int i = 0; i < cards[0]; i++)
            {
                _hands.Add(CardFiveStarCardPool.Ins.Create(HandType, _HandGroup.transform));
            }
            if (cards[0]%3 == 2)
            {
                _hands[0].Destroy();
                _hands.RemoveAt(0);
                ShowMoCard(0);
            }
        }
        //清除所有手牌
        public virtual void ClearHand()
        {
            foreach (var hand in _hands)
            {
                hand.Destroy();
            }
            _hands.Clear();
        }
        //显示亮牌
        protected List<CardFiveStarCard> _liangCards = new List<CardFiveStarCard>();
        public virtual void ShowLiangCards(IList<int> cards)
        {
            ClearLiangCards();
            for (int i = 0; i < cards.Count; i++)
            {
                _liangCards.Add(CardFiveStarCardPool.Ins.Create(LaingCardType, cards[i], _LiangCardGroupGo.transform));
            }
        }
        //增加亮牌card
        public virtual void AddLiangCards(int cardType,IList<int> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                _liangCards.Add(CardFiveStarCardPool.Ins.Create(cardType, cards[i], _LiangCardGroupGo.transform));
            }
        }
        //增加亮牌card
        public virtual void AddLiangCards(int card)
        {
           _liangCards.Add(CardFiveStarCardPool.Ins.Create(LaingCardType, card, _LiangCardGroupGo.transform));
        }
        //清除所有亮牌
        public void ClearLiangCards()
        {
            foreach (var card in _liangCards)
            {
                card.Destroy();
            }
            _liangCards.Clear();
        }

        //初始化准备手套
        private GameObject readyGo;
        public void CreateReadyGo()
        {
            readyGo = CardFiveStarPlayerPartFactory.CreateReadyGo(_ReadyPoint.transform);
        }
        //设置准备状态
        public void SetReadyState(bool state)
        {
            if (readyGo == null)
            {
                CreateReadyGo();
            }
            readyGo.SetActive(state);
        }
        //吃碰杠集合
        public List<int> _PengGangArray=new List<int>();
        protected List<OnePengGang> _PengGangs;
        private int _CurrPengIndex = 0;
        //初始化碰杠
        protected virtual void InitPengGangs()
        {
            _PengGangs = CardFiveStarPlayerPartFactory.CreatePengGangs(_OnePengGang, PengCardType, AnGangCardType);
        }
        //初始化碰颠倒位置
        public void ReversePengGangsPoint()
        {
            List<OnePengGang> _guoDuOnePengGangs = new List<OnePengGang>(_PengGangs);

            for (int i = 0; i < _PengGangs.Count; i++)
            {
                _PengGangs[i] = _guoDuOnePengGangs[_PengGangs.Count - i - 1];
            }
        }
        //隐藏所有碰杠
        public void HidePengGangs()
        {
            if (_PengGangs == null)
            {
                return;
            }
            foreach (var pengGang in _PengGangs)
            {
                pengGang.Hide();
            }
            _CurrPengIndex = 0;
            _PengGangArray.Clear();
        }
        //添加碰杠信息 这个索引需要传递客户端索引
        public void AddPengGang(int operateType, int cardSize, int seatIndex = 0)
        {
            if (_PengGangs == null)
            {
                InitPengGangs();
            }
            if (_CurrPengIndex >= 4)
            {
                Log.Error("玩家的碰杠数已经到达最大值");
                return;
            }
            _PengGangs[_CurrPengIndex++].SetUI(operateType, cardSize, seatIndex);
            _PengGangArray.Add(cardSize);
        }
        //出牌的集合
        protected List<CardFiveStarCard> _ChuCards = new List<CardFiveStarCard>();
        //玩家出牌
        public virtual CardFiveStarCard AddChuCard(int cardSize)
        {
            CardFiveStarCard card = CardFiveStarCardPool.Ins.Create(ChuCardType, cardSize, _ChuCardGroupsGo.transform);
            _ChuCards.Add(card);
            return card;
        }
        //去掉最后一张出的牌
        public void RemoveEndChuCard()
        {
            _ChuCards[_ChuCards.Count - 1].Destroy();
            _ChuCards.RemoveAt(_ChuCards.Count - 1);
        }
        //隐藏所有出的牌
        public void HideChuCard()
        {
            foreach (var card in _ChuCards)
            {
                card.Destroy();
            }
            _ChuCards.Clear();
        }

    }
}
