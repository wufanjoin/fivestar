using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 修改亲友圈是否推荐给陌生人
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_AlterIsRecommendHandler : AMRpcHandler<C2F_AlterIsRecommend, F2C_AlterIsRecommend>
    {
        protected override async void Run(Session session, C2F_AlterIsRecommend message, Action<F2C_AlterIsRecommend> reply)
        {
            F2C_AlterIsRecommend response = new F2C_AlterIsRecommend();
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
                friendsCircle.AlterIsRecommend(message.IsRecommend);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
