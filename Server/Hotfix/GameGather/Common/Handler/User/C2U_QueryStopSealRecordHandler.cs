using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询封号记录
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_QueryStopSealRecordHandler : AMRpcHandler<C2U_QueryStopSealRecord, U2C_QueryStopSealRecord>
    {
        protected override async void Run(Session session, C2U_QueryStopSealRecord message, Action<U2C_QueryStopSealRecord> reply)
        {
            U2C_QueryStopSealRecord response = new U2C_QueryStopSealRecord();
            try
            {

               List< StopSealRecord > stopSealRecords =await UserComponent.Ins.dbProxyComponent.Query<StopSealRecord>(
                    stopSeal => stopSeal.StopSealUserId == message.QueryUserId);
                response.StopSealRecords.Add(stopSealRecords.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}