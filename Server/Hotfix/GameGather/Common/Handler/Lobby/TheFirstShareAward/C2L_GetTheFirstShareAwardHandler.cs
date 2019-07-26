using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取每日首次分享朋友圈的奖励钻石数量
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetTheFirstShareAwardHandler : AMRpcHandler<C2L_GetTheFirstShareAward, L2C_GetTheFirstShareAward>
    {
        protected override void Run(Session session, C2L_GetTheFirstShareAward message, Action<L2C_GetTheFirstShareAward> reply)
        {
            L2C_GetTheFirstShareAward response = new L2C_GetTheFirstShareAward();
            try
            {
                response.JeweleAmount = GameLobby._TheFirstShareAwarNum;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
