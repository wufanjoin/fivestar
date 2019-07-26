using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    /// <summary>
    /// 亲友圈成员列表
    /// </summary>
    [BsonIgnoreExtraElements]
    public  class FriendsCirleMemberInfo : Entity
    {
        public int FriendsCircleId { get; set; }//对应的亲友圈id
        public RepeatedField<long> MemberList = new RepeatedField<long>();//亲友圈所有成员的userId
    }
}
