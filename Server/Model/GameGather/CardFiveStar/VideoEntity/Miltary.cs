using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class Miltary : Entity
    {
        public  List<long> PlayerUserIds=new List<long>();
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < PlayerInofs.Count; i++)
            {
                PlayerInofs[i].Dispose();
            }
            PlayerInofs.Clear();
            PlayerUserIds.Clear();
        }
    }
}
