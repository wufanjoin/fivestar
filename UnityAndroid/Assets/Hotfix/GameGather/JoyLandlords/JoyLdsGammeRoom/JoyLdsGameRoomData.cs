using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
     public class JoyLdsGameState
     {
         public const int NoneStart = 1;//未开始
         public const int BeingMatching = 2;//匹配中
         public const int BeingDeal = 3;//发牌中
         public const int BeingPlayCards = 4;//游戏中打牌中
         public const int LookResult = 5;//查看结算面板中
         public const int AlreadyDissolv = 6;//已经解散了
     }

    public class JoyLdsRoomData
    {
        public long RoomId;//匹配的时候就是匹配房间ID
    }
}
