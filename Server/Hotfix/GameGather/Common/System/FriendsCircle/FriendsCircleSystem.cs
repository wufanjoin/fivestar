using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
 public  static class FriendsCircleSystem
    {
        //申请加入亲友圈
        public static async Task ApplyJoinFriendsCircle(this FriendsCircle friendsCircle, long userId, IResponse iResponse)
        {
            FriendsCirleMemberInfo memberInfo = await  FriendsCircleComponent.Ins.QueryFriendsCircleMember(friendsCircle.FriendsCircleId);
            if (memberInfo.MemberList.Contains(userId))
            {
                iResponse.Message = "你已经在该亲友圈中";
                return;
            }
            ApplyFriendsCirleInfo applyFriendsCirleInfo=await FriendsCircleComponent.Ins.QueryFriendsCircleApply(friendsCircle.FriendsCircleId);
            if (applyFriendsCirleInfo.ApplyList.Contains(userId))
            {
                iResponse.Message = "已经在申请列表中";
                return;
            }
            applyFriendsCirleInfo.ApplyList.Add(userId);
            await FriendsCircleComponent.Ins.SaveDB(applyFriendsCirleInfo);
        }
        //处理申请加入亲友圈
        public static async Task DisposeApplyResult(this FriendsCircle friendsCircle, long applyUserId, bool isConsent, IResponse iResponse)
        {
            ApplyFriendsCirleInfo applyFriendsCirleInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleApply(friendsCircle.FriendsCircleId);
            if (!applyFriendsCirleInfo.ApplyList.Contains(applyUserId))
            {
                iResponse.Message = "该玩家不在申请列表里面 或被其他管理处理";
                return;
            }
            applyFriendsCirleInfo.ApplyList.Remove(applyUserId);
            await FriendsCircleComponent.Ins.SaveDB(applyFriendsCirleInfo);
            if (isConsent)
            {
               await friendsCircle.SucceedJoinFriendsCircle(applyUserId);//成功加入亲友圈
            }
        }
        //玩家成功加入亲友圈
        public static async Task SucceedJoinFriendsCircle(this FriendsCircle friendsCircle, long userId)
        {
            UserInFriendsCircle userInFriendsCircle = await FriendsCircleComponent.Ins.QueryUserInFriendsCircle(userId);
            userInFriendsCircle.FriendsCircleIdList.Add(friendsCircle.FriendsCircleId);
            await FriendsCircleComponent.Ins.SaveDB(userInFriendsCircle);
            FriendsCirleMemberInfo friendsCirleMemberInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleMember(friendsCircle.FriendsCircleId);
            friendsCirleMemberInfo.MemberList.Add(userId);
            await FriendsCircleComponent.Ins.SaveDB(friendsCirleMemberInfo);
            friendsCircle.TotalNumber++;
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }
        //把人踢出亲友圈
        public static async Task KickOutFriendsCircle(this FriendsCircle friendsCircle, long operateUserId, long byOperateUserId, IResponse iResponse)
        {
            if (friendsCircle.ManageUserIds.Contains(byOperateUserId))
            {
                iResponse.Message = "不能踢管理出亲友圈";
                return;
            }
            FriendsCirleMemberInfo applyFriendsCirleInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleMember(friendsCircle.FriendsCircleId);
            if (!applyFriendsCirleInfo.MemberList.Contains(byOperateUserId))
            {
                iResponse.Message = "玩家已经不再亲友圈中";
                return;
            }
            friendsCircle.SucceedOutriendsCircle(byOperateUserId);//成功退出亲友圈
        }
        //玩家退出亲友圈 被管理踢 或者自己退都走这
        public static async void SucceedOutriendsCircle(this FriendsCircle friendsCircle, long userId)
        {
            if (friendsCircle.CreateUserId== userId)
            {
                return;
            }
            UserInFriendsCircle userInFriendsCircle = await FriendsCircleComponent.Ins.QueryUserInFriendsCircle(userId);
            if (userInFriendsCircle.FriendsCircleIdList.Contains(friendsCircle.FriendsCircleId))
            {
                userInFriendsCircle.FriendsCircleIdList.Remove(friendsCircle.FriendsCircleId);
            }
            await FriendsCircleComponent.Ins.SaveDB(userInFriendsCircle);
            FriendsCirleMemberInfo friendsCirleMemberInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleMember(friendsCircle.FriendsCircleId);
            if (friendsCirleMemberInfo.MemberList.Contains(userId))
            {
                friendsCirleMemberInfo.MemberList.Remove(userId);
            }
            await FriendsCircleComponent.Ins.SaveDB(friendsCirleMemberInfo);
            friendsCircle.TotalNumber--;
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }

        //修改公告
        public static async void AlterAnnouncement(this FriendsCircle friendsCircle, string announcement)
        {
            friendsCircle.Announcement = announcement;
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }
        //修改玩法
        public static async Task AlterWanFa(this FriendsCircle friendsCircle, RepeatedField<int> roomConfigs,long toyGameId, IResponse iResponse)
        {
            //效验配置 如果配置错误 会使用默认配置
            if (!RoomConfigIntended.IntendedRoomConfigParameter(roomConfigs, toyGameId))
            {
                //效验不通过 不会修改
                iResponse.Message = "玩法参数有误 无法修改";
                return;
            }
            friendsCircle.DefaultWanFaCofigs = roomConfigs;
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }
        //修改是否推荐给陌生人
        public static async void AlterIsRecommend(this FriendsCircle friendsCircle, bool isRecommend)
        {
            friendsCircle.IsRecommend = isRecommend;
            FriendsCircleComponent.Ins.AlterRecommend(friendsCircle, isRecommend);
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }
        //设置或取消管理 操作管理权限
        public static async Task OperateManageJurisdiction(this FriendsCircle friendsCircle,long oprateMageJuridictionUserId, bool isSetManage, IResponse iResponse)
        {
            if (oprateMageJuridictionUserId == friendsCircle.CreateUserId)
            {
                iResponse.Message = "不能设置自己";
                return;
            }
            if (isSetManage)
            {
                if (!friendsCircle.ManageUserIds.Contains(oprateMageJuridictionUserId))
                {
                    friendsCircle.ManageUserIds.Add(oprateMageJuridictionUserId);
                }
            }
            else
            {
                if (friendsCircle.ManageUserIds.Contains(oprateMageJuridictionUserId))
                {
                    friendsCircle.ManageUserIds.Remove(oprateMageJuridictionUserId);
                }
            }
            await FriendsCircleComponent.Ins.SaveDB(friendsCircle);
        }
        //游戏服玩家打完一大局 发送过来的结算信息 用于排行榜信息
        public static async void RankingGameReult(this FriendsCircle friendsCircle,RepeatedField<TotalPlayerInfo> totalPlayerInfos)
        {
            for (int i = 0; i < totalPlayerInfos.Count; i++)
            {
                RanKingPlayerInfo  ranKingPlayerInfo=await  friendsCircle.QueryRankingInfo(totalPlayerInfos[i].UserId);
                ranKingPlayerInfo.TotalScore += totalPlayerInfos[i].TotalScore;
                ranKingPlayerInfo.TotalNumber++;
                await FriendsCircleComponent.Ins.SaveDB(ranKingPlayerInfo);
            }
        }

    
    
        //查询本亲友圈排行榜信息
        public static async Task<RanKingPlayerInfo>  QueryRankingInfo(this FriendsCircle friendsCircle,long userId)
        {
            List<RanKingPlayerInfo>  ranKingPlayerInfos=await FriendsCircleComponent.Ins.dbProxyComponent.Query<RanKingPlayerInfo>(
                ranking => ranking.UserId == userId && ranking.FriendsCircleId == friendsCircle.FriendsCircleId);
            if (ranKingPlayerInfos.Count == 0)
            {
                ranKingPlayerInfos.Add(RanKingPlayerInfoFactory.Create(friendsCircle.FriendsCircleId, userId));
            }
            return ranKingPlayerInfos[0];
        }
        //获取亲友圈的成员列表
        public static async Task<RepeatedField<long>> GetMemberList(this FriendsCircle friendsCircle)
        {
            FriendsCirleMemberInfo friendsCirleMemberInfo =await FriendsCircleComponent.Ins.QueryFriendsCircleMember(friendsCircle.FriendsCircleId);
            return friendsCirleMemberInfo.MemberList;
        }
    }
}
