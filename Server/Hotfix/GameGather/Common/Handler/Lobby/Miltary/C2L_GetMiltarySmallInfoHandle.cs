using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetMiltarySmallInfoHandler : AMRpcHandler<C2L_GetMiltarySmallInfo, L2C_GetMiltarySmallInfo>
    {
        protected override async void Run(Session session, C2L_GetMiltarySmallInfo message, Action<L2C_GetMiltarySmallInfo> reply)
        {
            L2C_GetMiltarySmallInfo response = new L2C_GetMiltarySmallInfo();
            try
            {
                response.MiltarySmallAllInfo = await MiltaryComponent.Ins.GetMiltarySmallInfo(message.MiltaryId);
               
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
