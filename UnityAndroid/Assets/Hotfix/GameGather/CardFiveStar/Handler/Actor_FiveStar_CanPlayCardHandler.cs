using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_CanPlayCardHandler : AMHandler<Actor_FiveStar_CanPlayCard>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_CanPlayCard message)
        {
            CardFiveStarRoom.Ins.PlayerCanPlayCard(message.SeatIndex);
        }
    }
}
