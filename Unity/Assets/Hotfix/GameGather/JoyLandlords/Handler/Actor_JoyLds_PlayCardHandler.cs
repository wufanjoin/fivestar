using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_PlayCardHandler : AMHandler<Actor_JoyLds_PlayCard>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_PlayCard message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.PlayCard, message.SeatIndex,message.PlayCardType, message.Cards,message.Hands);
        }
    }
}
