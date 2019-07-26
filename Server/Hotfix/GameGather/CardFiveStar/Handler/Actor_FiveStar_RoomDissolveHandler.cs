using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 强制解散房间
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_FiveStar_RoomDissolveHandler : AMActorHandler<FiveStarRoom, Actor_RoomDissolve>
    {
        protected override void Run(FiveStarRoom fiveStarRoom, Actor_RoomDissolve message)
        {
            try
            {
                if (fiveStarRoom.RoomType == RoomType.Match)
                {
                    Log.Error("匹配类型的房间 不可能被强制销毁 出现错误");
                    fiveStarRoom.Dispose();
                    return;
                }
                fiveStarRoom.RoomTotalResult();//房间直接总结算
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
