using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_SmallStartGameHandler : AMHandler<Actor_FiveStar_SmallStartGame>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_SmallStartGame message)
        {
            CardFiveStarRoom.Ins.SmallStarGame(message);
        }
    }
}
