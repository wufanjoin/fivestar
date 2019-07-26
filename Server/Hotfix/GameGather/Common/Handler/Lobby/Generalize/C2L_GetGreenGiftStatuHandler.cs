using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询新手礼包领取的状态
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetGreenGiftStatuHandler : AMRpcHandler<C2L_GetGreenGiftStatu, L2C_GetGreenGiftStatu>
    {
        protected override async void Run(Session session, C2L_GetGreenGiftStatu message, Action<L2C_GetGreenGiftStatu> reply)
        {
            L2C_GetGreenGiftStatu response = new L2C_GetGreenGiftStatu();
            try
            {
                response.IsHaveGet = await GeneralizeComponent.Ins.GreenGiftGetStatu(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
