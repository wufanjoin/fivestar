using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class CardFiveStarCardFactory
    {

        private static Dictionary<int, GameObject> _cardPrefabDic;
        public static CardFiveStarCard Create(int cardPrefabType, int size, Transform parenTransform)
        {
            if (cardPrefabType == CardFiveStarCardType.Down_DaoDi_ZhengMain)
            {
                return Create<CardFiveStarDownDaoDiZhengMainCard>(cardPrefabType, size, parenTransform);
            }
            return Create<CardFiveStarCard>(cardPrefabType, size, parenTransform);
        }

        public static CardFiveStarHand CreateHand(int size,int index ,Transform parenTransform)
        {
            CardFiveStarHand cardFiveStarHand=Create<CardFiveStarHand>(CardFiveStarCardType.CardFiveStarHand, size, parenTransform);
            cardFiveStarHand.iHandIndex = index;
            return cardFiveStarHand;
        }

        public static CardFiveStarNewHand CreateNewHand(int size, Transform parenTransform)
        {
            return Create<CardFiveStarNewHand>(CardFiveStarCardType.CardFiveStarHand, size, parenTransform);
        }
        private static T Create<T>(int cardPrefabType, int size, Transform parenTransform) where T: CardFiveStarCard
        {
            T cardFiveStarCard = ComponentFactory.Create<T>();
            if (_cardPrefabDic == null)
            {
                InitCardPrefab();
            }
            cardFiveStarCard.CardType = cardPrefabType;
            cardFiveStarCard.Init(GameObject.Instantiate(_cardPrefabDic[cardPrefabType], parenTransform));
            cardFiveStarCard.SetCardUI(size);
            return cardFiveStarCard;
        }
        private static void InitCardPrefab()
        {
            _cardPrefabDic = new Dictionary<int, GameObject>();
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            _cardPrefabDic.Add(CardFiveStarCardType.Down_ZhiLi_ZhengMain, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Down_ZhiLi_ZhengMain") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Down_DaoDi_ZhengMain, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Down_DaoDi_ZhengMain") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Down_DaoDi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Down_DaoDi_BeiMian") as GameObject);

            _cardPrefabDic.Add(CardFiveStarCardType.Right_ZhiLi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Right_ZhiLi_BeiMian") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Right_DaoDi_ZhengMain, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Right_DaoDi_ZhengMain") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Right_DaoDi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Right_DaoDi_BeiMian") as GameObject);

            _cardPrefabDic.Add(CardFiveStarCardType.Up_ZhiLi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Up_ZhiLi_BeiMian") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Up_DaoDi_ZhengMain, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Up_DaoDi_ZhengMain") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Up_DaoDi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Up_DaoDi_BeiMian") as GameObject);

            _cardPrefabDic.Add(CardFiveStarCardType.Left_ZhiLi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Left_ZhiLi_BeiMian") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Left_DaoDi_ZhengMain, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Left_DaoDi_ZhengMain") as GameObject);
            _cardPrefabDic.Add(CardFiveStarCardType.Left_DaoDi_BeiMian, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "Card_Left_DaoDi_BeiMian") as GameObject);

            _cardPrefabDic.Add(CardFiveStarCardType.CardFiveStarHand, resourcesComponent.GetResoure(UIType.CardFiveStarRoomPanel, "CardFiveStarHand") as GameObject);

        }
    }
}
