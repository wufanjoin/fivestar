using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{ 
   
 public static class MatchRoomFactory
 {

     //创建房卡匹房间
        public static MatchRoom Create(RepeatedField<int> roomConfigLists,long toyGameId,int roomId,int friendsCircleId,long userJewelNum,IResponse iResponse)
        {
            int needJeweNumCount = 0;
            int number = 0;
            bool isAADeductJewel = false;
            //效验房间配置正不正确 不正确的值会修改为默认值  会赋值 需要的钻石数 和房间配置人数
            if (!RoomConfigIntended.IntendedRoomConfigParameter(roomConfigLists, toyGameId, ref needJeweNumCount,
                ref number,ref isAADeductJewel))
            {
                iResponse.Message = "房间配置表错误";
                return null;
            }

            //如果是亲友圈房间亲友圈的 圈主钻石数要大于100
            if (friendsCircleId > 0 && userJewelNum < 100)
            {
                iResponse.Message = "亲友圈主人钻石不足100";
                return null;
            }
            if (userJewelNum < needJeweNumCount)
            {
                iResponse.Message = "钻石不足";
                return null;
            }
            MatchRoom matchRoom = ComponentFactory.Create<MatchRoom>();
            MatchRoomConfig roomConfig = MatchRoomConfigFactory.Create(number, roomId, toyGameId, roomConfigLists);
            matchRoom.RoomId = roomId;
            matchRoom.FriendsCircleId = friendsCircleId;
            matchRoom.RoomConfig = roomConfig;
            matchRoom.RoomType = RoomType.RoomCard;
            matchRoom.NeedJeweNumCount = needJeweNumCount;
            matchRoom.IsAADeductJewel = isAADeductJewel;
            return matchRoom;
        }

        //创建随机匹配房间
     public static MatchRoom CreateRandomMatchRoom(long toyGameId, int roomId,RepeatedField<MatchPlayerInfo> matchPlayerInfos, MatchRoomConfig matchRoomConfig)
     {
         try
         {
             MatchRoom matchRoom = ComponentFactory.Create<MatchRoom>();
             matchRoom.RoomId = roomId;
             matchRoom.RoomType = RoomType.Match;
             matchRoom.RoomConfig = matchRoomConfig;

             for (int i = 0; i < matchPlayerInfos.Count; i++)
             {
                 matchRoom.PlayerInfoDic[matchPlayerInfos[i].SeatIndex] = matchPlayerInfos[i];
             }
             return matchRoom;
            }
         catch (Exception e)
         {
             Log.Error(e);
             throw;
         }

     }
    }
}
