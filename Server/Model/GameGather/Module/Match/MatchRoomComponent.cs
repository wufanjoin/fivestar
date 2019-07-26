using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
using ETModel;

namespace ETModel
{
    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem : AwakeSystem<MatchRoomComponent>
    {
        public override void Awake(MatchRoomComponent self)
        {
            self.Awake();
        }
    }
    public class MatchRoomComponent : Component
    {
        public Dictionary<long,long> mUserInQueue=new Dictionary<long, long>();//用户和ID所在对应的匹配队列的ID
        public Dictionary<long,List<User>> mAllQueueDics=new Dictionary<long, List<User>>();//每个匹配队列ID 里面所对应的玩家

        public Dictionary<int, MatchRoom> MatchRoomDic = new Dictionary<int, MatchRoom>();//房间ID和对应的房间 
        public Dictionary<long, MatchRoom> UserIdInRoomIdDic = new Dictionary<long, MatchRoom>();//用户ID和所对应房间
        public Dictionary<int, List<MatchRoom> > FriendsCircleInMatchRoomDic=new Dictionary<int, List<MatchRoom>>();//亲友圈对应的房间信息

        public List<MatchRoom> VoteInRooms=new List<MatchRoom>();//所有正在投票中的房间

        public MatchRoomConfig[] AllMatchRoomConfigs;//所有匹配房间配置
        public static MatchRoomComponent Ins { get; private set; }
        public async void Awake()
        {
            Ins = this;
            AllMatchRoomConfigs = await Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetAllRoomConfig();
            foreach (var config in AllMatchRoomConfigs)
            {
                mAllQueueDics.Add(config.MatchRoomId, new List<User>());
            }
        }
    }
}
