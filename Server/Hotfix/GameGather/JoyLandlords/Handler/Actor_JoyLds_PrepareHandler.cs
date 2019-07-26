using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家准备
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_PrepareHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_Prepare>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_Prepare message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.PlayerPrepare(joyLdsPlayer.pSeatIndex);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            
        }
    }
}
