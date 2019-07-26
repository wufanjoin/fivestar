using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;


namespace ETModel
{
    [ObjectSystem]
    public class SingInActivityConfigComponentAwakeSystem : AwakeSystem<SingInActivityComponent>
    {
        public override void Awake(SingInActivityComponent self)
        {
            self.Awake();
        }
    }
    public class SingInActivityComponent : Component
    {
        public List<SignInAward> mSignInAwardList = new List<SignInAward>();
        public DBProxyComponent dbProxyComponent;
        public static SingInActivityComponent Ins;
        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            mSignInAwardList = await dbProxyComponent.Query<SignInAward>(signInAward => true);
            if (mSignInAwardList.Count <= 0)
            {
                SaveInitDefaultSignInAwardList();
            }
        }

        //如果数据库没有签到奖励列表就初始化
        public async void SaveInitDefaultSignInAwardList()
        {
            for (int i = 0; i < 7; i++)
            {
                SignInAward signInAward = ComponentFactory.Create<SignInAward>();
                if (i < 3)
                {
                    signInAward.Amount = 2;
                }
                else if (i < 6)
                {
                    signInAward.Amount = i;
                }
                else
                {
                    signInAward.Amount = 10;
                }
                signInAward.GoodsId = GoodsId.Jewel;
                signInAward.NumberDays = i + 1;
                mSignInAwardList.Add(signInAward);
                await dbProxyComponent.Save(signInAward);
            }
        }

    }
}
