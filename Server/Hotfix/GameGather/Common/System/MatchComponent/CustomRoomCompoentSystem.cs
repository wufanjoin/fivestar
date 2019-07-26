using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static partial class MatchRoomSystem
    {
        //创建房间
        public static MatchRoom CreateRoom(this MatchRoomComponent matchRoomComponent, RepeatedField<int> roomConfigLists,int friendsCircleId, long toyGameId, long userJewelNum, IResponse iResponse)
        {
            MatchRoom matchRoom=MatchRoomFactory.Create(roomConfigLists, toyGameId, matchRoomComponent.RandomRoomId(), friendsCircleId, userJewelNum, iResponse);
            if (matchRoom == null)
            {
                return null;//为空表示创建房间失败
            }
            matchRoomComponent.MatchRoomDic.Add(matchRoom.RoomId, matchRoom);
            matchRoomComponent.AddFriendsCircleRoom(matchRoom);
            return matchRoom;
        }
        //添加亲友圈房间房间
        public static void AddFriendsCircleRoom(this MatchRoomComponent matchRoomComponent, MatchRoom matchRoom)
        {
            if (matchRoom.FriendsCircleId != 0)
            {
                if (matchRoomComponent.FriendsCircleInMatchRoomDic.ContainsKey(matchRoom.FriendsCircleId))
                {
                    matchRoomComponent.FriendsCircleInMatchRoomDic[matchRoom.FriendsCircleId].Add(matchRoom);
                }
                else
                {
                    matchRoomComponent.FriendsCircleInMatchRoomDic[matchRoom.FriendsCircleId]=new List<MatchRoom>(){ matchRoom };
                }
            }
        }
        //亲友房间结束
        public static void DestroyFriendsCircleRoom(this MatchRoomComponent matchRoomComponent, MatchRoom matchRoom)
        {
            if (matchRoom.FriendsCircleId != 0)
            {
                if (matchRoomComponent.FriendsCircleInMatchRoomDic.ContainsKey(matchRoom.FriendsCircleId))
                {
                    if (matchRoomComponent.FriendsCircleInMatchRoomDic[matchRoom.FriendsCircleId].Contains(matchRoom))
                    {
                        matchRoomComponent.FriendsCircleInMatchRoomDic[matchRoom.FriendsCircleId].Remove(matchRoom);
                    }
                }
            }
        }
        //玩家加入房间
        public static void JoinRoom(this MatchRoomComponent matchRoomComponent,int roomId, User user, long sessionActorId, IResponse iResponse)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(user.UserId))
            {
                //玩家已经在其他房间中
                iResponse.Message = "正在其他房间游戏中";
                return ;
            }

            MatchRoom matchRoom = matchRoomComponent.GetRoom(roomId);
            if (matchRoom != null)
            {
                if (matchRoom.UserJoinRoom(user, sessionActorId))
                {
                    //玩家成功加入
                    matchRoomComponent.UserIdInRoomIdDic[user.UserId] = matchRoom;
                    user.AddComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
                    iResponse.Message = string.Empty;
                    return ;
                }
                else
                {
                    //玩家加入失败 房间已满
                    iResponse.Message = "房间已满";
                    return ;
                }
            }
            //房间不存在
            iResponse.Message = "房间不存在";
        }
        //玩家退出房间
        public static void OutRoom(this MatchRoomComponent matchRoomComponent,long userId, IResponse iResponse)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                if (matchRoomComponent.UserIdInRoomIdDic[userId].UserOutRoom(userId))
                {
                    if (matchRoomComponent.UserIdInRoomIdDic[userId].RoomType != RoomType.Match)
                    {
                        matchRoomComponent.UserIdInRoomIdDic.Remove(userId);//如果在随机匹配房间 状态保留 只能等房间结束
                    }
                    //成功退出房间
                    iResponse.Message = string.Empty;
                }
            }
            else
            {
                //玩家根本不在任何一个房间中
                iResponse.Message = "玩家不再任何房间";
            }
        }

        //玩家上线
        public static void PlayerOnLine(this MatchRoomComponent matchRoomComponent, long userId,long sessionActorId)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                matchRoomComponent.UserIdInRoomIdDic[userId].PlayerOnLine(userId, sessionActorId);
            }
        }

        //玩家下线
        public static void PlayerOffline(this MatchRoomComponent matchRoomComponent, long userId)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                matchRoomComponent.UserIdInRoomIdDic[userId].PlayerOffline(userId);
            }
        }

        //检测一个房间是否能开始游戏
        public static void DetetionRoomStartGame(this MatchRoomComponent matchRoomComponent, int roomId)
        {
            MatchRoom matchRoom=matchRoomComponent.GetRoom(roomId);
            if (matchRoom!=null&& !matchRoom.IsGameBeing&& matchRoom.DetetionMayStartGame())
            {
                matchRoom.StartGame();//开始游戏
            }
        }
        //得到房间
        public static MatchRoom GetRoom(this MatchRoomComponent matchRoomComponent, int roomId)
        {
            MatchRoom matchRoom;
            if (matchRoomComponent.MatchRoomDic.TryGetValue(roomId, out matchRoom))
            {

            }
            return matchRoom;
        }
        //得到房间
        public static MatchRoom GetRoomUserIdIn(this MatchRoomComponent matchRoomComponent, long userId)
        {
            MatchRoom matchRoom;
            if (matchRoomComponent.UserIdInRoomIdDic.TryGetValue(userId, out matchRoom))
            {

            }
            return matchRoom;
        }
        //移除房间
        public static void RemoveRoom(this MatchRoomComponent matchRoomComponent, int roomId)
        {
            if (matchRoomComponent.MatchRoomDic.ContainsKey(roomId))
            {
                foreach (var playerInfo in matchRoomComponent.MatchRoomDic[roomId].PlayerInfoDic)
                {
                    if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(playerInfo.Value.User.UserId))
                    {
                        matchRoomComponent.UserIdInRoomIdDic.Remove(playerInfo.Value.User.UserId);//移除所有房间里的玩家
                    }
                }
                MatchRoom matchRoom=matchRoomComponent.MatchRoomDic[roomId];//得到该房间信息实体
                matchRoomComponent.DestroyFriendsCircleRoom(matchRoom);//如果是亲友圈房间 还要从亲友圈中移除
                matchRoom.Dispose();//销毁房间
                matchRoomComponent.MatchRoomDic.Remove(roomId);//从列表里移除房间
            }
            else
            {
                Log.Error("要移除房间不存在:"+ roomId);
            }
        }
       

        //随机一个房间号 不与之前的重复
        public static int RandomRoomId(this MatchRoomComponent matchRoomComponent)
        {
            int roomId = RandomTool.Random(10000, 99999);
            while (matchRoomComponent.MatchRoomDic.ContainsKey(roomId))
            {
                roomId=RandomTool.Random(10000, 99999);
                Log.Info("随机生成房间号:"+ roomId);
            }
            return roomId;
        }

        //判断用户在不在游戏 如果在 就通知客户端玩家在游戏中
        public static bool JudgeUserIsGameIn(this MatchRoomComponent matchRoomComponent,long userId,long userSessionActorId)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                ActorHelp.SendeActor(userSessionActorId, new Actor_BeingInGame() { IsGameBeing = true });//通知客户端 用户在游戏中
                return true;
            }
            return false;
        }
    }
}