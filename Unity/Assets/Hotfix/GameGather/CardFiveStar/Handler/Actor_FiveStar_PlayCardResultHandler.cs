using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_PlayCardResultHandler : AMHandler<Actor_FiveStar_PlayCardResult>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_PlayCardResult message)
        {
            CardFiveStarRoom.Ins.PlayerPlayCard(message.SeatIndex, message.Card);
        }
    }
}
