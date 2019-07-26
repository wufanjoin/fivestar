using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix.GameGather.Common.Handler.User
{
    /// <summary>
    /// 用户下线
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class G2S_UserOfflineHandler : AMHandler<G2S_UserOffline>
    {
        protected override void Run(Session session, G2S_UserOffline message)
        {
            try
            {
                Game.Scene.GetComponent<MatchRoomComponent>().RmoveQueueUser(message.UserId);//直接移除匹配队列
                Game.Scene.GetComponent<MatchRoomComponent>().PlayerOffline(message.UserId);//通知玩家下线
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
