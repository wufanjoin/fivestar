using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 匹配服通知游戏服开始一局游戏
    /// </summary>
   // [MessageHandler(AppType.JoyLandlords)]
    public class M2S_JoyLds_StartGameHandler : AMHandler<M2S_StartGame>
    {
        protected override async void Run(Session session, M2S_StartGame message)
        {
            S2M_StartGame response=new S2M_StartGame();
            try
            {
                Log.Info("斗地主服收到开始游戏消息");
                if (message.RoomConfig.ToyGameId != ToyGameId.JoyLandlords)
                {
                    return;
                }
                JoyLdsRoom joyLdsRoom=await JoyLdsRoomFactory.Create(message);
                joyLdsRoom.StartGame();
                response.RoomActorId = joyLdsRoom.Id;
                session.Send(response);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
