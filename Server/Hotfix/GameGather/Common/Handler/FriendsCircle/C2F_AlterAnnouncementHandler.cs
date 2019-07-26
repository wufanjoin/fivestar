using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 修改亲友圈公告
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_AlterAnnouncementHandler : AMRpcHandler<C2F_AlterAnnouncement, F2C_AlterAnnouncement>
    {
        protected override async void Run(Session session, C2F_AlterAnnouncement message, Action<F2C_AlterAnnouncement> reply)
        {
            F2C_AlterAnnouncement response = new F2C_AlterAnnouncement();
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
                friendsCircle.AlterAnnouncement(message.Announcement);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
