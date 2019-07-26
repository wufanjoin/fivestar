using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class S2M_RoomDissolveHandler : AMHandler<S2M_RoomDissolve>
    {
        protected override async void Run(Session session, S2M_RoomDissolve message)
        {
            
            try
            {
                MatchRoomComponent matchRoomComponent = Game.Scene.GetComponent<MatchRoomComponent>();
                MatchRoom matchRoom = matchRoomComponent.GetRoom(message.RoomId);
                if (matchRoom!=null)
                {
                    if (matchRoom.RoomType==RoomType.Match)
                    {
                        await matchRoom.DeductBeans();//扣除豆子
                    }
                    else if (message.CurrOfficNum <=1&& message.CurrRoomStateType == RoomStateType.GameIn)
                    {
                        
                    }
                    else
                    {
                        Session lobbySession=Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.Lobby);
                        M2S_UserFinishRoomCardGame m2SUserFinishRoom = new M2S_UserFinishRoomCardGame();
                        foreach (var player in matchRoom.PlayerInfoDic)
                        {
                            m2SUserFinishRoom.UserIds.Add(player.Value.User.UserId);
                        }
                        lobbySession.Send(m2SUserFinishRoom);//通知大厅服 这些玩家完成一局游戏
                        //通知大厅 服 玩家完成一局游戏
                       await matchRoom.DeductJewel();//扣除钻石
                    }
                }
                matchRoomComponent.RemoveRoom(message.RoomId);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }


    }
}