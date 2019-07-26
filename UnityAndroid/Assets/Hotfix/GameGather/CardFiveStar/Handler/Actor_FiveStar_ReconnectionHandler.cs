using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 游戏重连
    /// </summary>
    [MessageHandler]
    public class Actor_FiveStar_ReconnectionHandler : AMHandler<Actor_FiveStar_Reconnection>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_Reconnection message)
        {
            CardFiveStarAisle.Reconnection(message);
        }
    }
}
