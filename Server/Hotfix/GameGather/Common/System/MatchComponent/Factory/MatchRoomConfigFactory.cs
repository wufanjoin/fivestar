using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
 public  static  class MatchRoomConfigFactory
    {
        public static MatchRoomConfig Create(int number,int  roomId,long toyGameId, RepeatedField<int> configs)
        {
            MatchRoomConfig matchRoomConfig=ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig.GameNumber = number;
            matchRoomConfig.RoomConfigs = configs;
            matchRoomConfig.MatchRoomId = roomId;
            matchRoomConfig.ToyGameId = toyGameId;
            return matchRoomConfig;
        }
    }
}
