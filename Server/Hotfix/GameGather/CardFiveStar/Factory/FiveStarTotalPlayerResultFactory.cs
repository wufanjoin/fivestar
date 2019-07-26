using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class FiveStarTotalPlayerResultFactory
    {
        public static FiveStarTotalPlayerResult Create(FiveStarPlayer fiveStarPlayer)
        {
            FiveStarTotalPlayerResult fiveStarTotalPlayerResult = ComponentFactory.Create<FiveStarTotalPlayerResult>();
            fiveStarTotalPlayerResult.SeatIndex = fiveStarPlayer.SeatIndex;
            fiveStarTotalPlayerResult.HuPaiCount = fiveStarPlayer.HuPaiCount;
            fiveStarTotalPlayerResult.ZiMoCount = fiveStarPlayer.ZiMoCount;
            fiveStarTotalPlayerResult.FangChongCount = fiveStarPlayer.FangChongCount;
            fiveStarTotalPlayerResult.GangPaiCount = fiveStarPlayer.GangPaiCount;
            fiveStarTotalPlayerResult.TotalSocre = fiveStarPlayer.NowScore;
            return fiveStarTotalPlayerResult;
        }
    }
}
