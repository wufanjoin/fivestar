using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 请求抽奖
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_TurntableDrawLotteryHandler : AMRpcHandler<C2L_TurntableDrawLottery, L2C_TurntableDrawLottery>
    {
        protected override async void Run(Session session, C2L_TurntableDrawLottery message, Action<L2C_TurntableDrawLottery> reply)
        {
            L2C_TurntableDrawLottery response = new L2C_TurntableDrawLottery();
            try
            {
                response.TurntableGoodsId=(await TurntableComponent.Ins.DarwLottery(message.UserId, response)) .TurntableGoodsId;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
