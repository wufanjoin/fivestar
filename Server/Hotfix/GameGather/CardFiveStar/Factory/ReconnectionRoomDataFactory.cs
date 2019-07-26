using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
  public  class ReconnectionRoomDataFactory
    {
        public static Actor_FiveStar_Reconnection Create(FiveStarRoom fiveStarRoom,long reconnectionUserId)
        {
            Actor_FiveStar_Reconnection actorFiveStarReconnection=new Actor_FiveStar_Reconnection();
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                actorFiveStarReconnection.Players.Add(FiveStarPlayerFactory.CopySerialize(fiveStarRoom.FiveStarPlayerDic[i])); 
            }
        
            actorFiveStarReconnection.RoomId = fiveStarRoom.RoomId;
            if (fiveStarRoom.RoomType == RoomType.Match)
            {
                actorFiveStarReconnection.RoomId = fiveStarRoom.MathRoomId;
            }
            actorFiveStarReconnection.FriendsCircleId = fiveStarRoom.FriendsCircleId;
            actorFiveStarReconnection.RoomType = fiveStarRoom.RoomType;
            actorFiveStarReconnection.Configs = fiveStarRoom.RoomConfig.Configs;
            actorFiveStarReconnection.CurrRestSeatIndex = fiveStarRoom.CurrRestSeatIndex;
            actorFiveStarReconnection.CurrRoomStateType = fiveStarRoom.CurrRoomStateType;
            actorFiveStarReconnection.CurrOfficNum = fiveStarRoom.CurrOfficNum;
            
            if (fiveStarRoom.ResidueCards != null)
            {
                actorFiveStarReconnection.ResidueCardCount = fiveStarRoom.ResidueCards.Count;
            }
            actorFiveStarReconnection.IsDaPiaoBeing= fiveStarRoom.IsDaPiaoBeing;
            actorFiveStarReconnection.EndPlayCardSize = fiveStarRoom.CurrChuPaiCard;//最后出第一张牌 就是当前出的牌
            for (int i = 0; i < actorFiveStarReconnection.Players.Count; i++)
            {
                if (reconnectionUserId == actorFiveStarReconnection.Players[i].User.UserId ||
                    actorFiveStarReconnection.Players[i].IsLiangDao)
                {
                    continue;
                }
                if (actorFiveStarReconnection.Players[i].Hands == null)
                {
                    continue;
                }
                fiveStarRoom.intData = actorFiveStarReconnection.Players[i].Hands.Count;
                actorFiveStarReconnection.Players[i].Hands = new RepeatedField<int>();
                actorFiveStarReconnection.Players[i].Hands.Add(fiveStarRoom.intData);//不是自己本人 也没有亮倒的手牌信息 只能得到数量
            }
            return actorFiveStarReconnection;
        }

        public static void Dispose(Actor_FiveStar_Reconnection actorFiveStarReconnection)
        {
            for (int i = 0; i < actorFiveStarReconnection.Players.Count; i++)
            {
                FiveStarPlayerFactory.DisposeSerializePlayer(actorFiveStarReconnection.Players[i]);
            }
        }
    }
}
