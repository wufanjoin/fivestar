using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_CanPlayCardHandler : AMHandler<Actor_JoyLds_CanPlayCard>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_CanPlayCard message)
        {
            
            EventMsgMgr.SendEvent(JoyLandlordsEventID.CanPlayCard, message.SeatIndex, message.IsFirst);
        }
    }
}
