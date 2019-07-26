using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class AdministratorComponentAwakeSystem : AwakeSystem<AdministratorComponent>
    {
        public override void Awake(AdministratorComponent self)
        {
            self.Awake();
        }
    }
    public   class AdministratorComponent: Component
    {
        public static AdministratorComponent Ins { private set; get; }

        public User ExamineUser;//当前查看的User对象

        public bool IsStopSeal;//是否被封号了

        public long LastLoginTime;//最后登录时间

        public int AgecyLv;//代理等级
        public void Awake()
        {
            Ins = this;
        }

        public void SetExamineUser(U2C_QueryUserInfo u2CQueryUserInfo)
        {
            ExamineUser = u2CQueryUserInfo.User;
            IsStopSeal = u2CQueryUserInfo.IsStopSeal;
            LastLoginTime = u2CQueryUserInfo.LastLoginTime;
            AgecyLv = u2CQueryUserInfo.AgecyLv;
        }
    }
}
