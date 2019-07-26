using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public class GoodsId
    {
        public const long None = 0;
        public const long CNY = 99;
        public const long Jewel = 1000;
        public const long Besans = 1001;
        public const long HongBao = 1002;
    }

    public static class GoodsInfoTool
    {
        public static string GetGoodsIcon(long goodsId)
        {
            switch (goodsId)
            {
                case GoodsId.Jewel:
                    return "JewelIcon";
                case GoodsId.Besans:
                    return "BeansIcon";
                case GoodsId.HongBao:
                    return "HongBaoIcon";
            }
            Log.Error($"物品:{goodsId}没有对应的图标");
            return "";
        }

        public static string GetGoodsName(long goodsId)
        {
            switch (goodsId)
            {
                case GoodsId.Jewel:
                    return "钻石";
                case GoodsId.Besans:
                    return "豆子";
                case GoodsId.HongBao:
                    return "红包";
            }
            Log.Error($"物品:{goodsId}没有对应的名字");
            return "";
        }
    }
}
