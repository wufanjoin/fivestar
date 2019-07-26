using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class HeartbeatMgrComponentAwakeSystem : AwakeSystem<HeartbeatMgrComponent>
    {
        public static List<SessionHeartbeatComponent> _DestroyHeartbeatComponents=new List<SessionHeartbeatComponent>();
        public override async void Awake(HeartbeatMgrComponent self)
        {
            HeartbeatMgrComponent.Ins = self;
            while (true)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
                for (int i = 0; i < self.SessionHeartbeatIds.Count; i++)
                {
                    //一秒内收到消息的数量
                    self.SessionHeartbeatDic[self.SessionHeartbeatIds[i]].SecondTotalMessageNum = 0;
                    self.SessionHeartbeatDic[self.SessionHeartbeatIds[i]].UpReceiveMessageDistance++;

                    if (self.SessionHeartbeatDic[self.SessionHeartbeatIds[i]].UpReceiveMessageDistance>=
                        SessionHeartbeatComponent.DestroySeesiontTime)
                    {
                        self.SessionHeartbeatDic[self.SessionHeartbeatIds[i]].DisposeSession();//如果上次收到时间 过长 就直接销毁DisposeSession
                    }
                }
            }
        }
    }
    public class HeartbeatMgrComponent: Component
    {
        public List<long> SessionHeartbeatIds=new List<long>();
        public Dictionary<long, SessionHeartbeatComponent> SessionHeartbeatDic=new Dictionary<long, SessionHeartbeatComponent>();
        public static HeartbeatMgrComponent Ins { get; set; }

        /// <summary>
        /// 添加SessionHeartbeat 组件
        /// </summary>
        /// <param name="sessionHeartbeat"></param>
        public void AddSessionHeartbeat(SessionHeartbeatComponent sessionHeartbeat)
        {
            SessionHeartbeatDic[sessionHeartbeat.InstanceId] = sessionHeartbeat;
            SessionHeartbeatIds.Add(sessionHeartbeat.InstanceId);
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="id"></param>
        public void RemoveSessionHeartbeat(long id)
        {
            if (SessionHeartbeatDic.ContainsKey(id))
            {
                SessionHeartbeatDic.Remove(id);
                SessionHeartbeatIds.Remove(id);
            }
        }
    }
}
