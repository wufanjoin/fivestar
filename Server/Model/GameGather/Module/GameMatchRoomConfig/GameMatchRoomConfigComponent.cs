using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;

namespace ETModel
{
    [ObjectSystem]
    public class GameMatchRoomConfigComponentAwakeSystem : AwakeSystem<GameMatchRoomConfigComponent>
    {
        public override void Awake(GameMatchRoomConfigComponent self)
        {
            self.Awake();
        }
    }

    public class GameMatchRoomConfigComponent : Component
    {
        private readonly Dictionary<long, List<MatchRoomConfig>> mMatachRoomConfigs=new Dictionary<long, List<MatchRoomConfig>>();//游戏id 对应所有的匹配房间配置
        private readonly Dictionary<long, MatchRoomConfig> mMatachIdInRoomConfigs = new Dictionary<long, MatchRoomConfig>();//匹配房间id 对应的匹配配置
        private DBProxyComponent dbProxyComponent;
        public async void Awake()
        {
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            List<MatchRoomConfig> matchRoomConfigList = await dbProxyComponent.Query<MatchRoomConfig>(matchRoomConfig =>true);
            if (matchRoomConfigList.Count <= 0)
            {
              await InitDefaultMatachRoomConfigs(matchRoomConfigList);
            }
            InitMatachRoomConfigs(matchRoomConfigList);
            InitMatachInRoomConfigs(matchRoomConfigList);
        }

        public void InitMatachRoomConfigs(List<MatchRoomConfig> matchRoomConfigs)
        {
            foreach (var matchRoomConfig in matchRoomConfigs)
            {
                if (!mMatachRoomConfigs.ContainsKey(matchRoomConfig.ToyGameId))
                {
                    mMatachRoomConfigs.Add(matchRoomConfig.ToyGameId,new List<MatchRoomConfig>(){ matchRoomConfig });
                }
                else
                {
                    mMatachRoomConfigs[matchRoomConfig.ToyGameId].Add(matchRoomConfig);
                }
            }
        }

        public void InitMatachInRoomConfigs(List<MatchRoomConfig> matchRoomConfigs)
        {
            foreach (var matchRoomConfig in matchRoomConfigs)
            {
                if (!mMatachIdInRoomConfigs.ContainsKey(matchRoomConfig.MatchRoomId))
                {
                    mMatachIdInRoomConfigs.Add(matchRoomConfig.MatchRoomId, matchRoomConfig);
                }
                else
                {
                    Log.Error("匹配房间配置错误 有相同的房间ID:"+ matchRoomConfig.MatchRoomId);
                }
            }
        }
        //得到所有对应房间的ID
        public async Task<long[]>  GetAllRoomId()
        {
            while (mMatachIdInRoomConfigs.Count<=0)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);//等待1秒
            }
            return mMatachIdInRoomConfigs.Keys.ToArray();
        }
        //得到所有的匹配房间配置
        public async Task<MatchRoomConfig[]> GetAllRoomConfig()
        {
            while (mMatachIdInRoomConfigs.Count <= 0)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);//等待1秒
            }
            return mMatachIdInRoomConfigs.Values.ToArray();
        }
        public List<MatchRoomConfig> GetMatachRoomConfigs(long toyGameId)
        {
            List<MatchRoomConfig> matchRoomConfigs;
            if (!mMatachRoomConfigs.TryGetValue(toyGameId, out matchRoomConfigs))
            {
                Log.Error("要获取游戏配置表不存在游戏Id:"+ toyGameId);
            }
            return matchRoomConfigs;
        }
        //得到匹配房间的房间信息
        public MatchRoomConfig GetMatachRoomInfo(long RoomId)
        {
            MatchRoomConfig matchRoomConfig;
            if (mMatachIdInRoomConfigs.TryGetValue(RoomId, out matchRoomConfig))
            {
                
            }
            return matchRoomConfig;
        }
        public async Task InitDefaultMatachRoomConfigs(List<MatchRoomConfig> matchRoomConfigs)
        {
            MatchRoomConfig matchRoomConfig = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig.MatchRoomId = 1001;
            matchRoomConfig.ToyGameId = ToyGameId.JoyLandlords;
            matchRoomConfig.BaseScore = 5;
            matchRoomConfig.GameNumber = 3;
            matchRoomConfig.BesansLowest = 3000;
            matchRoomConfig.RoomConfigs.Add(5);
            matchRoomConfig.RoomConfigs.Add(6);
            matchRoomConfig.RoomConfigs.Add(30);
            matchRoomConfig.Name = "初级场";

            MatchRoomConfig matchRoomConfig2 = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig2.MatchRoomId = 1002;
            matchRoomConfig2.ToyGameId = ToyGameId.JoyLandlords;
            matchRoomConfig2.BaseScore = 20;
            matchRoomConfig2.GameNumber = 3;
            matchRoomConfig2.BesansLowest = 10000;
            matchRoomConfig2.Name = "中级场";


            RepeatedField<int>  RoomConfigs3 = new RepeatedField<int>(){1,4,30,1,3,300,2,1,0,16,1};
            RepeatedField<int> RoomConfigs4 = new RepeatedField<int>() { 1, 4, 30, 1, 3, 1000, 2, 1, 0, 16, 1 };
            RepeatedField<int> RoomConfigs5 = new RepeatedField<int>() { 1, 4, 30, 1, 3, 5000, 2, 1, 0, 16, 1 };

            MatchRoomConfig matchRoomConfig3 = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig3.RoomConfigs = RoomConfigs3;
            matchRoomConfig3.MatchRoomId = 2001;
            matchRoomConfig3.ToyGameId = ToyGameId.CardFiveStar;
            matchRoomConfig3.BaseScore = 300;
            matchRoomConfig3.GameNumber = 3;
            matchRoomConfig3.CostConsume = 220;
            matchRoomConfig3.BesansLowest = 1000;
            matchRoomConfig3.Name = "初级场";


            MatchRoomConfig matchRoomConfig4 = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig4.RoomConfigs = RoomConfigs4;
            matchRoomConfig4.MatchRoomId = 2002;
            matchRoomConfig4.ToyGameId = ToyGameId.CardFiveStar;
            matchRoomConfig4.BaseScore = 1000;
            matchRoomConfig4.GameNumber = 3;
            matchRoomConfig4.CostConsume = 800;
            matchRoomConfig4.BesansLowest = 20000;
            matchRoomConfig4.Name = "中级场";


            MatchRoomConfig matchRoomConfig5 = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig5.RoomConfigs = RoomConfigs5;
            matchRoomConfig5.MatchRoomId = 2003;
            matchRoomConfig5.ToyGameId = ToyGameId.CardFiveStar;
            matchRoomConfig5.BaseScore = 5000;
            matchRoomConfig5.GameNumber = 3;
            matchRoomConfig5.CostConsume = 2000;
            matchRoomConfig5.BesansLowest = 500000;
            matchRoomConfig5.Name = "高级场";


            matchRoomConfigs.Add(matchRoomConfig);
            matchRoomConfigs.Add(matchRoomConfig2);
            matchRoomConfigs.Add(matchRoomConfig3);
            matchRoomConfigs.Add(matchRoomConfig4);
            matchRoomConfigs.Add(matchRoomConfig5);

            await dbProxyComponent.Save(matchRoomConfig);
            await dbProxyComponent.Save(matchRoomConfig2);
            await dbProxyComponent.Save(matchRoomConfig3);
            await dbProxyComponent.Save(matchRoomConfig4);
            await dbProxyComponent.Save(matchRoomConfig5);
        }

    }
}
