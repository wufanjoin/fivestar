using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_AnewDealHandler : AMHandler<Actor_JoyLds_AnewDeal>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_AnewDeal message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.AnewDeal);
        }
    }
}
