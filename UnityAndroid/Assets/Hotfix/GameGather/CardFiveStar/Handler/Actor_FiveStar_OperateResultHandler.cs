using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_OperateResultHandler : AMHandler<Actor_FiveStar_OperateResult>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_OperateResult message)
        {
            CardFiveStarRoom.Ins.PlayerOperate(message.SeatIndex, message.OperateInfo);
        }
    }
}
