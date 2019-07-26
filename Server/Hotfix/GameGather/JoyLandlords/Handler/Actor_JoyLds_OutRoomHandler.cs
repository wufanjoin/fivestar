using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家退出房间
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_OutRoomHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_OutRoom>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_OutRoom message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.PlayerOutRoom();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            
        }
    }
}
