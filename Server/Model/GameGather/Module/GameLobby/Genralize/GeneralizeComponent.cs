using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;


namespace ETModel
{
    [ObjectSystem]
    public class GeneralizeComponentAwakeSystem : AwakeSystem<GeneralizeComponent>
    {
        public override void Awake(GeneralizeComponent self)
        {
            self.Awake();
        }
    }
    public class GeneralizeComponent : Component
    {
        public const int AwardJewelNum = 20;//奖励获得的钻石
        public DBProxyComponent dbProxyComponent;
        public static GeneralizeComponent Ins;

        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();

        }
    }
}
