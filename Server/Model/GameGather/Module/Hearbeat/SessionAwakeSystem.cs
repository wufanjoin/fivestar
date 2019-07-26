using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class SessionHeartbeatAwakeSystem : AwakeSystem<Session, AChannel>
    {
        public override void Awake(Session self, AChannel b)
        {
            if (self.Network.AppType==AppType.None)//不是任何APPType 就是客户端
            {
                self.AddComponent<SessionHeartbeatComponent>();
            }
        }
    }
}

