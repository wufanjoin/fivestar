using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public class GetGreenGiftInfo : Entity
    {
        public long GetUserId { get; set; }//领取人的UserId
        public long InviteUserId { get; set; }//邀请人的UserId
        public int GetJewelNum{ get; set; }//两人都获得的钻石数
    }
}
