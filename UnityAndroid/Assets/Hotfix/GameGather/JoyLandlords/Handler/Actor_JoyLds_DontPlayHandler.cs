using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_DontPlayHandler : AMHandler<Actor_JoyLds_DontPlay>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_DontPlay message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.DontPlay, message.SeatIndex);
        }
    }
}
