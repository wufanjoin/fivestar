using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    public class S2U_UserGetGoodsHandler : AMRpcHandler<S2U_UserGetGoods, U2S_UserGetGoods>
    {
        protected  override async void Run(Session session, S2U_UserGetGoods message, Action<U2S_UserGetGoods> reply)
        {
            U2S_UserGetGoods response = new U2S_UserGetGoods();
            try
            {
                response.user = await Game.Scene.GetComponent<UserComponent>().UserGetGoods(message.UserId, message.GetGoodsList, message.GoodsChangeType, message.isShowHintPanel);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
