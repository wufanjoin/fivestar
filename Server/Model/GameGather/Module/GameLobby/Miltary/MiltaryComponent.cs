using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;


namespace ETModel
{
    [ObjectSystem]
    public class MiltaryComponentAwakeSystem : AwakeSystem<MiltaryComponent>
    {
        public override void Awake(MiltaryComponent self)
        {
            self.Awake();
        }
    }
    public class MiltaryComponent : Component
    {
        public DBProxyComponent dbProxyComponent;
        public static MiltaryComponent Ins;

        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
        }
    }
}
