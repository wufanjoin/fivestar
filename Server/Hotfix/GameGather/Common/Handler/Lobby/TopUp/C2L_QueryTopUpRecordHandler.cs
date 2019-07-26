using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询充值记录
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_QueryTopUpRecordHandler : AMRpcHandler<C2L_QueryTopUpRecord, L2C_QueryTopUpRecord>
    {
        protected override async void Run(Session session, C2L_QueryTopUpRecord message, Action<L2C_QueryTopUpRecord> reply)
        {
            L2C_QueryTopUpRecord response = new L2C_QueryTopUpRecord();
            try
            {

                List<TopUpRecord> stopSealRecords =await Game.Scene.GetComponent<DBProxyComponent>().Query<TopUpRecord>(
                    stopSeal => stopSeal.TopUpUserId == message.QueryUserId);
                response.TopUpRecords.Add(stopSealRecords.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}