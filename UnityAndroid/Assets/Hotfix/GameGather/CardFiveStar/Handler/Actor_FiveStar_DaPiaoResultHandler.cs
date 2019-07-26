using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_DaPiaoResultHandler : AMHandler<Actor_FiveStar_DaPiaoResult>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_DaPiaoResult message)
        {
            CardFiveStarRoom.Ins.PlayerDaPiao(message.SeatIndex, message.SelectPiaoNum);
        }
    }
}
