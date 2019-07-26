using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家出牌结果
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_PlayerReadyHandcs : AMActorHandler<FiveStarPlayer, Actor_FiveStar_PlayerReady>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_PlayerReady message)
        {
            try
            {
                fiveStarPlayer.Ready();//玩家准备
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
