using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 处理申请列表
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_DisposeApplyInfoHandler : AMRpcHandler<C2F_DisposeApplyInfo, F2C_DisposeApplyInfo>
    {
        protected override async void Run(Session session, C2F_DisposeApplyInfo message, Action<F2C_DisposeApplyInfo> reply)
        {
            F2C_DisposeApplyInfo response = new F2C_DisposeApplyInfo();
            try
            {
                FriendsCircle friendsCircle =
                    await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle==null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                if (!friendsCircle.ManageUserIds.Contains(message.UserId))
                {
                    response.Message = "您的管理权限不足";
                    reply(response);
                    return;
                }
                await  friendsCircle.DisposeApplyResult(message.ApplyUserId, message.IsConsent, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
