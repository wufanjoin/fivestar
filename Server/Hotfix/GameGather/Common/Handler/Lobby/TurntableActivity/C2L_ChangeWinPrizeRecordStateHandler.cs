using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 更改中奖记录状态
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_ChangeWinPrizeRecordStateHandler : AMRpcHandler<C2L_ChangeWinPrizeRecordState, L2C_ChangeWinPrizeRecordState>
    {
        protected override async void Run(Session session, C2L_ChangeWinPrizeRecordState message, Action<L2C_ChangeWinPrizeRecordState> reply)
        {
            L2C_ChangeWinPrizeRecordState response = new L2C_ChangeWinPrizeRecordState();
            try
            {
               List<WinPrizeRecord> winPrizeRecords=await TurntableComponent.Ins.dbProxyComponent.Query<WinPrizeRecord>(
                    winPrizeRecord => winPrizeRecord.WinPrizeId == message.UserId);
                if (winPrizeRecords.Count>0)
                {
                    winPrizeRecords[0].Remark = message.Remark;
                    winPrizeRecords[0].Type = message.Type;
                    await TurntableComponent.Ins.dbProxyComponent.Save(winPrizeRecords[0]);
                }
                else
                {
                    response.Message = "没有该中奖记录";
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
