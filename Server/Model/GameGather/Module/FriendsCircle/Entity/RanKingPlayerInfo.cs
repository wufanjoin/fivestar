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
    public partial class RanKingPlayerInfo : Entity
    {
        public int FriendsCircleId { get; set; }
    }
}
