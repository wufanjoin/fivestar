using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class AccountInfo:Entity
    {
        public string Account { get; set; }
        public string Password { get; set; }

        public long UserId { get; set; }

        public bool IsStopSeal { get; set; }//是否封号

        public long LastLoginTime { get; set; }//最后登录时间
    }
}
