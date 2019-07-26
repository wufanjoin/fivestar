using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
   public static class UserHelp
    {
        public static async Task<User> QueryUserInfo(long userId)
        {
            List<User> users = await Game.Scene.GetComponent<DBProxyComponent>()
                .Query<User>(user => user.UserId == userId);
            if (users.Count > 0)
            {
               return users[0];
            }
            return null;
        }
        public static async Task<List<User>> QueryUserInfo(RepeatedField<long> userId)
        {
            List<User> users = await Game.Scene.GetComponent<DBProxyComponent>()
                .Query<User>(user => userId.Contains(user.UserId));
            return users;
        }
        //封号或者解封
        public static void StopSealOrRelieve(long userId,bool isStopSeal,string explain)
        {
            Session userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
            //直接封号
            StopSealRecord stopSealRecord = new StopSealRecord();
            stopSealRecord.StopSealUserId = userId;
            stopSealRecord.IsStopSeal = isStopSeal;
            stopSealRecord.Explain = explain;
            userSession.Call(new C2U_SetIsStopSeal() { StopSeal = stopSealRecord });
        }
        //改变用户物品
        public static async Task<User> GoodsChange(long userId,long goodId, int amount, int changeType, bool isShowHint = false)
        {
            GetGoodsOne getGoodsOne = ComponentFactory.Create<GetGoodsOne>();
            getGoodsOne.SetGoodsOne(goodId, amount);
            List<GetGoodsOne> getGoodsOnes = new List<GetGoodsOne>() { getGoodsOne };
            User user=await  GoodsChange(userId,getGoodsOnes, changeType, isShowHint);
            getGoodsOne.Dispose();
            return user;
        }
        //改变用户物品
        public static async Task<User> GoodsChange(long userId, List<GetGoodsOne> getGoodsOnes,int changeType, bool isShowHint = false)
        {
            Session userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
            U2S_UserGetGoods u2SUserGetGoods = (U2S_UserGetGoods)await userSession.Call(new S2U_UserGetGoods()
            {
                UserId = userId,
                GetGoodsList = getGoodsOnes,
                isShowHintPanel = isShowHint,
                GoodsChangeType = changeType
            });
            return u2SUserGetGoods.user;
        }
    }
}
