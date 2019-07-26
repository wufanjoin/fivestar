using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class FriendsCircleComponentDBSystem
    {
        //查询用户所在亲友圈信息
        public static async Task<UserInFriendsCircle> QueryUserInFriendsCircle(this FriendsCircleComponent friendsCircleComponent, long userId)
        {
            List<UserInFriendsCircle> userfriendsCircles = await friendsCircleComponent.dbProxyComponent.Query<UserInFriendsCircle>(
                userInfriendsCircle => userInfriendsCircle.UserId == userId);
            if (userfriendsCircles.Count > 0)
            {
                return userfriendsCircles[0];
            }
            return UserInFriendsCircleFactory.Create(userId);
        }
        //查询亲友圈成员列表
        public static async Task<FriendsCirleMemberInfo> QueryFriendsCircleMember(this FriendsCircleComponent friendsCircleComponent, int friendsCircleId)
        {
            List<FriendsCirleMemberInfo> friendsCircles = await friendsCircleComponent.dbProxyComponent.Query<FriendsCirleMemberInfo>(
                friendsCircle => friendsCircle.FriendsCircleId == friendsCircleId);
            if (friendsCircles.Count > 0)
            {
                return friendsCircles[0];
            }
            return FriendsCirleMemberInfoFactory.Create(friendsCircleId);
        }
        //查询亲友圈信息
        public static async Task<FriendsCircle> QueryFriendsCircle(this FriendsCircleComponent friendsCircleComponent, int friendsCircleId)
        {
            List<FriendsCircle> friendsCircles = await friendsCircleComponent.dbProxyComponent.Query<FriendsCircle>(
                friendsCircle => friendsCircle.FriendsCircleId == friendsCircleId);
            if (friendsCircles.Count > 0)
            {
                return friendsCircles[0];
            }
            return null;
        }
        //查询亲友圈申请列表
        public static async Task<ApplyFriendsCirleInfo> QueryFriendsCircleApply(this FriendsCircleComponent friendsCircleComponent, int friendsCircleId)
        {
            List<ApplyFriendsCirleInfo> applyFriendsCirleInfos = await friendsCircleComponent.dbProxyComponent.Query<ApplyFriendsCirleInfo>(
                friendsCircle => friendsCircle.FriendsCirleId == friendsCircleId);
            if (applyFriendsCirleInfos.Count > 0)
            {
                return applyFriendsCirleInfos[0];
            }
            return ApplyFriendsCirleInfoFactory.Create(friendsCircleId);
        }
        //查询本亲友圈所有排行榜信息
        public static async Task<List<RanKingPlayerInfo>> QueryRankingInfo(this FriendsCircleComponent friendsCircleComponent, int friendsCircleId)
        {
            List<RanKingPlayerInfo> ranKingPlayerInfos = await FriendsCircleComponent.Ins.dbProxyComponent.Query<RanKingPlayerInfo>(ranking => ranking.FriendsCircleId == friendsCircleId
            && ranking.TotalNumber!=0);//局数是0的 不显示

            return ranKingPlayerInfos;
        }
    }
}
