using System.Collections.Generic;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    public class OnePengGang : Entity
    {
        public GameObject gameObject;
        private int DaoDiCardType= CardFiveStarCardType.None;
        private int DaoDiCardBeiMianType = CardFiveStarCardType.None;
        private int _OperateTyp = FiveStarOperateType.None;
        public int _CardSize = 0;

        public Transform _CardGroup;
        public Transform _GangCardPoint;
        public GameObject _Pointer;
        public GameObject _GangPointer;

        public void Init(GameObject go,int daoDadoiType,int beiMianType)
        {
            gameObject = go;
            DaoDiCardType = daoDadoiType;
            DaoDiCardBeiMianType = beiMianType;
            Transform transform = gameObject.transform;
            _CardGroup=ETModel.GameObjectHelper.FindChild(transform, "CardGroup");
            _GangCardPoint = ETModel.GameObjectHelper.FindChild(transform, "GangCardPoint");
            _Pointer = ETModel.GameObjectHelper.FindChild(transform, "Pointer").gameObject;
            if (transform.Find("GangPointer") != null)
            {
                _GangPointer = ETModel.GameObjectHelper.FindChild(transform, "GangPointer").gameObject;
            }
        }
        List<CardFiveStarCard> _PengGangCards=new List<CardFiveStarCard>();
        private CardFiveStarCard _GangPaiCard;
        public void ClearPengGangCards()
        {
            foreach (var card in _PengGangCards)
            {
                card.Destroy();
            }
            _PengGangCards.Clear();
            _GangPaiCard?.Destroy();
            _GangPaiCard = null;
            _Pointer.SetActive(false);
            _GangPointer?.SetActive(false);
        }
        private static Dictionary<int, Quaternion> SeatInPointerRotation=new Dictionary<int, Quaternion>()
        {
            {0, Quaternion.Euler(0,0,-90) },
            {1, Quaternion.Euler(0,0,0) },
            {2, Quaternion.Euler(0,0,90) },
            {3, Quaternion.Euler(0,0,180) },
        };

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetUI(int operateType,int cardSize,int seatIndex=0)
        {
            gameObject.SetActive(true);
            ClearPengGangCards();
            
            _CardSize = cardSize;

            //擦杠不要更改指针的方向
            if (!(_OperateTyp== FiveStarOperateType.Peng&&operateType==FiveStarOperateType.CaGang))
            {
                _Pointer.transform.localRotation = SeatInPointerRotation[seatIndex];
                if (_GangPointer != null)
                {
                    //显示并更改指针的方向
                    _GangPointer.SetActive(true);
                    _GangPointer.transform.localRotation = SeatInPointerRotation[seatIndex];
                }
            }
            _OperateTyp = operateType;
            //根据类型 显示碰的牌 和额外杠的一张牌
            switch (_OperateTyp)
            {
                case FiveStarOperateType.Peng:
                    ShowPengCard(DaoDiCardType);
                    break;
                case FiveStarOperateType.CaGang:
                case FiveStarOperateType.MingGang:
                    ShowPengCard(DaoDiCardType);
                    ShowGangCard(DaoDiCardType);
                    break;
                case FiveStarOperateType.AnGang:
                    ShowPengCard(DaoDiCardBeiMianType);
                    if (DaoDiCardBeiMianType == CardFiveStarCardType.Down_DaoDi_BeiMian)
                    {
                        ShowGangCard(DaoDiCardType);//自己要看见 自己按杠的什么牌
                    }
                    else
                    {
                        ShowGangCard(DaoDiCardBeiMianType);
                    }
                    break;
            }
            //根据类型显示不同的指针
            switch (_OperateTyp)
            {
                case FiveStarOperateType.Peng:
                    _Pointer.SetActive(true);
                    _GangPointer?.SetActive(false);
                    break;
                case FiveStarOperateType.CaGang:
                case FiveStarOperateType.MingGang:
                    if (_GangPointer == null)
                    {
                        _Pointer.SetActive(true);
                    }
                    else
                    {
                        _Pointer.SetActive(false);
                        _GangPointer.SetActive(true);
                    }
                    break;
                case FiveStarOperateType.AnGang:
                    _Pointer.SetActive(false);
                    _GangPointer?.SetActive(false);
                    break;
            }
        }

        //显示普通的三张牌
        private void ShowPengCard(int cardType)
        {
            for (int i = 0; i < 3; i++)
            {
                _PengGangCards.Add(CardFiveStarCardPool.Ins.Create(cardType, _CardSize, _CardGroup));
            }
        }
        //单独显示杠的一张牌
        private void ShowGangCard(int cardType)
        {
            _GangPaiCard = CardFiveStarCardPool.Ins.Create(cardType, _CardSize, _GangCardPoint);
            _GangPaiCard.LocalPositionZero();
        }

    }
}
