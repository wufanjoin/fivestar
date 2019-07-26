using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_CanAddTwiceHandler : AMHandler<Actor_JoyLds_CanAddTwice>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_CanAddTwice message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.CanAddTwice);
        }
    }
}
