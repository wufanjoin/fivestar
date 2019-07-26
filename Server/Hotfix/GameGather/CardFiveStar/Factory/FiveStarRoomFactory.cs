using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
 public  static class FiveStarRoomFactory
    {
        public static async Task<FiveStarRoom> Create(M2S_StartGame m2SStartGame)
        {
            try
            {
                FiveStarRoom fiveStarRoom = ComponentFactory.Create<FiveStarRoom>();
                
                FiveStarRoomConfig fiveStarRoomConfig = FiveStarRoomConfigFactory.Create(m2SStartGame.RoomConfig.RoomConfigs);//创建房间配置信息

                fiveStarRoom.FriendsCircleId = m2SStartGame.FriendsCircleId;
                fiveStarRoom.NeedJeweNumCount = m2SStartGame.NeedJeweNumCount;//需要的钻石数量
                fiveStarRoom.RoomConfig = fiveStarRoomConfig;//房间配置
                fiveStarRoom.MathRoomId = m2SStartGame.RoomConfig.MatchRoomId;//匹配房间ID 在房卡模式下就是本身的房间ID
                fiveStarRoom.RoomId = m2SStartGame.RoomId;//房间ID
                fiveStarRoom.RoomType=m2SStartGame.RoomType;//赋值房间类型
                fiveStarRoom.RoomNumber = fiveStarRoomConfig.RoomNumber;//房间人数
                fiveStarRoom.CurrRoomStateType = RoomStateType.GameIn;//房间状态 改为游戏中
               
                if (fiveStarRoom.RoomType == RoomType.RoomCard)
                {
                    fiveStarRoom.StartVideoDataId = FiveStarRoomComponent.Ins.GetMiltaryDataStartId();//如果在房卡模式要获取起始录像Id
                }
                fiveStarRoom.IsHaveAI = false;//默认是没有AI的
                //创建玩家对象
                foreach (var playerInfo in m2SStartGame.MatchPlayerInfos)
                {
                    fiveStarRoom.FiveStarPlayerDic[playerInfo.SeatIndex] = await FiveStarPlayerFactory.Create(playerInfo, fiveStarRoom);
                    if (playerInfo.IsAI)
                    {
                        fiveStarRoom.IsHaveAI = true;//只要有一个AI就是有AI
                    }
                }
                await fiveStarRoom.AddComponent<MailBoxComponent>().AddLocation();
                return fiveStarRoom;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            
        }
    }
}
