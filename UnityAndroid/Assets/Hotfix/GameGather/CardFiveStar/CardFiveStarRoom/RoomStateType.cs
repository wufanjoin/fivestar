using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
  public  class RoomStateType
  {
      public const int None = 0;//没有状态 空闲
      public const int AwaitPerson = 1;//等待状态 等人齐
      public const int GameIn = 2;//游戏中
      public const int ReadyIn = 3;//准备中
    }
}
