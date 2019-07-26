using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家叫地主
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_CallLanlordHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_CallLanlord>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_CallLanlord message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.PlayerCallLandlord(joyLdsPlayer.pSeatIndex, message.Result);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
           
        }
    }
}
