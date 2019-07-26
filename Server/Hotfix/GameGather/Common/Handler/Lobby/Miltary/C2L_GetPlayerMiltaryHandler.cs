using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetPlayerMiltaryHandler : AMRpcHandler<C2L_GetPlayerMiltary, L2C_GetPlayerMiltary>
    {
        protected override async void Run(Session session, C2L_GetPlayerMiltary message, Action<L2C_GetPlayerMiltary> reply)
        {
            L2C_GetPlayerMiltary response = new L2C_GetPlayerMiltary();
            try
            {
                List < Miltary > miltaries= await MiltaryComponent.Ins.GetMiltary(message.QueryUserId, message.FriendCircleId);
                response.Miltarys.Add(miltaries.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
