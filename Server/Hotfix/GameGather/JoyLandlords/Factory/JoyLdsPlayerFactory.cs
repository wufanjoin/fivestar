using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
   public static class JoyLdsPlayerFactory
    {
        public  static async Task<JoyLdsPlayer>  Create(MatchPlayerInfo matchPlayer, JoyLdsRoom joyLdsRoom)
        {
            JoyLdsPlayer joyLdsPlayer = ComponentFactory.Create<JoyLdsPlayer>();
            joyLdsPlayer.pSeatIndex = matchPlayer.SeatIndex;
            joyLdsPlayer.pUser = matchPlayer.User;
            joyLdsPlayer.pUser.AddComponent<UserGateActorIdComponent>().ActorId = matchPlayer.SessionActorId;
            joyLdsPlayer.pJoyLdsRoom = joyLdsRoom;
            //添加收取Actor消息组件 并且本地化一下 就是所有服务器都能向这个对象发 
            await joyLdsPlayer.AddComponent<MailBoxComponent>().AddLocation();
            return joyLdsPlayer;
        }
    }
}
