using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
  public static class SeatIndexTool
    {
        public static int GetLastSeatIndex(int currSeatIndexint, int maxSeatIndex)
        {
            if (currSeatIndexint > 0)
            {
                return currSeatIndexint-1;
            }
            else
            {
                return maxSeatIndex;
            }
        }                                       
        public static int GetNextSeatIndex(int currSeatIndexint, int maxSeatIndex)
        {
            if (currSeatIndexint >= maxSeatIndex)
            {
                return 0;
            }
            else
            {
                return currSeatIndexint + 1;
            }
        }

        //根据服务索引 和用户所处索引 和房间人数 获得客户端位置位置索引 0:下 1:右 2:上 3:左
        public static int GetClientSeatIndex(int serverSeatIndex, int serverUserSeatIndex, int roomNumber)
        {
            if(serverSeatIndex== serverUserSeatIndex)
            {
                return 0;
            }
            if(roomNumber==4)
            {
                if (serverSeatIndex > serverUserSeatIndex)
                {
                    return serverSeatIndex - serverUserSeatIndex;
                }
                else
                {
                    return roomNumber - (serverUserSeatIndex - serverSeatIndex);
                }
            }
            else if(roomNumber==3)
            {
                if (serverSeatIndex == GetNextSeatIndex(serverUserSeatIndex, 2))
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else if (roomNumber == 2)
            {
                return 2;
            }
            Log.Error("获取客户端索引错误 人数:" + roomNumber + "  用户索引:" + serverUserSeatIndex + "  玩家索引:" + serverSeatIndex);
            return 0;
        }
    }
}
