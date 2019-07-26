using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class EverydayShareInfo : Entity
    {
        public long UserId { get; set; }
        public long ShareTime { get; set; }
    }
}
