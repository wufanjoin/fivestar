using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取物品变化记录 只有钻石的
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_GetGoodsDealRecordHandler : AMRpcHandler<C2U_GetGoodsDealRecord, U2C_GetGoodsDealRecord>
    {
        protected override async void Run(Session session, C2U_GetGoodsDealRecord message, Action<U2C_GetGoodsDealRecord> reply)
        {
            U2C_GetGoodsDealRecord response = new U2C_GetGoodsDealRecord();
            try
            {

               List< GoodsDealRecord > goodsDealRecords =await UserComponent.Ins.dbProxyComponent.Query<GoodsDealRecord>(
                    goodsdeal => goodsdeal.UserId == message.DealRecordUserId);
                response.GoodsDealRecords.Add(goodsDealRecords.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}