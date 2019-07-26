using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_CallLanlordHandler : AMHandler<Actor_JoyLds_CallLanlord>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_CallLanlord message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.CallLanlord, message.SeatIndex, message.Result);
        }
    }
}
