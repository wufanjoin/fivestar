using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_OtherOutRoomHandler : AMHandler<Actor_OtherOutRoom>
    {
        protected override void Run(ETModel.Session session, Actor_OtherOutRoom message)
        {
            CardFiveStarRoom.Ins.PlayerOutRoom(message.UserId);//显示玩家信息
        }
    }
}
