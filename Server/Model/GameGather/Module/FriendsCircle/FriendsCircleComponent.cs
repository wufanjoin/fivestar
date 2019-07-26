using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;


namespace ETModel
{
    [ObjectSystem]
    public class FriendsCircleComponentAwakeSystem : AwakeSystem<FriendsCircleComponent>
    {
        public override void Awake(FriendsCircleComponent self)
        {
            self.Awake();
        }
    }
    public class FriendsCircleComponent : Component
    {
        public DBProxyComponent dbProxyComponent;
        public static FriendsCircleComponent Ins;
        public List<FriendsCircle> RecommendFriendsCircleList;//推荐亲友圈列表 就是可以推荐给陌生人的
        public int FriendsCircleMaxId=110211;//目前亲友圈最大ID

        public async void  Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            RecommendFriendsCircleList = await dbProxyComponent.Query<FriendsCircle>(friendsCircle => friendsCircle.IsRecommend);
           List<FriendsCircle> friendsCircles=await dbProxyComponent.SortQuery<FriendsCircle>(friendsCircle => true,
                friendsCircle => friendsCircle.FriendsCircleId == -1, 1);
            if (friendsCircles.Count > 0)
            {
                FriendsCircleMaxId = friendsCircles[0].FriendsCircleId;
            }
        }
    }
}
