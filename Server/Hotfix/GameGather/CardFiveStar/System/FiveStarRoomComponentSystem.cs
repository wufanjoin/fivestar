using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarRoomComponentAwakeSystem : AwakeSystem<FiveStarRoomComponent>
    {
        public override async void Awake(FiveStarRoomComponent self)
        {
            while (true)
            {
                try
                {
                    await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);//等待1秒
                    FiveStarRoomComponent.CurrTime++;

                    for (int i = 0; i < self.RoomIds.Count; i++)
                    {
                        if (self.pJoyLdsRoomDic[self.RoomIds[i]].OverTime!=0&&
                            self.pJoyLdsRoomDic[self.RoomIds[i]].OverTime<= FiveStarRoomComponent.CurrTime)
                        {
                            self.pJoyLdsRoomDic[self.RoomIds[i]].OverTime = 0;
                            self.pJoyLdsRoomDic[self.RoomIds[i]].OverTimeOperate();
                        }
                    }
                   
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }

            }
        }
    }

    public static class FiveStarRoomComponentSystem
    {

        public static async Task<FiveStarRoom> StartGame(this FiveStarRoomComponent fiveStarRoomComponent, M2S_StartGame m2SStartGame)
        {
            try
            {
                FiveStarRoom fiveStarRoom = await FiveStarRoomFactory.Create(m2SStartGame);
                fiveStarRoomComponent.pJoyLdsRoomDic[fiveStarRoom.RoomId] = fiveStarRoom;
                fiveStarRoomComponent.RoomIds.Add(fiveStarRoom.RoomId);
                fiveStarRoom.StartGame();
                return fiveStarRoom;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }

        public static void RemoveRoom(this FiveStarRoomComponent fiveStarRoomComponent, int roomId)
        {
            if (fiveStarRoomComponent.pJoyLdsRoomDic.ContainsKey(roomId))
            {
                fiveStarRoomComponent.pJoyLdsRoomDic.Remove(roomId);
                fiveStarRoomComponent.RoomIds.Remove(roomId);
            }
        }

    }
}
