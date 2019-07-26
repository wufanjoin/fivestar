using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
  public partial class FriendsCircle
    {
        public FiveStarRoomConfig defaultWanFaConfigInfo;//当前亲友默认玩法配置

        public FiveStarRoomConfig GetDefaultWanFaConfigInfoasd()
        {
            if (defaultWanFaConfigInfo == null || defaultWanFaConfigInfo.Configs == null)
            {
                defaultWanFaConfigInfo = FiveStarRoomConfigFactory.Create(DefaultWanFaCofigs);
            }
            return defaultWanFaConfigInfo;
        }
    }
}
