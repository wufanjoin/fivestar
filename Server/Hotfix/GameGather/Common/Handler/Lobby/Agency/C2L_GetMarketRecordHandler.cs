using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取销售记录
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetMarketRecordHandler : AMRpcHandler<C2L_GetMarketRecord, L2C_GetMarketRecord>
    {
        protected override async void Run(Session session, C2L_GetMarketRecord message, Action<L2C_GetMarketRecord> reply)
        {
            L2C_GetMarketRecord response = new L2C_GetMarketRecord();
            try
            {
                List<MarketInfo> marketInfos=await AgencyComponent.Ins.GetMarketRecord(message.UserId, response);
                if (marketInfos != null)
                {
                    response.MarketInfos.Add(marketInfos.ToArray());
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
