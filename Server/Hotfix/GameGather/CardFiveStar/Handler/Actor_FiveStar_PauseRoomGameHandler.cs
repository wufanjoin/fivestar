using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 房间暂停和结束暂停
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_PauseRoomGameHandler : AMActorHandler<FiveStarRoom, Actor_PauseRoomGame>
    {
        protected override void Run(FiveStarRoom fiveStarPlayer, Actor_PauseRoomGame message)
        {
            try
            {
                //还要停止计时功能 会专门写个方法暂时不管
                fiveStarPlayer.IsPause = message.IsPause;
                //只有 房卡模式才会暂停 而房卡模式 又没有超时操作 所以什么都不用做
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
