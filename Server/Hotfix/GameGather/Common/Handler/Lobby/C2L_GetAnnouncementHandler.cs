using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetAnnouncementHandler : AMRpcHandler<C2L_GetAnnouncement, L2C_GetAnnouncement>
    {
        protected override void Run(Session session, C2L_GetAnnouncement message, Action<L2C_GetAnnouncement> reply)
        {
            L2C_GetAnnouncement response = new L2C_GetAnnouncement();
            try
            {
                response.Message = "本游戏仅供娱乐,禁止赌博";
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
