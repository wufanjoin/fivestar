using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_StartGameHandler : AMHandler<Actor_FiveStar_StartGame>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_StartGame message)
        {
            CardFiveStarRoom.Ins._RoomId = message.RoomId;
            CardFiveStarRoom.Ins.SetConfigInfo(message.RoomConfigs);
            CardFiveStarRoom.Ins.ShowPlayerInfo(message.PlayerInfos);
        }
    }
}
