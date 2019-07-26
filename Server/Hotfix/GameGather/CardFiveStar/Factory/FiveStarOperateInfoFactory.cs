using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public static class FiveStarOperateInfoFactory
    {
        public static FiveStarOperateInfo Create(int card,int oprateType,int playCardIndex)
        {
            FiveStarOperateInfo fiveStarOperateInfo=ComponentFactory.Create<FiveStarOperateInfo>();
            fiveStarOperateInfo.Card = card;
            fiveStarOperateInfo.OperateType = oprateType;
            fiveStarOperateInfo.PlayCardIndex = playCardIndex;
            return fiveStarOperateInfo;
        }
    }
}
