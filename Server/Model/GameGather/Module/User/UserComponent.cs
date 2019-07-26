using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using ETModel;

namespace ETModel
{
    [ObjectSystem]
    public class UserComponentAwakeSystem : AwakeSystem<UserComponent>
    {
        public override void Awake(UserComponent self)
        {
            self.Awake();
        }
    }
    public  class UserComponent : Component
    {
        public DBProxyComponent dbProxyComponent;
        public readonly Dictionary<long,User> mOnlineUserDic=new Dictionary<long, User>();
        public static UserComponent Ins { private set; get; }
        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
        }

       


    }
}
