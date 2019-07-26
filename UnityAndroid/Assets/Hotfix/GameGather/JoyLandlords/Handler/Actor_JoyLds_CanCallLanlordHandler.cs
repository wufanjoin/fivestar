using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_CanCallLanlordHandler : AMHandler<Actor_JoyLds_CanCallLanlord>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_CanCallLanlord message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.CanCallLanlord, message.SeatIndex);
        }
    }
}
