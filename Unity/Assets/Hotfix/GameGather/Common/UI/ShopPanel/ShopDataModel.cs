using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public partial class Commodity
    {
        public Sprite GetIconSprite()
        {
            string iconStr = "";
            if (CommodityType == GoodsId.Besans)
            {
                iconStr = "besansIcon_0";
            }
            else
            {
                iconStr = "jewelIcon_0";
            }
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            return resourcesComponent.GetResoure(UIType.ShopPanel, iconStr + Lv) as Sprite;
        }

        public string MoneyTypeStr
        {
            get
            {
                if (MonetaryType == GoodsId.CNY)
                {
                    return "￥";
                }
                else
                {
                    return "钻";
                }
            }
        }
        public string ChineseType
        {
            get
            {
                if (CommodityType == GoodsId.Jewel)
                {
                    return "钻";
                }
                else
                {
                    return "豆";
                }
            }
        }
    }
}