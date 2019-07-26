using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询指定亲友圈信息
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetFriendsCircleInfoHandler : AMRpcHandler<C2F_GetFriendsCircleInfo, F2C_GetFriendsCircleInfo>
    {
        protected override async void Run(Session session, C2F_GetFriendsCircleInfo message, Action<F2C_GetFriendsCircleInfo> reply)
        {
            F2C_GetFriendsCircleInfo response = new F2C_GetFriendsCircleInfo();
            try
            {
                for (int i = 0; i < message.FriendsCrircleIds.Count; i++)
                {
                    FriendsCircle friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleIds[i]);
                    if (friendsCircle != null)
                    {
                        response.FrienCircleInfos.Add(friendsCircle);
                    }
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
