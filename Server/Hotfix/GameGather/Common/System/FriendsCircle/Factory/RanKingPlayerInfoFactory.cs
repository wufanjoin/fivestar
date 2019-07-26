using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public class RanKingPlayerInfoFactory
    {
        public static RanKingPlayerInfo Create(int friendsCircleId,long userId)
        {

            RanKingPlayerInfo friendsCircle = ComponentFactory.Create<RanKingPlayerInfo>();
            friendsCircle.FriendsCircleId = friendsCircleId;
            friendsCircle.UserId = userId;
            return friendsCircle;
        }
    }
}
