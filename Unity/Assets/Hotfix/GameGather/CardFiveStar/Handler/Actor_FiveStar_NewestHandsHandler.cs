using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_NewestHandsHandler : AMHandler<Actor_FiveStar_NewestHands>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_NewestHands message)
        {
            CardFiveStarHandComponent.Ins.RefreshHand(message.Hands);
        }
    }
}
