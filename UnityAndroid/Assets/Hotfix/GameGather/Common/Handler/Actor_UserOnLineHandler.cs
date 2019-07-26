using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家上线
    /// </summary>
    [MessageHandler]
    public class Actor_UserOnLineHandler : AMHandler<Actor_UserOnLine>
    {
        protected override void Run(ETModel.Session session, Actor_UserOnLine message)
        {
            EventMsgMgr.SendEvent(CommEventID.UserOnLine, message.UserId);
        }
    }
}
