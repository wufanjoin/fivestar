using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarRightPlayer : CardFiveStarPlayer
    {
        public override int HandType
        {
            get { return CardFiveStarCardType.Right_ZhiLi_BeiMian; }
        }
        public override int ChuCardType
        {
            get { return CardFiveStarCardType.Right_DaoDi_ZhengMain; }
        }
        public override int LaingCardType
        {
            get { return CardFiveStarCardType.Right_DaoDi_ZhengMain; }
        }
        public override int PengCardType
        {
            get { return CardFiveStarCardType.Right_DaoDi_ZhengMain; }
        }
        public override int AnGangCardType
        {
            get { return CardFiveStarCardType.Right_DaoDi_BeiMian; }
        }

        public GameObject _OneChuCardGroupGo;
        public List<Transform> _ChuCardGroups = new List<Transform>();
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
        }

        public override CardFiveStarCard AddChuCard(int cardSize)
        {
            int groupIndex = _ChuCards.Count / 10;
            CardFiveStarCard card = CardFiveStarCardPool.Ins.Create(ChuCardType, cardSize, _ChuCardGroups[groupIndex]);
            card.gameObject.transform.SetAsFirstSibling(); ;
            _ChuCards.Add(card);
            return card;
        }

        ////初始化碰杠集合 上和右边的玩家要特殊处理下 颠倒一下位置
        //protected override void InitPengGangs()
        //{
        //    base.InitPengGangs();
        //    ReversePengGangsPoint();
        //}

        //右边玩家亮牌要颠倒一下
        public override void ShowLiangCards(IList<int> cards)
        {
            base.ShowLiangCards(cards);
            for (int i = 0; i < _liangCards.Count; i++)
            {
                _liangCards[i].gameObject.transform.SetAsFirstSibling();
            }
        }

        //右边玩家亮牌要颠倒一下
        public override void AddLiangCards(int cardType,IList<int> cards)
        {
            base.AddLiangCards(cardType, cards);
            for (int i = _liangCards.Count- cards.Count; i < _liangCards.Count; i++)
            {
                _liangCards[i].gameObject.transform.SetAsFirstSibling();
            }
        }
    }
}
