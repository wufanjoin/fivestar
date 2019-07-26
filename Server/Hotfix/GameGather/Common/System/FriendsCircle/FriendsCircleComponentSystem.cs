using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
 public static class FriendsCircleComponentSystem
    {
        //创建亲友圈
        public static async Task<FriendsCircle> CreatorFriendsCircle(this FriendsCircleComponent friendsCircleComponent,
            long creatorUserId,string name,string announcement,RepeatedField<int> roomConfigs,long toyGameId, IResponse iResponse)
        {
            User creatorUser = await UserHelp.QueryUserInfo(creatorUserId);
            if (creatorUser.Jewel < 200)
            {
                iResponse.Message = "钻石少于200无法创建亲友圈";
                return null;
            }
            //效验配置 如果配置错误 会使用默认配置
            if (!RoomConfigIntended.IntendedRoomConfigParameter(roomConfigs, toyGameId))
            {
                iResponse.Message = "玩法配置错误 无法创建";
                return null;
            }
            FriendsCircle friendsCircle=FriendsCircleFactory.Create(name, creatorUserId, roomConfigs, announcement);
            await  friendsCircle.SucceedJoinFriendsCircle(creatorUserId);//成功加入到亲友圈 这个方法和保存亲友圈数据到数据库
            return friendsCircle;
        }

        //获取推荐亲友圈信息
        public static async Task<List<FriendsCircle>>  GetRecommendFriendsCircle(this FriendsCircleComponent friendsCircleComponent,int startIndex,int count)
        {
            if (friendsCircleComponent.RecommendFriendsCircleList.Count > startIndex + count)
            {
               return friendsCircleComponent.RecommendFriendsCircleList.GetRange(startIndex, count);
            }
            else
            {
                if (friendsCircleComponent.RecommendFriendsCircleList.Count >= count)
                {
                    return friendsCircleComponent.RecommendFriendsCircleList.GetRange(friendsCircleComponent.RecommendFriendsCircleList.Count - count, count);
                }
                else
                {
                    return friendsCircleComponent.RecommendFriendsCircleList;
                }
            }
        
        }
        //修改推荐亲友圈配置
        public static void AlterRecommend(this FriendsCircleComponent friendsCircleComponent, FriendsCircle friendsCircle, bool isRecommend)
        {
            if (isRecommend)
            {
                if (!FriendsCircleComponent.Ins.RecommendFriendsCircleList.Contains(friendsCircle))
                {
                    FriendsCircleComponent.Ins.RecommendFriendsCircleList.Add(friendsCircle);
                }
            }
            else
            {
                if (FriendsCircleComponent.Ins.RecommendFriendsCircleList.Contains(friendsCircle))
                {
                    FriendsCircleComponent.Ins.RecommendFriendsCircleList.Remove(friendsCircle);
                }
            }
        }
        //玩家退出亲友圈
        public static async void UserOutFriendsCircle(this FriendsCircleComponent friendsCircleComponent, long userId, int friendsCircleId)
        {
            FriendsCircle friendsCircle = await friendsCircleComponent.QueryFriendsCircle(friendsCircleId);
          
        }
        //存储到数据库
        public static  async Task SaveDB(this FriendsCircleComponent friendsCircleComponent, ComponentWithId component)
        {
            await friendsCircleComponent.dbProxyComponent.Save(component);
        }
        
        
        //获得新创建的亲友圈Id
        public static int GetNewFriendsCircleId(this FriendsCircleComponent friendsCircleComponent)
        {
            return ++friendsCircleComponent.FriendsCircleMaxId;
        }
    }
}
