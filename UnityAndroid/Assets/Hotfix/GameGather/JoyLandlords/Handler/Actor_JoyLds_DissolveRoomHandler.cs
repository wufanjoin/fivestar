using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_DissolveRoomHandler : AMHandler<Actor_JoyLds_DissolveRoom>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_DissolveRoom message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.DissolveRoom);
        }
    }
}
