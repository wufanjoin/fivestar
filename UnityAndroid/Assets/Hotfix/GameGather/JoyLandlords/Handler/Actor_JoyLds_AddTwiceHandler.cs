using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_AddTwiceHandler : AMHandler<Actor_JoyLds_AddTwice>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_AddTwice message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.AddTwice, message.SeatIndex,message.Result);
        }
    }
}
