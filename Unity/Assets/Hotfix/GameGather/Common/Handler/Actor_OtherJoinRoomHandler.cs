using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_OtherJoinRoomHandler : AMHandler<Actor_OtherJoinRoom>
    {
        protected override void Run(ETModel.Session session, Actor_OtherJoinRoom message)
        {
            CardFiveStarRoom.Ins.ShowPlayerInfo(message.PlayerInfo.User, message.PlayerInfo.SeatIndex);//显示玩家信息
        }
    }
}
