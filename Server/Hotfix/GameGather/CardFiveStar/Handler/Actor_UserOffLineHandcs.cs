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
    public class Actor_UserOffLineHandcs : AMActorHandler<FiveStarPlayer, Actor_UserOffLine>
    {
        protected override void Run(FiveStarPlayer fiveStarPlayer, Actor_UserOffLine message)
        {
            try
            {
                fiveStarPlayer.User.GetComponent<UserGateActorIdComponent>().ActorId =0;//网关SeeionId设为0
                fiveStarPlayer.User.IsOnLine = false;//用户下线 后面还要做托管
                fiveStarPlayer.SetCollocation(true);//进入托管
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
