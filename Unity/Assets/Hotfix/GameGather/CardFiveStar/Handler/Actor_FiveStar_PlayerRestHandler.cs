using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_PlayerRestHandler : AMHandler<Actor_FiveStar_PlayerRest>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_PlayerRest message)
        {
            CardFiveStarRoom.Ins.PlayerRest(message.RestSeatIndex);//玩家轮休 索引更改
        }
    }
}
