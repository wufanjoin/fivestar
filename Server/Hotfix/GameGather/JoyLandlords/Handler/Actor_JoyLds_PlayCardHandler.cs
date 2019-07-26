using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家出牌结果
    /// </summary>
    [ActorMessageHandler(AppType.JoyLandlords)]
    public class Actor_JoyLds_PlayCardHandler : AMActorHandler<JoyLdsPlayer, Actor_JoyLds_PlayCard>
    {
        protected override void Run(JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_PlayCard message)
        {
            try
            {
                joyLdsPlayer.pJoyLdsRoom.PlayCard(joyLdsPlayer.pSeatIndex, message.Cards);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            

        }
    }
}
