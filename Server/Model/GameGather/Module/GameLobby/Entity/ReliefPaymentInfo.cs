using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [BsonIgnoreExtraElements]
 public   class ReliefPaymentInfo:Entity
    {
        public long UserId { get; set; }//领取人的userid

        public long Time { get; set; }//最后一次领取的时机
        public int Number { get; set; }//领取的次数
    }
}
