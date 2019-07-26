using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_DealHandler : AMHandler<Actor_JoyLds_Deal>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_Deal message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.Deal, message.Cards);
        }
    }
}
