using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetReliefPaymentHandler : AMRpcHandler<C2L_GetReliefPayment, L2C_GetReliefPayment>
    {
        protected override async void Run(Session session, C2L_GetReliefPayment message, Action<L2C_GetReliefPayment> reply)
        {
            L2C_GetReliefPayment response = new L2C_GetReliefPayment();
            try
            {
                List<ReliefPaymentInfo>  reliefPaymentInfos=await GameLobby.Ins.dbProxyComponent.Query<ReliefPaymentInfo>(info => info.UserId == message.UserId);
                if (reliefPaymentInfos.Count <= 0)
                {
                    ReliefPaymentInfo reliefPaymentInfo = ComponentFactory.Create<ReliefPaymentInfo>();
                    reliefPaymentInfo.UserId = message.UserId;
                    reliefPaymentInfo.Number = 0;
                    reliefPaymentInfos.Add(reliefPaymentInfo);
                }
                if (!TimeTool.TimeStampIsToday(reliefPaymentInfos[0].Time))//如果上次领取的时机 不是今天 次数重置为0
                {
                    reliefPaymentInfos[0].Number = 0;
                }
                if (reliefPaymentInfos[0].Number >= GameLobby.ReliefPaymentNumber)
                {
                    response.Message = "今日领取救济金达到上限";
                    reply(response);
                    return;
                }
                reliefPaymentInfos[0].Time = TimeTool.GetCurrenTimeStamp();//记录当前领取的时机
                reliefPaymentInfos[0].Number++;//领取的次数++
                //给用户加上豆子
                await UserHelp.GoodsChange(message.UserId, GoodsId.Besans, GameLobby.ReliefPaymentBeansNum,
                    GoodsChangeType.None, false);
                await GameLobby.Ins.dbProxyComponent.Save(reliefPaymentInfos[0]);//存储 领取救济金信息
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
