using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public class ApplyFriendsCirleInfoFactory
    {
        public static ApplyFriendsCirleInfo Create(int friendsCirleId)
        {
            ApplyFriendsCirleInfo applyFriendsCirleInfo=ComponentFactory.Create<ApplyFriendsCirleInfo>();
            applyFriendsCirleInfo.FriendsCirleId = friendsCirleId;
            return applyFriendsCirleInfo;
        }
    }
}
