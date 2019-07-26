using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   /// <summary>
   /// 获取是不是代理
   /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetAgencyStatuHandler : AMRpcHandler<C2L_GetAgencyStatu, L2C_GetAgencyStatu>
    {
        protected override void Run(Session session, C2L_GetAgencyStatu message, Action<L2C_GetAgencyStatu> reply)
        {
            L2C_GetAgencyStatu response = new L2C_GetAgencyStatu();
            try
            {
                response.IsAgency = AgencyComponent.Ins.JudgeIsAgency(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
