using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class TopUpComponentAwakeSystem : AwakeSystem<TopUpComponent>
    {
        public override void Awake(TopUpComponent self)
        {
            self.Awake();
        }
    }
    public class TopUpComponent : Component
    {
        public static TopUpComponent Ins { private set; get; }

        public void Awake()
        {
            Game.Scene.AddComponent<WeChatPayComponent>();
        }

        //获取充值订单id
        public static string GetTradeNo()
        {
            return "kxwyx" + TimeTool.GetCurrenTimeStamp();
        }
    }
}
