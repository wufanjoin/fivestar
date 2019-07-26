using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 设置代理等级
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_SetAgencyLvHandler : AMRpcHandler<C2L_SetAgencyLv, L2C_SetAgencyLv>
    {
        protected override void Run(Session session, C2L_SetAgencyLv message, Action<L2C_SetAgencyLv> reply)
        {
            L2C_SetAgencyLv response = new L2C_SetAgencyLv();
            try
            {
                AgencyComponent.Ins.AlterAgencyLv(message.AgencyUserId, message.AgencyLv);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
