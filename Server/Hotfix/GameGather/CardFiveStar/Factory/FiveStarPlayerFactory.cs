using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
 public  static class FiveStarPlayerFactory
    {
        public  static async Task<FiveStarPlayer> Create(MatchPlayerInfo matchPlayerInfo, FiveStarRoom fiveStarRoom)
        {
            try
            {
                FiveStarPlayer fiveStarPlayer = ComponentFactory.Create<FiveStarPlayer>();
                fiveStarPlayer.FiveStarRoom = fiveStarRoom;
                fiveStarPlayer.SeatIndex = matchPlayerInfo.SeatIndex;
                fiveStarPlayer.IsAI = matchPlayerInfo.IsAI;
                fiveStarPlayer.IsCollocation = fiveStarPlayer.IsAI;//如果是AI默认就是托管状态
                fiveStarPlayer.User = matchPlayerInfo.User;
                fiveStarPlayer.User.AddComponent<UserGateActorIdComponent>().ActorId = matchPlayerInfo.SessionActorId;
                if (fiveStarRoom.RoomType == RoomType.Match)
                {
                    fiveStarPlayer.NowScore = (int)fiveStarPlayer.User.Beans;
                }
                await fiveStarPlayer.AddComponent<MailBoxComponent>().AddLocation();
                return fiveStarPlayer;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
      
        }

        public static FiveStarPlayer CopySerialize(FiveStarPlayer fiveStarPlayer)
        {
            FiveStarPlayer newFiveStarPlayer;
            if (_playerPool.Count >0)
            {
                newFiveStarPlayer = _playerPool[0];
                _playerPool.RemoveAt(0);
            }
            else
            {
                newFiveStarPlayer = ComponentFactory.Create<FiveStarPlayer>();
            }
            newFiveStarPlayer.SeatIndex = fiveStarPlayer.SeatIndex;
            newFiveStarPlayer.User = fiveStarPlayer.User;
            newFiveStarPlayer.PlayCards = fiveStarPlayer.PlayCards;
            newFiveStarPlayer.OperateInfos = fiveStarPlayer.OperateInfos;
            newFiveStarPlayer.PiaoNum = fiveStarPlayer.PiaoNum;
            newFiveStarPlayer.NowScore = fiveStarPlayer.NowScore;
            newFiveStarPlayer.IsLiangDao = fiveStarPlayer.IsLiangDao;
            newFiveStarPlayer.Hands = fiveStarPlayer.Hands;
            newFiveStarPlayer.ReadyState = fiveStarPlayer.ReadyState;
            newFiveStarPlayer.IsAlreadyDaPiao = fiveStarPlayer.IsAlreadyDaPiao;
            newFiveStarPlayer.HuPaiCount = fiveStarPlayer.HuPaiCount;
            newFiveStarPlayer.FangChongCount = fiveStarPlayer.FangChongCount;
            newFiveStarPlayer.ZiMoCount = fiveStarPlayer.ZiMoCount;
            newFiveStarPlayer.GangPaiCount = fiveStarPlayer.GangPaiCount;
            return newFiveStarPlayer;
        }
        private static List<FiveStarPlayer> _playerPool=new List<FiveStarPlayer>();
        public static void DisposeSerializePlayer(FiveStarPlayer fiveStarPlayer)
        {
            _playerPool.Add(fiveStarPlayer);
        }
    }
}
