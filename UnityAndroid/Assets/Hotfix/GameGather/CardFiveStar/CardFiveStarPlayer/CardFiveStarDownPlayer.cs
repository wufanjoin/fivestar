using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    public partial class CardFiveStarDownPlayer : CardFiveStarPlayer
    {
        public override int ChuCardJianTouYAxleOffset//出牌箭头Y轴高度
        {
            get { return 60; }
        }
        public override int HandType
        {
            get { return CardFiveStarCardType.Down_ZhiLi_ZhengMain; }
        }
        public override int ChuCardType
        {
            get { return CardFiveStarCardType.Down_DaoDi_ZhengMain; }
        }
        public override int LaingCardType
        {
            get { return CardFiveStarCardType.Down_DaoDi_ZhengMain; }
        }
        public override int PengCardType
        {
            get { return CardFiveStarCardType.Down_DaoDi_ZhengMain; }
        }
        public override int AnGangCardType
        {
            get { return CardFiveStarCardType.Down_DaoDi_BeiMian; }
        }

        public GameObject _OneChuCardGroupGo;
        public List<Transform> _ChuCardGroups = new List<Transform>();
        public CardFiveStarHandComponent _CardFiveStarHandComponent;
        public override void Init(GameObject go)
        {
            base.Init(go);
            _OneChuCardGroupGo = _ChuCardGroupsGo.FindChild("OneChuCardGroup").gameObject;
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(_OneChuCardGroupGo, _ChuCardGroupsGo.transform);
            }
            for (int i = _ChuCardGroupsGo.transform.childCount - 1; i >= 0; i--)
            {
                _ChuCardGroups.Add(_ChuCardGroupsGo.transform.GetChild(i));
            }
            _CardFiveStarHandComponent = this.AddComponent<CardFiveStarHandComponent, Transform, Transform>(_HandGroup.transform, _NewHandPointGo.transform);
            EventMsgMgr.RegisterEvent(CommEventID.SelfUserInfoRefresh,RefreshBeans);
        }

        public void RefreshBeans(params object[] objs)
        {
            if (CardFiveStarRoom.Ins != null&& CardFiveStarRoom.Ins._RoomType==RoomType.Match)
            {
                _NowSocre = UserComponent.Ins.pSelfUser.Beans;
                _PlayerHead.RenewalSocre();
            }
        }
        //增加出的牌
        public override CardFiveStarCard AddChuCard(int cardSize)
        {
            int groupIndex = _ChuCards.Count / 10;
            CardFiveStarCard card = CardFiveStarCardPool.Ins.Create(ChuCardType, cardSize, _ChuCardGroups[groupIndex], 0.53f);
            _ChuCards.Add(card);
            return card;
        }

        //直接刷新手牌
        public override void ShowHands(RepeatedField<int> cards)
        {
            _CardFiveStarHandComponent.RefreshHand(cards);
        }

        //清除所有手牌
        public override void ClearHand()
        {
            _CardFiveStarHandComponent.ClearHands();
        }
        //发牌
        public override async void Deal(params object[] objs)
        {
            if ((!gameObject.activeInHierarchy) || IsRestIn)
            {
                return;
            }
            gameObject.transform.SetAsLastSibling();//每次发牌 把自己的节点 设置到最后一个
            RepeatedField<int> hands = objs[0] as RepeatedField<int>;
            _CardFiveStarHandComponent.Deal(hands);
            CardFiveStarRoom.Ins.ReduceCardTotalNum(hands.Count);//减少牌的总数
            CardFiveStarRoom.Ins.ReduceCardInNum(hands);//减少每张牌对应的数量
        }


        //摸牌
        public override void MoCard(int card)
        {
            base.MoCard(card);
            CardFiveStarRoom.Ins.ReduceCardInNum(card);//减少牌对应的数量
        }
        //显示摸牌
        public override void ShowMoCard(int card)
        {
            _CardFiveStarHandComponent.MoCard(card);
        }

        //隐藏摸的牌
        public override void HideMoCard()
        {
            CardFiveStarHandComponent.Ins._NewHand.SetActive(false);
        }
        //出牌
        public override  void ChuCard(int card)
        {
            Vector3 playCardPoint;
            if (CardFiveStarHandComponent.Ins._DragPlayPoint != Vector3.zero)//如果拖动出牌的 位置 不是0 就用拖动出牌的位置
            {
                playCardPoint = CardFiveStarHandComponent.Ins._DragPlayPoint;
                CardFiveStarHandComponent.Ins._DragPlayPoint = Vector3.zero;
            }
            else if (CardFiveStarHandComponent.Ins.UpChuCardHand != null)
            {
                playCardPoint = CardFiveStarHandComponent.Ins.UpChuCardHand.gameObject.transform.position;
            }
            else
            {
                playCardPoint = CardFiveStarHandComponent.Ins._NewHand.gameObject.transform.position;
            }
            CardFiveStarHandComponent.Ins.ChuCard(card);//UpChuCardHand 可能为空 这个方法会判断并赋值
            CoroutineMgr.StartCoroutinee(ChuCardAnimSound(card, playCardPoint, 0.5f));
        }

        public override  void PengGangHandAndCardNumChang(int operateType, int cardSize)
        {
           //这个父类方法就是空 碰杠之后 手牌变化由Hand组件管理 碰杠 之后牌的数量也不用减 因为摸牌的时候减过了
        }

        //显示漂数
        public override void DaPiao(int num)
        {
            base.DaPiao(num);
            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowAwaitHint();
        }

        //玩家可以出牌
        public override void CanChuCard()
        {
            base.CanChuCard();
            _CardFiveStarHandComponent.CanChuCard();
        }

        //显示亮倒的牌
        public override void ShowLiangDaoCards(RepeatedField<int> cards)
        {
            RepeatedField<RepeatedField<int>> liangDaoCards = CardFiveStarHuPaiLogic.GetLiangDaoNoneHuCards(cards);
            ClearLiangCards();
            liangDaoCards[1].Sort();
            for (int i = 0; i < liangDaoCards[1].Count; i++)
            {
                CardFiveStarDownDaoDiZhengMainCard card = CardFiveStarCardPool.Ins.Create(LaingCardType, liangDaoCards[1][i], _LiangCardGroupGo.transform) as CardFiveStarDownDaoDiZhengMainCard;
                _liangCards.Add(card);
                card.ShowKouIcon();
            }
            liangDaoCards[0].Sort();
            for (int i = 0; i < liangDaoCards[0].Count; i++)
            {
                _liangCards.Add(CardFiveStarCardPool.Ins.Create(LaingCardType, liangDaoCards[0][i], _LiangCardGroupGo.transform));
            }
        }

        //亮倒 情况下 杠之后 玩家手牌变化
        public override void LiangDaoGangHandChange(int cardSize)
        {
            List< CardFiveStarCard> _destroyCards=new List<CardFiveStarCard>();
            for (int i = 0; i < _liangCards.Count; i++)
            {
                CardFiveStarDownDaoDiZhengMainCard card=_liangCards[i] as CardFiveStarDownDaoDiZhengMainCard;
                if (card.CardSize == cardSize&& card._KouIconGo.activeInHierarchy)
                {
                    _destroyCards.Add(_liangCards[i]);
                }
            }
            foreach (var card in _destroyCards)
            {
                card.Destroy();
                _liangCards.Remove(card);
            }
        }

        //小结算
        public override void SmallResult(FiveStar_SmallPlayerResult playerResult)
        {
            base.SmallResult(playerResult);
        }
    }
}
