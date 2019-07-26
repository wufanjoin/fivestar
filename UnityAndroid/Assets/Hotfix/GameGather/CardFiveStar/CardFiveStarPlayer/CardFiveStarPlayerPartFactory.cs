using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class CardFiveStarPlayerPartFactory
    {
        //创建准备对象
        private static GameObject RedyIconPrefab;

        public static GameObject CreateReadyGo(Transform parentTrm)
        {
            if (RedyIconPrefab == null)
            {
                RedyIconPrefab =
                    ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "ReadyIcon") as GameObject;
            }
            GameObject ReadyIcon = GameObject.Instantiate(RedyIconPrefab, parentTrm);
            ReadyIcon.transform.localPosition = Vector3.zero;
            return ReadyIcon;
        }

        //创建人物头像对象
        private static GameObject PlayerHeadPrefab;

        public static CardFiveStarPlayerHead CreateHead(CardFiveStarPlayer player, Transform parentTrm)
        {
            CardFiveStarPlayerHead cardFiveStarPlayerHead = ComponentFactory.Create<CardFiveStarPlayerHead>();
            if (PlayerHeadPrefab == null)
            {
                PlayerHeadPrefab =
                    ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "PlayerHead") as GameObject;
            }
            GameObject go = GameObject.Instantiate(PlayerHeadPrefab, parentTrm);
            go.transform.localPosition = Vector3.zero;
            cardFiveStarPlayerHead.Init(player, go);
            return cardFiveStarPlayerHead;
        }

        //创建吃碰杠
        public static List<OnePengGang> CreatePengGangs(GameObject go, int daodiType, int beiMianType)
        {
            List<OnePengGang> onePengGangs = new List<OnePengGang>();
            for (int i = 0; i < 4; i++)
            {
                OnePengGang onePengGang = ComponentFactory.Create<OnePengGang>();
                GameObject insGo=GameObject.Instantiate(go, go.transform.parent);
                onePengGang.Init(insGo, daodiType, beiMianType);
                onePengGangs.Add(onePengGang);
            }
            go.SetActive(false);
            return onePengGangs;
        }
    }
}
