using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserComponentStopSealSystem
    {
        public static async Task StopSealOperate(this UserComponent userComponent, StopSealRecord stopSealRecord, IResponse iResponse)
        {
            List<AccountInfo>  accountInfos=await userComponent.dbProxyComponent.Query<AccountInfo>(coount => coount.UserId == stopSealRecord.StopSealUserId);
            if (accountInfos.Count > 0)
            {
                accountInfos[0].IsStopSeal = stopSealRecord.IsStopSeal;
                await userComponent.dbProxyComponent.Save(accountInfos[0]);
            }
            else
            {
                iResponse.Message = "用户不存在";
            }
            stopSealRecord.Time = TimeTool.GetCurrenTimeStamp();
            await  userComponent.dbProxyComponent.Save(stopSealRecord);
        }
    }
}
