using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_MoPaiHandler : AMHandler<Actor_FiveStar_MoPai>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_MoPai message)
        {
            CardFiveStarRoom.Ins.MoCard(message.SeatIndex, message.Card);
        }
    }
}
