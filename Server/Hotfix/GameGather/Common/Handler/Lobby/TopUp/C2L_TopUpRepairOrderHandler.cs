using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 充值补单
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_TopUpRepairOrderHandler : AMRpcHandler<C2L_TopUpRepairOrder, L2C_TopUpRepairOrder>
    {
        protected override  void Run(Session session, C2L_TopUpRepairOrder message, Action<L2C_TopUpRepairOrder> reply)
        {
            L2C_TopUpRepairOrder response = new L2C_TopUpRepairOrder();
            try
            {
                TopUpComponent.Ins.RepairOrder(message.OrderId, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}