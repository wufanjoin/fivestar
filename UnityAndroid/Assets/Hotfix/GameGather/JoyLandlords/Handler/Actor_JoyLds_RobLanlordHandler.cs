using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_RobLanlordHandler : AMHandler<Actor_JoyLds_RobLanlord>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_RobLanlord message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.RobLanlord, message.SeatIndex, message.Result);
        }
    }
}
