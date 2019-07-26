using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家托管
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_CollocationChangeHandler : AMActorHandler<FiveStarPlayer, Actor_FiveStar_CollocationChange>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_CollocationChange message)
        {
            try
            {
                fiveStarPlayer.SetCollocation(message.IsCollocation);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
