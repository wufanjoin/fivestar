using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 申请加入亲友圈
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_ApplyJoinFriendsCircleHandler : AMRpcHandler<C2F_ApplyJoinFriendsCircle, F2C_ApplyJoinFriendsCircle>
    {
        protected override async void Run(Session session, C2F_ApplyJoinFriendsCircle message, Action<F2C_ApplyJoinFriendsCircle> reply)
        {
            F2C_ApplyJoinFriendsCircle response = new F2C_ApplyJoinFriendsCircle();
            try
            {
                FriendsCircle friendsCircle=await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                await friendsCircle.ApplyJoinFriendsCircle(message.UserId, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
