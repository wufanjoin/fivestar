using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static partial class MatchRoomComponentSystem
    {
        //玩家聊天
        public static bool UserChat(this MatchRoomComponent matchRoomComponent, long userId, ChatInfo chatInfo)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                return matchRoomComponent.UserIdInRoomIdDic[userId].UserChat(userId, chatInfo);//广播聊天信息
            }
            return false;
        }
    }
}