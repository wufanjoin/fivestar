using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class GoodsDealRecord : Entity
    {
        //public long UserId { get; set; }//交易的UserId
        //public int Amount { get; set; }//交易的数量
        //public int FinishNowAmount { get; set; }//交易完成后的数量
        //public long Time { get; set; }//交易的时间

        //public int Type { get; set; }//交易的类型
    }
}
