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

    public static partial class JoyLandlordsCardTool
    {
       
        private readonly static Dictionary<int, string> TypeAtlasDic = new Dictionary<int, string>()
        {
            {JoyLandlordsCardPrefabType.Large, "LargeCard_"},
            {JoyLandlordsCardPrefabType.Mid, "MidCard_"},
            {JoyLandlordsCardPrefabType.Small, "SmallCard_"}
        };

        private const string HuaSe = "huase_";
        private const string HongNum = "commom_shuzi_hong_";
        private const string HeiNum = "commom_shuzi_hei_";

        private const string DaKingIcon = "LargeCard_king_huase_15";
        private const string XiaoKingIcon = "LargeCard_king_huase_14";
        private const string DaKingNum = "king_15";
        private const string XiaoKingNum = "king_14";
        public static Sprite GetCardSizeSprite(int prefabType, int size, int huaSeType)
        {
            switch (size)
            {
                case 53:
                    return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel,
                        TypeAtlasDic[prefabType] + XiaoKingNum) as Sprite;
                case 54:
                    return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel,
                        TypeAtlasDic[prefabType] + DaKingNum) as Sprite;
            }
            if (size > 13)
            {
                size -= 13;
            }
            string typeColorNum = string.Empty;
            switch (huaSeType)
            {
                case JoyLandlordsCardHuaSeType.FangKuai:
                case JoyLandlordsCardHuaSeType.HongXin:
                    typeColorNum = HongNum;
                    break;
                case JoyLandlordsCardHuaSeType.HeiTao:
                case JoyLandlordsCardHuaSeType.MeiHua:
                    typeColorNum = HeiNum;
                    break;
                default:
                    Log.Error("牌的花色不正确"+ huaSeType);
                    break;

            }
            return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel,
                TypeAtlasDic[prefabType] + typeColorNum + size) as Sprite;

        }

        public static Sprite GetCardHuaSeTypeSprite(int prefabType, int size, int huaSeType)
        {
            switch (size)
            {
                case 53:
                    return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel, XiaoKingIcon) as Sprite;
                case 54:
                    return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel, DaKingIcon) as Sprite;
            }
            return ResourcesComponent.Ins.GetResoure(UIType.JoyLandlordsRoomPanel,TypeAtlasDic[prefabType] + HuaSe + (huaSeType+1)) as Sprite;
        }
    }

}
