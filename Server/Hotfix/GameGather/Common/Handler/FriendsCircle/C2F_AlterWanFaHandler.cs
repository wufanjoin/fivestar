using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 修改默认玩法
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_AlterWanFaHandler : AMRpcHandler<C2F_AlterWanFa, F2C_AlterWanFa>
    {
        protected override async void Run(Session session, C2F_AlterWanFa message, Action<F2C_AlterWanFa> reply)
        {
            F2C_AlterWanFa response = new F2C_AlterWanFa();
            try
            {
                FriendsCircle friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                if (!friendsCircle.ManageUserIds.Contains(message.UserId))
                {
                    response.Message = "管理权限不足";
                    reply(response);
                    return;
                }
                await friendsCircle.AlterWanFa(message.WanFaCofigs, message.ToyGameId, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
