using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_PlayerReadyHandler : AMHandler<Actor_FiveStar_PlayerReady>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_PlayerReady message)
        {
            CardFiveStarRoom.Ins.PlayerReady(message.SeatIndex);
        }
    }
}
