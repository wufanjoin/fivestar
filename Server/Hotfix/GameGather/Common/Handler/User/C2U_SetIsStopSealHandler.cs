using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 封号和解封
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_SetIsStopSealHandler : AMRpcHandler<C2U_SetIsStopSeal, U2C_SetIsStopSeal>
    {
        protected override async void Run(Session session, C2U_SetIsStopSeal message, Action<U2C_SetIsStopSeal> reply)
        {
            U2C_SetIsStopSeal response = new U2C_SetIsStopSeal();
            try
            {

                await UserComponent.Ins.StopSealOperate(message.StopSeal, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}