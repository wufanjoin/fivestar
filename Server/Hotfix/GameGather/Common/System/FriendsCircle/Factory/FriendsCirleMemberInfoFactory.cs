using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
  public  class FriendsCirleMemberInfoFactory
    {
        public static FriendsCirleMemberInfo Create(int friendsCirleId)
        {
            FriendsCirleMemberInfo friendsCirleMemberInfo = ComponentFactory.Create<FriendsCirleMemberInfo>();
            friendsCirleMemberInfo.FriendsCircleId = friendsCirleId;
            return friendsCirleMemberInfo;
        }
    }
}
