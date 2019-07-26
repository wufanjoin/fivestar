using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    public class JoyLandlordsCardPrefabType
    {
        public const int Small = 1;//小
        public const int Mid = 2;//中
        public const int Large = 3;//大
    }
    public class JoyLandlordsCardHuaSeType
    {
        public const int HeiTao = 0;
        public const int HongXin = 1;
        public const int MeiHua = 2;
        public const int FangKuai = 3;
    }
    public class JoyLandlordsCard:Entity
    {
        public int pCardPrefabType;
        public int pCardSize;
        public int pCardHuaSeType;
        public int pCardNum;
        public GameObject gameObject;

        public GameObject mMengBanGo;
        public Image mCardLargeTypeImage;
        public Image mLandlordIconImage;

        public Image mCardKingSizeImage;
        public Image mCardKingTypeImage;

        public Image mCardTypeImage;
        public Image mCardSizeImage;

        public void Init(GameObject go)
        {
            gameObject = go;
            if (pCardPrefabType == JoyLandlordsCardPrefabType.Large)
            {
                mCardLargeTypeImage = go.Get<GameObject>("CardLargeTypeImage").GetComponent<Image>();
                mMengBanGo = go.Get<GameObject>("MengBanGo");
            }
           
            mLandlordIconImage = go.Get<GameObject>("LandlordIconImage").GetComponent<Image>();
            mCardKingSizeImage = go.Get<GameObject>("CardKingSizeImage").GetComponent<Image>();
            mCardKingTypeImage = go.Get<GameObject>("CardKingTypeImage").GetComponent<Image>();
            mCardTypeImage = go.Get<GameObject>("CardTypeImage").GetComponent<Image>();
            mCardSizeImage = go.Get<GameObject>("CardSizeImage").GetComponent<Image>();
        }

        //显示或者隐藏蒙版
        public void SetMengBanActive(bool isActive)
        {
            mMengBanGo?.SetActive(isActive);
        }
        //设置这个牌所对应的牌
        public void SetCardDataUI(int num)
        {
            if (pCardNum != num)
            {
                pCardNum = num;
                pCardSize = JoyLandlordsCardTool.CardConvertorSize(num);
                pCardHuaSeType = JoyLandlordsCardTool.CardConvertorType(num);
                RestorUI();
            }
            gameObject.SetActive(true);
        }
        //隐藏这张牌
        public void HideCard()
        {
            gameObject.SetActive(false);
        }
        //根据自己当前牌的信息显示对应的UI
        public void RestorUI()
        {
            SetMengBanActive(false);
            mLandlordIconImage.gameObject.SetActive(false);
            IsKingUI(pCardNum > 52);
            if (pCardNum >52)
            {
                mCardKingSizeImage.sprite=JoyLandlordsCardTool.GetCardSizeSprite(pCardPrefabType, pCardSize, pCardHuaSeType);
                mCardKingTypeImage.sprite = JoyLandlordsCardTool.GetCardHuaSeTypeSprite(pCardPrefabType, pCardSize, pCardHuaSeType);
            }
            mCardSizeImage.sprite = JoyLandlordsCardTool.GetCardSizeSprite(pCardPrefabType, pCardSize, pCardHuaSeType);
            mCardTypeImage.sprite = JoyLandlordsCardTool.GetCardHuaSeTypeSprite(pCardPrefabType, pCardSize, pCardHuaSeType);
            if (pCardPrefabType == JoyLandlordsCardPrefabType.Large)
            {
                mCardLargeTypeImage.sprite = JoyLandlordsCardTool.GetCardHuaSeTypeSprite(pCardPrefabType, pCardSize, pCardHuaSeType);
            }
        }
        //设置显示普通UI节点还是 王的节点
        public void IsKingUI(bool isKing)
        {
            mCardKingSizeImage.gameObject.SetActive(isKing);
            mCardKingTypeImage.gameObject.SetActive(isKing);
            mCardTypeImage.gameObject.SetActive(!isKing);
            mCardSizeImage.gameObject.SetActive(!isKing);
            if (pCardPrefabType == JoyLandlordsCardPrefabType.Large)
            {
                mCardLargeTypeImage.gameObject.SetActive(!isKing);
            }
        }
        public void SetParent(Transform parent)
        {
            gameObject.transform.SetParent(parent);
        }


        public void Destroy()
        {
            JoyLandlordsCardPool.Ins.DestroyJoyLandlordsCard(this);
            gameObject.SetActive(false);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
