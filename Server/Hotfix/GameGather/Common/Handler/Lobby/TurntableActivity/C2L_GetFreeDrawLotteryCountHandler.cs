using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取转免费抽奖次数
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetFreeDrawLotteryCountHandler : AMRpcHandler<C2L_GetFreeDrawLotteryCount, L2C_GetFreeDrawLotteryCount>
    {
        protected override async void Run(Session session, C2L_GetFreeDrawLotteryCount message, Action<L2C_GetFreeDrawLotteryCount> reply)
        {
            L2C_GetFreeDrawLotteryCount response = new L2C_GetFreeDrawLotteryCount();
            try
            {
                response.Count =await TurntableComponent.Ins.GetFreeDrawLotteryCount(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
