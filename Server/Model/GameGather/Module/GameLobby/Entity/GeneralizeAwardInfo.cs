using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class GeneralizeAwardInfo : Entity
    {
        public long UserId { get; set; }//推广人的UserId
    }
}
