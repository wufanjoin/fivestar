using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家抢地主
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_RobLanlordHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_RobLanlord>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_RobLanlord message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.PlayerRobLandlord(joyLdsPlayer.pSeatIndex, message.Result);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            
        }
    }
}
