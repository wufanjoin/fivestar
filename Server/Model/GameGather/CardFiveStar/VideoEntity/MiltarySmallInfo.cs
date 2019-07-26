using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class MiltarySmallInfo : Entity
    {
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < ParticularMiltarys.Count; i++)
            {
                ParticularMiltarys[i].Dispose();
            }
            ParticularMiltarys.Clear();
        }
    }
}
