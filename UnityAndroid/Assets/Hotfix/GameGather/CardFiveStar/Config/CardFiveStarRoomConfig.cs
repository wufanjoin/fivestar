using System;
using System.Collections.Generic;
using System.Reflection;
using ETModel;

namespace ETHotfix
{
    public class CreatorRoomOptionType
    {
        public const int SingleSelect=1;//单选
        public const int MoreSelect = 2;//多选
    }

   [Config((int)(AppType.ClientM | AppType.ClientH | AppType.Match | AppType.CardFiveStar))]
    public partial class CardFiveStarRoomConfigCategory : ACategory<CardFiveStarRoomConfig>
    {
    }

    public class CardFiveStarRoomConfig : IConfig
    {
        public const int ConfigCount = 11;//配置条数
        public const int EndTypeId = 0;//结束方式ID
        public const int JuShuId = 1;//局数ID
        public const int FenTuoDiId = 2;//分拖底ID
        public const int PayMoneyId = 3;//支付房费方式ID
        public const int NumberId = 4;//人数ID
        public const int BottomScoreId = 5;//底分ID
        public const int FloatNumId = 6;//漂数Id
        public const int MaiMaId = 7;//买马ID
        public const int WaiShiWuId = 8;//外十五ID
        public const int FengDingFanShuId = 9;//封顶番数
        public const int IsHaveOverTimeId = 10;//是否有超时
        public long Id { get; set; }
        public string Title;
        public List<RoomSingleSelectConfig> SelectConfigs;
        public int OptionType = CreatorRoomOptionType.SingleSelect;
        public int DefaultValue;

        private Dictionary<int,int> selectValuesInNeedJewelNum;
        public int IsValueExist(int value,ref int needJeweNum)
        {
            if (selectValuesInNeedJewelNum == null)
            {
                selectValuesInNeedJewelNum = new Dictionary<int, int>();
                for (int i = 0; i < SelectConfigs.Count; i++)
                {
                    selectValuesInNeedJewelNum.Add(SelectConfigs[i].Value, SelectConfigs[i].NeedJewelNum);
                }
            }
            if (selectValuesInNeedJewelNum.ContainsKey(value))
            {
                needJeweNum += selectValuesInNeedJewelNum[value];
                return value;
            }
            needJeweNum += selectValuesInNeedJewelNum[DefaultValue];
            return DefaultValue;
        }

        //根据实值 获得描述
        public string GetDescValueIn(int value)
        {
            for (int i = 0; i < SelectConfigs.Count; i++)
            {
                if (SelectConfigs[i].Value == value)
                {
                    return SelectConfigs[i].Describe;
                }
            }
            return string.Empty;
        }

        //根据配置id和实值value 获得描述
        public static string GetDescIdAndValueIn(int id,int value)
        {
            CardFiveStarRoomConfig cardFiveStarRoom = (CardFiveStarRoomConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(CardFiveStarRoomConfig), id);
            return cardFiveStarRoom.GetDescValueIn(value);
        }
    }

    public class RoomSingleSelectConfig
    {
        public int Value;
        public string Describe;
        public int NeedJewelNum;
    }
}
