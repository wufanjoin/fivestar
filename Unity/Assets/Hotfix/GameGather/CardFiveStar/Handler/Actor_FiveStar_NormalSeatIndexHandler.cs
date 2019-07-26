using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_NormalSeatIndexHandler : AMHandler<Actor_FiveStar_NormalSeatIndex>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_NormalSeatIndex message)
        {
            CardFiveStarRoom.Ins.NormalPlayerSeatIndex(message.NewIndexInUser);//玩家索引回归正常
        }
    }
}
