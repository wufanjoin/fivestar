using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;

namespace ETModel
{
    [ObjectSystem]
    public class ActivityLobbyAwakeSystem : AwakeSystem<GameLobby>
    {
        public override void Awake(GameLobby self)
        {
            self.Awake();
        }
    }
    public class GameLobby : Entity
    {
        public DBProxyComponent dbProxyComponent;
        public static GameLobby Ins;
        public List<ServiceInfo> mServiceInfos;
        public RepeatedField<ServiceInfo> ServiceInfosRepeatedField=new RepeatedField<ServiceInfo>();
        public const int _TheFirstShareAwarNum = 5;//每日首次分享到朋友圈的奖励数
        public const int ReliefPaymentBeansNum = 3000;//每次领取救济金豆子的数量
        public const int ReliefPaymentNumber = 3;//每日能领取救济金的次数上限
        public Action WeekRefreshAction;//每周一00 会调一次
        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            this.AddComponent<SingInActivityComponent>();
            this.AddComponent<TurntableComponent>();
            this.AddComponent<GeneralizeComponent>();
            this.AddComponent<AgencyComponent>();
            this.AddComponent<MiltaryComponent>();
            mServiceInfos = await dbProxyComponent.Query<ServiceInfo>((sevice) => true);
            if (mServiceInfos.Count <= 0)
            {
               await InitDefaultList();
            }
            ServiceInfosRepeatedField.Add(mServiceInfos.ToArray());
            
        }

      
        public async Task InitDefaultList()
        {
            ServiceInfo  serviceInfo1=ComponentFactory.Create<ServiceInfo>();
            serviceInfo1.Number = "470499850";
            serviceInfo1.Type = "QQ";

            ServiceInfo serviceInfo2 = ComponentFactory.Create<ServiceInfo>();
            serviceInfo2.Number = "ckxmjkf";
            serviceInfo2.Type = "VX";

           await dbProxyComponent.Save(serviceInfo1);
           await dbProxyComponent.Save(serviceInfo2);

            mServiceInfos.Add(serviceInfo1);
            mServiceInfos.Add(serviceInfo2);
        }
    }
}
