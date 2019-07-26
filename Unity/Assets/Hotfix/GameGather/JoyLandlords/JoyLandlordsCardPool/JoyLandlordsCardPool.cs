using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class JoyLandlordsCardPool: Single<JoyLandlordsCardPool>
    {
        private GameObject mPoolGo;
        private Dictionary<int,List<JoyLandlordsCard>> CardGoPool=new Dictionary<int, List<JoyLandlordsCard>>()
        {
            {JoyLandlordsCardPrefabType.Large,new List<JoyLandlordsCard>() },
            {JoyLandlordsCardPrefabType.Mid,new List<JoyLandlordsCard>() },
            {JoyLandlordsCardPrefabType.Small,new List<JoyLandlordsCard>() },
        };

        public override void Init()
        {
            base.Init();
            mPoolGo = new GameObject("JoyLandlordsCardPool");
            mPoolGo.transform.SetParent(GameObject.Find("Global/UI").transform);
        }

        //创建一个牌的对象
        public JoyLandlordsCard Create(int cardPrefabType,int cardNum,Transform parenTransform)
        {
            JoyLandlordsCard joyLandlordsCard;
            if (CardGoPool[cardPrefabType].Count > 1)
            {
                joyLandlordsCard = CardGoPool[cardPrefabType][0];
                CardGoPool[cardPrefabType].RemoveAt(0);
                joyLandlordsCard.SetParent(parenTransform);
                joyLandlordsCard.SetCardDataUI(cardNum);
            }
            else
            {
                joyLandlordsCard=JoyLandlordsCardFactory.Create(cardPrefabType, cardNum, parenTransform);
            }
            return joyLandlordsCard;
        }

        //销毁一个牌的实体
        public void DestroyJoyLandlordsCard(JoyLandlordsCard joyLandlordsCard)
        {
            joyLandlordsCard.SetParent(mPoolGo.transform);
            CardGoPool[joyLandlordsCard.pCardPrefabType].Add(joyLandlordsCard);
        }
    }
}
