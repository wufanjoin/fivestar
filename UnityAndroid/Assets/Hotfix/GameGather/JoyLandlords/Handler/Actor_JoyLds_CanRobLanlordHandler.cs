using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_CanRobLanlordHandler : AMHandler<Actor_JoyLds_CanRobLanlord>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_CanRobLanlord message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.CanRobLanlord, message.SeatIndex);
        }
    }
}
