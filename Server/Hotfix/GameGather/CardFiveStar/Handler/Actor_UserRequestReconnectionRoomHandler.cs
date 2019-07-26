using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 玩家请求重连
    /// </summary>
    [ActorMessageHandler(AppType.CardFiveStar)]
    public class Actor_UserRequestReconnectionRoomHandler : AMActorHandler<FiveStarRoom, Actor_UserRequestReconnectionRoom>
    {
        protected override void Run(FiveStarRoom fiveStarRoom, Actor_UserRequestReconnectionRoom message)
        {
            try
            {
                FiveStarPlayer fiveStarPlayer = fiveStarRoom.GetPlayerInfoUserIdIn(message.UserId);
                if (fiveStarPlayer == null)
                {
                    Log.Error("请求重连数据" + message.UserId + "不在房间中");
                    return;
                }
                fiveStarPlayer.User.IsOnLine = true;//状态改为在线
                fiveStarPlayer.SetCollocation(false);//取消托管
                if (fiveStarPlayer.FiveStarRoom.CurrRoomStateType == RoomStateType.ReadyIn)
                {
                    fiveStarPlayer.Ready(); //如果未准备的状态上线 直接改变状态为准备
                }
                Actor_FiveStar_Reconnection actorFiveStarReconnection=ReconnectionRoomDataFactory.Create(fiveStarRoom, message.UserId);
           
                fiveStarPlayer.User.GetComponent<UserGateActorIdComponent>().ActorId = message.UserActorId;
                fiveStarPlayer.SendGateStartGame();//通知用户所在的网关服开始游戏 就是把 游戏服的对象id发过去
                fiveStarPlayer.SendMessageUser(actorFiveStarReconnection);//发送重连数据
                //补发最后一条可操作消息
                if (fiveStarRoom.EndCanOperateAndCanChuMessage != null)
                {
                    fiveStarPlayer.SendMessageUser(fiveStarRoom.EndCanOperateAndCanChuMessage);
                }
                ReconnectionRoomDataFactory.Dispose(actorFiveStarReconnection);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
    }
}
