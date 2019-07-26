using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;


namespace ETModel
{
    [ObjectSystem]
    public class AgencyComponentAwakeSystem : AwakeSystem<AgencyComponent>
    {
        public override void Awake(AgencyComponent self)
        {
            self.Awake();
        }
    }
    public class AgencyComponent : Component
    {
        public DBProxyComponent dbProxyComponent;
        public static AgencyComponent Ins;
        public List<long> AgecyUserIdList;

        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            InitAgecyUserIdList();

        }
        public async void InitAgecyUserIdList()
        {
            AgecyUserIdList=new List<long>();
            List<AgecyInfo> agecyInfos = await dbProxyComponent.Query<AgecyInfo>((sevice) => true);
            for (int i = 0; i < agecyInfos.Count; i++)
            {
                AgecyUserIdList.Add(agecyInfos[i].UserId);
            }
            AgecyUserIdList.Add(1001);
        }
    }
}
