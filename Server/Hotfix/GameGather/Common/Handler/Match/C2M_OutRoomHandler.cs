using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class C2M_OutRoomHandler : AMRpcHandler<C2M_OutRoom, M2C_OutRoom>
    {
        protected override void Run(Session session, C2M_OutRoom message, Action<M2C_OutRoom> reply)
        {
            M2C_OutRoom response = new M2C_OutRoom();
            try
            {
                MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                matchRoomComponent.OutRoom(message.UserId, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }


    }
}