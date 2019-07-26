using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public partial class FreeDrawLottery : Entity
    {
        public long UserId { get; set; }
        public int Count { get; set; }

        public long UpAddFreeDrawLotteryTime { get; set; }//上次增加免费抽奖的时间
    }
}
