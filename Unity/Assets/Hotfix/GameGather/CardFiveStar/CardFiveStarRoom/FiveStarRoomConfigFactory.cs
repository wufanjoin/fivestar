using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class FiveStarRoomConfigFactory
    {
        public static FiveStarRoomConfig Create(RepeatedField<int> roomConfigs)
        {
            FiveStarRoomConfig fiveStarRoom = ComponentFactory.Create<FiveStarRoomConfig>();
            if (roomConfigs.Count != CardFiveStarRoomConfig.ConfigCount)
            {
                Log.Error("房间配置的条数不一致"+ roomConfigs.Count);
                return null;
            }
            fiveStarRoom.Configs = roomConfigs;
            fiveStarRoom.EndType = roomConfigs[CardFiveStarRoomConfig.EndTypeId];
            fiveStarRoom.MaxJuShu = roomConfigs[CardFiveStarRoomConfig.JuShuId];
            fiveStarRoom.TuoDiFen = roomConfigs[CardFiveStarRoomConfig.FenTuoDiId];
            fiveStarRoom.PayMoneyType = roomConfigs[CardFiveStarRoomConfig.PayMoneyId];
            fiveStarRoom.RoomNumber = roomConfigs[CardFiveStarRoomConfig.NumberId];
            fiveStarRoom.BottomScore = roomConfigs[CardFiveStarRoomConfig.BottomScoreId];
            fiveStarRoom.MaxPiaoNum = roomConfigs[CardFiveStarRoomConfig.FloatNumId];
            fiveStarRoom.MaiMaType = roomConfigs[CardFiveStarRoomConfig.MaiMaId];
            fiveStarRoom.WaiShiWuType = roomConfigs[CardFiveStarRoomConfig.WaiShiWuId];
            fiveStarRoom.FengDingFanShu = roomConfigs[CardFiveStarRoomConfig.FengDingFanShuId];
            fiveStarRoom.IsHaveOverTime = roomConfigs[CardFiveStarRoomConfig.IsHaveOverTimeId]!=0;
            return fiveStarRoom;
        }
    }
}
