using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class SessionHeartbeatComponentAwakeSystem : AwakeSystem<SessionHeartbeatComponent>
    {
        public override void Awake(SessionHeartbeatComponent self)
        {
            HeartbeatMgrComponent.Ins.AddSessionHeartbeat(self);

        }
    }


    public class SessionHeartbeatComponent : Component
    {
        public const int DestroySeesiontTime = 40;//多长 时间 没收到客户端消息 就直接销毁
        public const int DestroySeesiontSecondTotalNum = 10;//一秒内收到多少条消息 就直接销毁

        public int SecondTotalMessageNum = 0;//一秒内 累计收到的消息条数
        public int UpReceiveMessageDistance = 0;//距离上次收到消息 有多长时间

        //销毁Session
        public void DisposeSession()
        {
            Entity.Dispose();
        }
        public override void Dispose()
        {
            HeartbeatMgrComponent.Ins.RemoveSessionHeartbeat(InstanceId);//必须在 base.Dispose(); 前面调 因为 Dispose会吧id置为0
            base.Dispose();
             SecondTotalMessageNum = 0;//一秒内 累计收到的消息条数
            UpReceiveMessageDistance = 0;//距离上次收到消息 有多长时间
    }
    }
}
