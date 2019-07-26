using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class UserConfig : Entity
    {
        public long Value { get; set; } //实际值
        public string ConfigName { get; set; } //变量的名字
    }
}
