using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETHotfix;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{ 
  public static  class RoomInfoFactory
    {
       private static List<RoomInfo> _roomInfoPool=new List<RoomInfo>();
        public static RoomInfo Creator(MatchRoom mactchRoom)
        {
            RoomInfo roomInfo;
            if (_roomInfoPool.Count > 0)
            {
                roomInfo = _roomInfoPool[0];
                _roomInfoPool.RemoveAt(0);
            }
            else
            {
                roomInfo =new RoomInfo();
            }
            roomInfo.RoomConfigLists = mactchRoom.RoomConfig.RoomConfigs;
            roomInfo.RoomId = mactchRoom.RoomId;
            roomInfo.FriendsCircleId = mactchRoom.FriendsCircleId;
            roomInfo.MatchPlayerInfos.Clear();
            roomInfo.MatchPlayerInfos.Add(mactchRoom.PlayerInfoDic.Values.ToArray());
            return roomInfo;
        }

        public static void Destroy(RoomInfo roomInfo)
        {
            if (roomInfo == null)
            {
                return;
            }
            _roomInfoPool.Add(roomInfo);
        }

        public static RepeatedField<RoomInfo> Creator(List<MatchRoom>  mactchRoom)
        {
            RepeatedField<RoomInfo> listRooms = new RepeatedField<RoomInfo>();
            for (int i = 0; i < mactchRoom.Count; i++)
            {
                listRooms.Add(Creator(mactchRoom[i])); ;
            }
            return listRooms;
        }

        public static void Destroy(RepeatedField<RoomInfo> roomInfos)
        {
            for (int i = 0; i < roomInfos.Count; i++)
            {
                Destroy(roomInfos[i]);
            }
        }
    }
}
