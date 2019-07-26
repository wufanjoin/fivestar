using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 匹配服通知游戏服开始一局游戏
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class S2M_StartGameHandler : AMHandler<S2M_StartGame>
    {
        protected override  void Run(Session session, S2M_StartGame message)
        {
            try
            {
                Game.Scene.GetComponent<MatchRoomComponent>().GetRoom(message.RoomId).GameServeStartGame(message.RoomActorId);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
