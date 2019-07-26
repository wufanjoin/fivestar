using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家完成一局 房卡游戏 增加免费一次免费抽奖次数
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class M2S_UserFinishRoomCardGameHandler : AMHandler<M2S_UserFinishRoomCardGame>
    {
        protected override void Run(Session session, M2S_UserFinishRoomCardGame message)
        {
            try
            {
                TurntableComponent.Ins.FinishTaskAddLotteryCount(message.UserIds);//玩家完成一局 游戏 增加免费抽奖次数
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
