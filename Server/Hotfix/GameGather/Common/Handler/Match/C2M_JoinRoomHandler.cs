using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class C2M_JoinRoomHandler : AMRpcHandler<C2M_JoinRoom, M2C_JoinRoom>
    {
        protected override async void Run(Session session, C2M_JoinRoom message, Action<M2C_JoinRoom> reply)
        {
            M2C_JoinRoom response = new M2C_JoinRoom();
            try
            {
                MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                //判断玩家 是否在其他游戏中
                if (matchRoomComponent.JudgeUserIsGameIn(message.UserId, message.SessionActorId))
                {
                    return;
                }
                MatchRoom matchRoom = matchRoomComponent.GetRoom(message.RoomId);
                if (matchRoom == null)
                {
                    response.Message = "房间不存在";
                    reply(response);
                    return;
                }
                if (matchRoom.IsAADeductJewel && matchRoom.NeedJeweNumCount > message.User.Jewel)
                {
                    response.Message = "钻石不足 无法加入";
                    reply(response);
                    return;
                }

                matchRoomComponent.JoinRoom(message.RoomId, message.User, message.SessionActorId, response);
                if (string.IsNullOrEmpty(response.Message))
                {
                    response.RoomInfo = RoomInfoFactory.Creator(matchRoomComponent.GetRoomUserIdIn(message.UserId));
                }
                reply(response);
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                matchRoomComponent.DetetionRoomStartGame(message.RoomId);
                RoomInfoFactory.Destroy(response.RoomInfo);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }


    }
}