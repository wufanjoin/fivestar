using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取推广奖励信息
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetGenralizeInfoHandler : AMRpcHandler<C2L_GetGenralizeInfo, L2C_GetGenralizeInfo>
    {
        protected override async void Run(Session session, C2L_GetGenralizeInfo message, Action<L2C_GetGenralizeInfo> reply)
        {
            L2C_GetGenralizeInfo response = new L2C_GetGenralizeInfo();
            try
            {
                response.AwardInfo = await GeneralizeComponent.Ins.GetGeneralizeAwardInfo(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
