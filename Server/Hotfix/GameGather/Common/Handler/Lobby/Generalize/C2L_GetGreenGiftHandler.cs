using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 领取新手礼包
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetGreenGiftHandler : AMRpcHandler<C2L_GetGreenGift, L2C_GetGreenGift>
    {
        protected override async void Run(Session session, C2L_GetGreenGift message, Action<L2C_GetGreenGift> reply)
        {
            L2C_GetGreenGift response = new L2C_GetGreenGift();
            try
            {
                await GeneralizeComponent.Ins.GetGreenGift(message.UserId, message.Code, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
