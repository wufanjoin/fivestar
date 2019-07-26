using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    /// <summary>
    /// 玩家请求断线重连数据
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class C2M_GetReconnectionRoomInfoHandler : AMRpcHandler<C2M_GetReconnectionRoomInfo, M2C_GetReconnectionRoomInfo>
    {
        protected override void Run(Session session, C2M_GetReconnectionRoomInfo message, Action<M2C_GetReconnectionRoomInfo> reply)
        {
            M2C_GetReconnectionRoomInfo response = new M2C_GetReconnectionRoomInfo();
            try
            {
                MatchRoom matchRoom=MatchRoomComponent.Ins.GetRoomUserIdIn(message.UserId);
                if (matchRoom == null)
                {
                    response.IsGameBeing = false;
                    response.Message = "房间已经结算";
                    reply(response);
                    return;
                }
                response.IsGameBeing = true;
                if (matchRoom.IsGameBeing)
                {
                    Actor_UserRequestReconnectionRoom m2SUserRequest = new Actor_UserRequestReconnectionRoom();
                    m2SUserRequest.UserId = message.UserId;
                    m2SUserRequest.UserActorId=matchRoom.GetPlayerInfo(message.UserId).SessionActorId;
                    ActorHelp.SendeActor(matchRoom.GameServeRoomActorId, m2SUserRequest);
                    if (matchRoom.IsVoteDissolveIn)//如果正在投票解散中 补发一条投票结果消息
                    {
                        ActorHelp.SendeActor(m2SUserRequest.UserActorId, matchRoom.VoteDissolveResult);
                    }
                }
                else
                {
                    response.RoomInfos=RoomInfoFactory.Creator(matchRoom);
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}