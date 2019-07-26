using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class JoyLandlordsCardFactory
    {
        private static Dictionary<int, GameObject> mCardPrefabDic;
        public static JoyLandlordsCard Create(int cardPrefabType, int cardNum, Transform parenTransform)
        {
            JoyLandlordsCard joyLandlordsCard=ComponentFactory.Create<JoyLandlordsCard>();
            if (mCardPrefabDic == null)
            {
                InitCardPrefab();
            }
            joyLandlordsCard.pCardPrefabType = cardPrefabType;
            joyLandlordsCard.Init(GameObject.Instantiate(mCardPrefabDic[cardPrefabType], parenTransform));
            joyLandlordsCard.SetCardDataUI(cardNum);
            return joyLandlordsCard;
        }

        private static void InitCardPrefab()
        {
            mCardPrefabDic=new Dictionary<int, GameObject>();
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            mCardPrefabDic.Add(JoyLandlordsCardPrefabType.Small, resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "SmallCardItem") as GameObject);
            mCardPrefabDic.Add(JoyLandlordsCardPrefabType.Mid, resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "MidCardItem") as GameObject);
            mCardPrefabDic.Add(JoyLandlordsCardPrefabType.Large, resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "LargeCardItem") as GameObject);
        }

    }
}
