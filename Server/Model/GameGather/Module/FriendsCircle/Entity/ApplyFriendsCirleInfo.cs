using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    /// <summary>
    /// 亲友圈申请信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public  class ApplyFriendsCirleInfo : Entity
    {
        public int FriendsCirleId { get; set; }//对应的亲友圈id
        public RepeatedField<long> ApplyList =new RepeatedField<long>();//申请列表里面的玩家
    }
}
