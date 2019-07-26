using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
   public class FriendsCircleAndCreateUserFactory
    {
        public static async ETTask<FriendsCircleAndCreateUser>  Create(FriendsCircle friendsCircle)
        {
            FriendsCircleAndCreateUser friendsCircleAndCreateUser=new FriendsCircleAndCreateUser();
            friendsCircleAndCreateUser.FriendsCircleInfo = friendsCircle;
            friendsCircleAndCreateUser.CreateUser =await UserComponent.Ins.GetUserInfo(friendsCircle.CreateUserId);
            return friendsCircleAndCreateUser;
        }
        public static async ETTask<List<FriendsCircleAndCreateUser>>   Create(IList<FriendsCircle> friendsCircles)
        {
            List < FriendsCircleAndCreateUser > friendsCircleAndCreateUsers =new List<FriendsCircleAndCreateUser>();
            for (int i = 0; i < friendsCircles.Count; i++)
            {
                friendsCircleAndCreateUsers.Add(await Create(friendsCircles[i]));
            }
            return friendsCircleAndCreateUsers;
        }
    }
}
