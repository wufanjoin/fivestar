using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 把人踢出亲友圈
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_KickOutFriendsCircleHandler : AMRpcHandler<C2F_KickOutFriendsCircle, F2C_KickOutFriendsCircle>
    {
        protected override async void Run(Session session, C2F_KickOutFriendsCircle message, Action<F2C_KickOutFriendsCircle> reply)
        {
            F2C_KickOutFriendsCircle response = new F2C_KickOutFriendsCircle();
            try
            {
                FriendsCircle friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                await friendsCircle.KickOutFriendsCircle(message.UserId, message.OperateUserId, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
