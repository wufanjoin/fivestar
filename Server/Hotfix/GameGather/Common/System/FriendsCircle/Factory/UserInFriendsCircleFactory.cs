using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
  public  class UserInFriendsCircleFactory
    {
        public static UserInFriendsCircle Create(long userId)
        {
            UserInFriendsCircle userInFriendsCircle = ComponentFactory.Create<UserInFriendsCircle>();
            userInFriendsCircle.UserId = userId;
            return userInFriendsCircle;
        }
    }
}
