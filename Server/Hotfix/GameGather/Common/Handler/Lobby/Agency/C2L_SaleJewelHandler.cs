using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 销售钻石
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_SaleJewelHandler : AMRpcHandler<C2L_SaleJewel, L2C_SaleJewel>
    {
        protected override async void Run(Session session, C2L_SaleJewel message, Action<L2C_SaleJewel> reply)
        {
            L2C_SaleJewel response = new L2C_SaleJewel();
            try
            {
                await AgencyComponent.Ins.SaleJewel(message.UserId,message.MaiJiaUser,message.JewelNum, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
