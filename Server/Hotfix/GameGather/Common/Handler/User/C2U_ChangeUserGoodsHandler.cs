using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 更改用户物品
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_ChangeUserGoodsHandler : AMRpcHandler<C2U_ChangeUserGoods, U2C_ChangeUserGoods>
    {
        protected override async void Run(Session session, C2U_ChangeUserGoods message, Action<U2C_ChangeUserGoods> reply)
        {
            U2C_ChangeUserGoods response = new U2C_ChangeUserGoods();
            try
            {
                User user = await UserHelp.GoodsChange(message.ChangeUserUserId, message.GoodsId, message.Amount,GoodsChangeType.Administrator);
                if (user == null)
                {
                    response.Message = "用户不存在";
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}