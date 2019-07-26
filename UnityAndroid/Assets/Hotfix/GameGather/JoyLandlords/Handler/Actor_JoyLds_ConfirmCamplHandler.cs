using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_ConfirmCampHandler : AMHandler<Actor_JoyLds_ConfirmCamp>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_ConfirmCamp message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.ConfirmCamp, message.LandlordSeatIndex, message.Hands, message.LandlordThreeCard);
        }
    }
}
