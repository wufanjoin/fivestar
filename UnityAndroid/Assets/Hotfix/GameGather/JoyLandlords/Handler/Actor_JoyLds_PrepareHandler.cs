using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_PrepareHandler : AMHandler<Actor_JoyLds_Prepare>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_Prepare message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.Prepare, message.SeatIndex);
        }
    }
}
