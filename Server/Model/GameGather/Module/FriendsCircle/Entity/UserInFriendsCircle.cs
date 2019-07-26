using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    /// <summary>
    /// 用户所在的亲友圈列表
    /// </summary>
    [BsonIgnoreExtraElements]
    public class UserInFriendsCircle : Entity
    {
        public long UserId { get; set; }//用户ID
        public RepeatedField<int> FriendsCircleIdList = new RepeatedField<int>();//用户所在的亲友圈id
    }
}
