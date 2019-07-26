using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public  class AgecyInfo : Entity
    {
        public long UserId { get; set; }//代理的userId
        public int Level { get; set; }//代理的等级
    }
}
