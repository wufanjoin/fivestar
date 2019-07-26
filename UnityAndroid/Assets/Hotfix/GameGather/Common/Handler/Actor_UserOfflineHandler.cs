using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家下线
    /// </summary>
    [MessageHandler]
    public class Actor_UserOfflineHandler : AMHandler<Actor_UserOffline>
    {
        protected override void Run(ETModel.Session session, Actor_UserOffline message)
        {
            EventMsgMgr.SendEvent(CommEventID.UserOffLine, message.UserId);//玩家下线
        }
    }
}
