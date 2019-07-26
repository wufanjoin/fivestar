using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家打漂结果
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_DaPiaoResultHandler : AMActorHandler<FiveStarPlayer, Actor_FiveStar_DaPiaoResult>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_DaPiaoResult message)
        {
            try
            {
                fiveStarPlayer.DaPiao(message.SelectPiaoNum);//玩家打漂
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
