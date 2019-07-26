using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_CanOperateHandler : AMHandler<Actor_FiveStar_CanOperate>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_CanOperate message)
        {
            CardFiveStarRoom.Ins.PlayerCanOperate(message.SeatIndex,message.CanOperateLits, message.CanGangLits);
        }
    }
}
