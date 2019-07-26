using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家亮倒
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_LiangDaoHandler : AMActorHandler<FiveStarPlayer, Actor_FiveStar_LiangDao>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_FiveStar_LiangDao message)
        {
            try
            {
                fiveStarPlayer.LiangDao();//玩家亮倒
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
