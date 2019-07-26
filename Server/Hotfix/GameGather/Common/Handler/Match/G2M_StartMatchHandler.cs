using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class G2M_StartMatchHandler : AMRpcHandler<C2M_StartMatch, M2C_StartMatch>
    {
        protected override  void Run(Session session, C2M_StartMatch message, Action<M2C_StartMatch> reply)
        {
            M2C_StartMatch response = new M2C_StartMatch();
            try
            {
                //判断玩家 是否在其他游戏中
                if (MatchRoomComponent.Ins.JudgeUserIsGameIn(message.UserId, message.SessionActorId))
                {
                    return;
                }
                message.User.AddComponent<UserGateActorIdComponent>().ActorId = message.SessionActorId;
                response.Error=Game.Scene.GetComponent<MatchRoomComponent>().JoinMatchQueue(message.MatchRoomId, message.User, response);
                reply(response);
                Game.Scene.GetComponent<MatchRoomComponent>().DetectionMatchCondition(message.MatchRoomId);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}