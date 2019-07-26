using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class ParticularMiltary : Entity
    {
        public override void Dispose()
        {
            base.Dispose();
            GetScoreInfos.Clear();
        }
    }
}
