using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_GetCommodityListHandler : AMRpcHandler<C2L_GetCommodityList, L2C_GetCommodityList>
    {
        protected override void Run(Session session, C2L_GetCommodityList message, Action<L2C_GetCommodityList> reply)
        {
            L2C_GetCommodityList response = new L2C_GetCommodityList();
            try
            {
                ShoppingCommodityComponent shoppingCommodity=Game.Scene.GetComponent<ShoppingCommodityComponent>();

                response.BeansList.AddRange(shoppingCommodity.GetBeansList());
                response.JewelList.AddRange(shoppingCommodity.GetJewelList());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        public bool tsasdc(User s)
        {
            return true;
        }
    }
}