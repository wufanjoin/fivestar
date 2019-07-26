using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家退出亲友圈
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_OutFriendsCircleHandler : AMRpcHandler<C2F_OutFriendsCircle, F2C_OutFriendsCircle>
    {
        protected override async void Run(Session session, C2F_OutFriendsCircle message, Action<F2C_OutFriendsCircle> reply)
        {
            F2C_OutFriendsCircle response = new F2C_OutFriendsCircle();
            try
            {
                FriendsCircle friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                if (friendsCircle.CreateUserId == message.UserId)
                {
                    response.Message = "创建者不能退出亲友圈";
                    reply(response);
                    return;
                }
                friendsCircle.SucceedOutriendsCircle(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
