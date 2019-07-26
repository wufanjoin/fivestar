using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取转盘物品列表
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetTurntableGoodssHandler : AMRpcHandler<C2L_GetTurntableGoodss, L2C_GetTurntableGoodss>
    {
        protected override void Run(Session session, C2L_GetTurntableGoodss message, Action<L2C_GetTurntableGoodss> reply)
        {
            L2C_GetTurntableGoodss response = new L2C_GetTurntableGoodss();
            try
            {
                response.Goodss=TurntableComponent.Ins.GetTurntableGoodss();
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
