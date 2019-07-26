using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Collections;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 创建房间参数效验 并返回需要的钻石
    /// </summary>
    public static class RoomConfigIntended
    {
        private static Dictionary<long, CardFiveStarRoomConfig> _cardFiveStarRoomConfigDic;
        private static RepeatedField<int> DefaultCardFiveStarConfigLists = new RepeatedField<int>() { 1, 8, 30, 1, 3, 3, 2, 1, 0 };

        private static void InitCardFiveStarRoomConfigDic()
        {
            IConfig[] cardFiveStarRoomConfigs = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(CardFiveStarRoomConfig));
            _cardFiveStarRoomConfigDic = new Dictionary<long, CardFiveStarRoomConfig>();
            for (int i = 0; i < cardFiveStarRoomConfigs.Length; i++)
            {
                _cardFiveStarRoomConfigDic.Add(cardFiveStarRoomConfigs[i].Id, cardFiveStarRoomConfigs[i] as CardFiveStarRoomConfig);
            }
        }

        private static int defaulNumber;
        private static int defaulneedJeweNumCount;
        private static bool isAADeductJewel;
        public static bool IntendedRoomConfigParameter(RepeatedField<int> roomConfigLists, long toyGameId)
        {
            return IntendedRoomConfigParameter(roomConfigLists, toyGameId,ref defaulneedJeweNumCount, ref defaulNumber,ref isAADeductJewel);
        }
        public static bool IntendedRoomConfigParameter(RepeatedField<int> roomConfigLists, long toyGameId,ref int needJeweNumCount,ref int number,ref bool isAADeductJewel)
        {
            if (_cardFiveStarRoomConfigDic == null)
            {
                InitCardFiveStarRoomConfigDic();
            }
            needJeweNumCount = 0;
            switch (toyGameId)
            {
                case ToyGameId.CardFiveStar:
                    //参数的数量不等于规定的数量
                    if (roomConfigLists==null||roomConfigLists.Count != CardFiveStarRoomConfig.ConfigCount)
                    {
                        roomConfigLists = DefaultCardFiveStarConfigLists;
                    }
                    for (int i = 0; i < roomConfigLists.Count; i++)
                    {
                        roomConfigLists[i] = _cardFiveStarRoomConfigDic[i].IsValueExist(roomConfigLists[i], ref needJeweNumCount);//修改不正常的参数 和获取需要的钻石数
                    }
                    int RepetitionDectJewel = 0;
                    switch (roomConfigLists[CardFiveStarRoomConfig.EndTypeId])
                    {
                        case FiveStarEndType.FenTuoDi:
                            _cardFiveStarRoomConfigDic[CardFiveStarRoomConfig.JuShuId]
                                .IsValueExist(roomConfigLists[CardFiveStarRoomConfig.JuShuId], ref RepetitionDectJewel);
                            break;
                        case FiveStarEndType.JuShu:
                            _cardFiveStarRoomConfigDic[CardFiveStarRoomConfig.FenTuoDiId]
                                .IsValueExist(roomConfigLists[CardFiveStarRoomConfig.FenTuoDiId], ref RepetitionDectJewel);
                            break;
                    }
                    needJeweNumCount -= RepetitionDectJewel;
                    isAADeductJewel = roomConfigLists[CardFiveStarRoomConfig.PayMoneyId] == FiveStarPayMoneyType.JunTan;
                    if (roomConfigLists[CardFiveStarRoomConfig.PayMoneyId] == FiveStarPayMoneyType.JunTan)
                    {
                        needJeweNumCount =
                            (int)Math.Ceiling(needJeweNumCount * 1.00f /
                                              roomConfigLists[CardFiveStarRoomConfig.NumberId]);//如果是均摊 就要向上取整
                    }
                    number = roomConfigLists[CardFiveStarRoomConfig.NumberId];
                    return true;
                default:
                    Log.Error("创建房间的游戏类型暂不支持:" + toyGameId);
                    return false;
            }
        }
    }
}
