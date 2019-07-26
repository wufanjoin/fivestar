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
    public class Actor_FiveStar_PlayCardResultHandler : AMActorHandler<FiveStarPlayer, Actor_FiveStar_PlayCardResult>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_PlayCardResult message)
        {
            try
            {
                fiveStarPlayer.PlayCard(message.Card);//玩家出牌
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
