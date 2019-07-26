using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarCardPool : Single<CardFiveStarCardPool>
    {
        private GameObject _poolGo;
        private Dictionary<int, List<CardFiveStarCard>> CardGoPool = new Dictionary<int, List<CardFiveStarCard>>()
        {
            {CardFiveStarCardType.Down_ZhiLi_ZhengMain,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Down_DaoDi_ZhengMain,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Down_DaoDi_BeiMian,new List<CardFiveStarCard>() },

            {CardFiveStarCardType.Right_ZhiLi_BeiMian,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Right_DaoDi_ZhengMain,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Right_DaoDi_BeiMian,new List<CardFiveStarCard>() },

            {CardFiveStarCardType.Up_ZhiLi_BeiMian,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Up_DaoDi_ZhengMain,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Up_DaoDi_BeiMian,new List<CardFiveStarCard>() },

            {CardFiveStarCardType.Left_ZhiLi_BeiMian,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Left_DaoDi_ZhengMain,new List<CardFiveStarCard>() },
            {CardFiveStarCardType.Left_DaoDi_BeiMian,new List<CardFiveStarCard>() },

            {CardFiveStarCardType.CardFiveStarHand,new List<CardFiveStarCard>() },

        };

        public override void Init()
        {
            base.Init();
            _poolGo = new GameObject("CardFiveStarPool",typeof(RectTransform));
            _poolGo.transform.SetParent(GameObject.Find("Global/UI/PoolCanvas").transform);
            _poolGo.transform.localScale=Vector3.one;
        }

        //创建一个牌的对象
        public CardFiveStarCard Create(int cardPrefabType, Transform parenTransform, float scale = 1)
        {
            return Create(cardPrefabType, 0, parenTransform, scale);
        }

        //创建一个牌的对象
        public CardFiveStarCard Create(int cardPrefabType, int cardSize, Transform parenTransform, float scale = 1)
        {
            CardFiveStarCard cardFiveStarCard;
            if (CardGoPool[cardPrefabType].Count > 1)
            {
                cardFiveStarCard = CardGoPool[cardPrefabType][0];
                CardGoPool[cardPrefabType].RemoveAt(0);
                cardFiveStarCard.SetParent(parenTransform);
                cardFiveStarCard.SetCardUI(cardSize);
            }
            else
            {
                cardFiveStarCard = CardFiveStarCardFactory.Create(cardPrefabType, cardSize, parenTransform);
            }
            cardFiveStarCard.SetScale(scale);
            return cardFiveStarCard;
        }

        //创建一个手牌的对象
        public CardFiveStarHand CreateHand(int cardSize, int index, Transform parenTransform)
        {
            CardFiveStarHand cardFiveStarCard;
            if (CardGoPool[CardFiveStarCardType.CardFiveStarHand].Count > 1)
            {
                cardFiveStarCard = CardGoPool[CardFiveStarCardType.CardFiveStarHand][0] as CardFiveStarHand;
                CardGoPool[CardFiveStarCardType.CardFiveStarHand].RemoveAt(0);
                cardFiveStarCard.iHandIndex = index;
                cardFiveStarCard.SetParent(parenTransform);
                cardFiveStarCard.SetCardUI(cardSize);
            }
            else
            {
                cardFiveStarCard = CardFiveStarCardFactory.CreateHand(cardSize, index, parenTransform);
            }
            cardFiveStarCard.SetScale(1);
            return cardFiveStarCard;
        }
        //销毁一个牌的实体
        public void DestroyJoyLandlordsCard(CardFiveStarCard cardFiveStarCard)
        {
            cardFiveStarCard.SetParent(_poolGo.transform);
            CardGoPool[cardFiveStarCard.CardType].Add(cardFiveStarCard);
        }

    }
}
