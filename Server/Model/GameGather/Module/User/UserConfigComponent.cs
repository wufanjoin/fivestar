using ETHotfix;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class GameConfiguredComponentAwakeSystem : AwakeSystem<UserConfigComponent>
    {
        public override void Awake(UserConfigComponent self)
        {
            self.Awake();
        }
    }
    public class UserConfigComponent: Component
    {
        public UserConfig InitUserBeans { private set; get; }
        public UserConfig InitUserJewel { private set; get; }
        public long MaximumUserId { set; get; }
        public static UserConfigComponent Ins { private set; get; }

        private DBProxyComponent dbProxyComponent;

        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();

            List<User> userIdMax =await dbProxyComponent.SortQuery<User>(user=>true, user=> user.UserId == -1, 1);
            if (userIdMax.Count > 0)
            {
                MaximumUserId = (userIdMax[0] as User).UserId;
            }
            else
            {
                MaximumUserId = 1000;
            }
           
            List<UserConfig> userConfigs = await dbProxyComponent.Query<UserConfig>(userConfig =>true);
            if (userConfigs.Count > 0)
            {
                SetConfigured(userConfigs);
            }
            else
            {
                SaveConfigured();
            }

        }

        public void SetConfigured(List<UserConfig> userConfigs)
        {
            for (int i = 0; i < userConfigs.Count; i++)
            {
                switch (userConfigs[i].Id)
                {
                    case 1:
                        InitUserBeans = userConfigs[i];
                        break;
                    case 2:
                        InitUserJewel = userConfigs[i];
                        break;
                    default:
                        Log.Error($"用户服没有该{userConfigs[i].Id}Id的配置");
                        break;
                }
            }
        }
        public async void SaveConfigured()
        {
            InitUserBeans =new UserConfig();
            InitUserBeans.Id = 1;
            InitUserBeans.ConfigName = "InitUserBeans";
            InitUserBeans.Value = 3000;
            await dbProxyComponent.Save(InitUserBeans);
            InitUserJewel = new UserConfig();
            InitUserJewel.Id = 2;
            InitUserJewel.ConfigName = "InitUserJewel";
            InitUserJewel.Value = 20;
            await dbProxyComponent.Save(InitUserJewel);
        }
    }
}
