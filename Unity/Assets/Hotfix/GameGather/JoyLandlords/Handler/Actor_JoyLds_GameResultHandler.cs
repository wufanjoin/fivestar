using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_GameResultHandler : AMHandler<Actor_JoyLds_GameResult>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_GameResult message)
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.GameResult, message.PlayerResults);
        }
    }
}
