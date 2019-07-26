using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
   public partial class RanKingPlayerInfo
    {
        private User _User { get; set; }

        public async ETTask<User> GetUser()
        {
            if (_User == null)
            {
                _User = await UserComponent.Ins.GetUserInfo(UserId);
            }
            return _User;
        }
    }
}
