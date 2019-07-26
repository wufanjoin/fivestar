using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_GetMatchRoomConfigsHandler : AMRpcHandler<C2L_GetMatchRoomConfigs, L2C_GetMatchRoomConfigs>
    {
        protected override void Run(Session session, C2L_GetMatchRoomConfigs message, Action<L2C_GetMatchRoomConfigs> reply)
        {
            L2C_GetMatchRoomConfigs response = new L2C_GetMatchRoomConfigs();
            try
            {
              List<MatchRoomConfig>  matchRoomConfigs=Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetMatachRoomConfigs(message.ToyGameId);
              response.MatchRoomConfigs.AddRange(matchRoomConfigs.ToArray());
              reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}