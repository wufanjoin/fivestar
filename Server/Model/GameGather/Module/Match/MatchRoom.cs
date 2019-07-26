using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
using Google.Protobuf.Collections;

namespace ETModel
{
    [ObjectSystem]
    public class CustomRoomAwakeSystem : AwakeSystem<MatchRoom>
    {
        public override void Awake(MatchRoom self)
        {
            self.Awaek();
        }
    }
    public class MatchRoom : Entity
    {
        public bool IsGameBeing=false;//是否在游戏中
        public Actor_VoteDissolveRoomResult VoteDissolveResult;//现在投票解散房间信息
        public int RoomId { get; set; }//房间ID
        public int RoomType = 0;//房间类型
        public int NeedJeweNumCount = 0;//房卡模式需要钻石的数量
        public bool IsAADeductJewel = false;//是否是AA扣除钻石
        public int FriendsCircleId = 0;//所在的亲友圈id 0就是不是亲友圈的房间
        public long FriendsCreateUserId = 0;//如果是亲友圈里面房间 就有亲友圈 主人的Userid 扣钻石的时候用

        public long GameServeRoomActorId {get; set; }//对应游戏服的房间ActorId
        public MatchRoomConfig RoomConfig; //房间配置信息   
        public Dictionary<int, MatchPlayerInfo> PlayerInfoDic=new Dictionary<int, MatchPlayerInfo>();//座位索引对应的玩家信息
        public bool IsVoteDissolveIn = false;//是否在投票解散中
        public int VoteTimeResidue = 0;//投票计时剩余时间
        public int intData;

        public void Awaek()
        {
            
        }

        public override void Dispose()
        {
            foreach (var player in PlayerInfoDic)
            {
                player.Value.Dispose();
            }
            PlayerInfoDic.Clear();
            VoteDissolveResult = null;
            if (RoomType == ETHotfix.RoomType.RoomCard)
            {
                RoomConfig?.Dispose();
            }
            RoomId = 0;
            IsGameBeing = false;
            IsVoteDissolveIn = false;
            GameServeRoomActorId = 0;
            NeedJeweNumCount = 0;
            FriendsCircleId = 0;
            RoomType = 0;
            base.Dispose();
        }
    }
}
