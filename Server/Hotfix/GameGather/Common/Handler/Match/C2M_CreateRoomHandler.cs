using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class C2M_CreateRoomHandler : AMRpcHandler<C2M_CreateRoom, M2C_CreateRoom>
    {
        protected override async void Run(Session session, C2M_CreateRoom message, Action<M2C_CreateRoom> reply)
        {
            M2C_CreateRoom response = new M2C_CreateRoom();
            try
            {
                MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                //判断玩家 是否在其他游戏中
                if (matchRoomComponent.JudgeUserIsGameIn(message.UserId, message.SessionActorId))
                {
                    return;
                }
                int needJeweNumCount = 0;
                long userJewel = message.User.Jewel;
                User friendsCreateUser=null;
                if (message.FriendsCircleId > 0)
                {
                    friendsCreateUser=await  GetFriendsCircleCreateUser(message.FriendsCircleId, response);
                    if (friendsCreateUser == null)
                    {
                        reply(response);
                        return;
                    }
                    userJewel = friendsCreateUser.Jewel;
                }
                MatchRoom room = matchRoomComponent.CreateRoom(message.RoomConfigLists, message.FriendsCircleId, message.ToyGameId, userJewel, response);
                if (room == null)
                {
                    reply(response);
                    return;
                }
                if (friendsCreateUser != null)
                {
                    //记录亲友圈 主人的id
                    room.FriendsCreateUserId = friendsCreateUser.UserId;
                }
                
                //创建者加入房间
                matchRoomComponent.JoinRoom(room.RoomId, message.User, message.SessionActorId, response);
                response.RoomInfo = RoomInfoFactory.Creator(matchRoomComponent.GetRoomUserIdIn(message.UserId));
                reply(response);
                RoomInfoFactory.Destroy(response.RoomInfo);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        //查询亲友创建者User
        public static async Task<User>  GetFriendsCircleCreateUser(int friendsCircleId, IResponse iResponse)
        {
            List<FriendsCircle> friendsCircles = await Game.Scene.GetComponent<DBProxyComponent>().Query<FriendsCircle>(friends => (friends.FriendsCircleId == friendsCircleId));
            if (friendsCircles.Count <= 0)
            {
                iResponse.Message = "亲友圈不存在";
                return null;
            }
            return await UserHelp.QueryUserInfo(friendsCircles[0].CreateUserId);
        }


    }
}