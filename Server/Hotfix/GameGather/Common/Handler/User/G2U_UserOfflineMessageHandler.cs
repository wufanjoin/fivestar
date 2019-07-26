using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix.GameGather.Common.Handler.User
{
    [MessageHandler(AppType.User)]
 public class G2U_UserOfflineMessageHandler : AMHandler<G2S_UserOffline>
    {
        protected override void Run(Session session, G2S_UserOffline message)
        {
            try
            {
                Game.Scene.GetComponent<UserComponent>().UserOffline(message.UserId);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
