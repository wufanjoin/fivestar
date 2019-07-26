using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ETHotfix
{
 public   class CopyFriendsCircle
    {
        public static FriendsCircle Copy(FriendsCircle friendsCircle)
        {
            FriendsCircle newfriendsCircle=new FriendsCircle();
            newfriendsCircle.FriendsCircleId = friendsCircle.FriendsCircleId;
            newfriendsCircle.Name = friendsCircle.Name;
            newfriendsCircle.CreateUserId = friendsCircle.CreateUserId;
            newfriendsCircle.TotalNumber = friendsCircle.TotalNumber;
            newfriendsCircle.DefaultWanFaCofigs =new RepeatedField<int>(){ friendsCircle.DefaultWanFaCofigs };
            newfriendsCircle.ManageUserIds =new RepeatedField<long>(){ friendsCircle.ManageUserIds.ToArray() }; 
            newfriendsCircle.Announcement = friendsCircle.Announcement;
            newfriendsCircle.IsRecommend = friendsCircle.IsRecommend;
            return newfriendsCircle;
        }
    }
}
