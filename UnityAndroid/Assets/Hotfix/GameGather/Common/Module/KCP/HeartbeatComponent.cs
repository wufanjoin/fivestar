using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeartbeatComponentAwakeSystem : AwakeSystem<HeartbeatComponent>
    {
        public static C2G_Heartbeat heartBeat = new C2G_Heartbeat();
        public override async void Awake(HeartbeatComponent self)
        {
            Session session = (self.Entity as Session);
            DetectionNetworkType(session);//检测网络连接状态
            while (!self.IsDisposed)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(10000);
                session.Send(heartBeat);//每间隔10秒发一条 心跳消息
            }

        }

        public async void DetectionNetworkType(Session session)
        {
            while (!session.IsDisposed)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
                //如果当前是无网络状态 发送连接失败的消息
                if (NetworkType.None == SdkMgr.Ins.GetNetworkInfo())
                {
                    session.session.Error = (int) SocketError.NetworkDown;
                    session.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// 心跳组件
    /// </summary>
    public  class HeartbeatComponent: Component
    {
        
    }
}
