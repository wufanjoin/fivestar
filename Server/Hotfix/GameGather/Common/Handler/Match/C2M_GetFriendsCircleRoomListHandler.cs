using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 请求所在亲友圈的所有房间列表
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class C2M_GetFriendsCircleRoomListHandler : AMRpcHandler<C2M_GetFriendsCircleRoomList, M2C_GetFriendsCircleRoomList>
    {
        protected override void Run(Session session, C2M_GetFriendsCircleRoomList message, Action<M2C_GetFriendsCircleRoomList> reply)
        {
            M2C_GetFriendsCircleRoomList response = new M2C_GetFriendsCircleRoomList();
            try
            {
                if (MatchRoomComponent.Ins.FriendsCircleInMatchRoomDic.ContainsKey(message.FriendsCircleId))
                {
                    response.RoomInfos=RoomInfoFactory.Creator(MatchRoomComponent.Ins.FriendsCircleInMatchRoomDic[message.FriendsCircleId]);
                }
                reply(response);
                RoomInfoFactory.Destroy(response.RoomInfos);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
