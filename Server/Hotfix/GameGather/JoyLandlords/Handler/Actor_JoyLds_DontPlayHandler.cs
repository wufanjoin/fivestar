using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家不出牌
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_DontPlayHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_DontPlay>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_DontPlay message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.DontPlay(joyLdsPlayer.pSeatIndex);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            
        }
    }
}
