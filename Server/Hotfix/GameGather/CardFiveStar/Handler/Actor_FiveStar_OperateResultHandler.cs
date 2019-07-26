using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家操作结果
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_OperateResultHandler : AMActorHandler<FiveStarPlayer, Actor_FiveStar_OperateResult>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_OperateResult message)
        {
            try
            {
                fiveStarPlayer.OperatePengGangHu(message.OperateInfo);//玩家操作
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
