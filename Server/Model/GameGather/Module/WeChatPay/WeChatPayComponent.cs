using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    [ObjectSystem]
    public class WeChatPayComponentAwakeSystem : AwakeSystem<WeChatPayComponent>
    {
        public override void Awake(WeChatPayComponent self)
        {
            self.Awake();
        }
    }
    public class WeChatPayComponent : Component
    {
        public static WeChatPayComponent Ins { private set; get; }
        public void Awake()
        {
            Ins = this;
        }


    }
}
