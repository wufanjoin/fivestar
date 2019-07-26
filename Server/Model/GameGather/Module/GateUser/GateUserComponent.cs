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
    public class GateUserComponentAwakeSystem : AwakeSystem<GateUserComponent>
    {
        public override void Awake(GateUserComponent self)
        {
            self.Awake();
        }
    }
    public class GateUserComponent : Component
    {
        public readonly Dictionary<long, User> mUserDic = new Dictionary<long, User>();
        private Session userSession;
        private Session matchSession;

        public Session UserSession
        {

            get
            {
                if (userSession == null)
                {
                    userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
                }
                return userSession;
            }
        }

        public Session MatchSession
        {
            get
            {
                if (matchSession == null)
                {
                    matchSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.Match);
                }
                return matchSession;
            }
        }
        public static GateUserComponent Ins { private set; get; }
        public void Awake()
        {
            Ins = this;
        }
    }
}
