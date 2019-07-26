using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_CanDaPiaoHandler : AMHandler<Actor_FiveStar_CanDaPiao>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_CanDaPiao message)
        {
            CardFiveStarRoom.Ins.CanDaPiao(message.MaxPiaoNum);
        }
    }
}
