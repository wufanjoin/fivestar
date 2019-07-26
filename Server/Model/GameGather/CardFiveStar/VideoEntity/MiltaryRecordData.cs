using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class MiltaryRecordData : Entity
    {
        public override void Dispose()
        {
            base.Dispose();
            Opcode = 0;
        }
    }
}
