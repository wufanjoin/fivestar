using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_OutRoomHandler : AMHandler<Actor_JoyLds_OutRoom>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_OutRoom message)
        {
           //暂时 如果有一个玩家退出了 会直接返回解散房间的消息
        }
    }
}
