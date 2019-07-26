using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
 public static class FreeDrawLotteryFactory
    {
        public static FreeDrawLottery Create(long userId)
        {
            FreeDrawLottery freeDrawLottery=ComponentFactory.Create<FreeDrawLottery>();
            freeDrawLottery.UserId = userId;
            freeDrawLottery.Count = 1;
            freeDrawLottery.UpAddFreeDrawLotteryTime = TimeTool.GetCurrenTimeStamp();
            return freeDrawLottery;
        }
    }
}
