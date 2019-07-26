using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;

namespace ETModel
{
    public static class UserComponentGetGoodsSystem
    {
        public static async Task<User> UserGetGoods(this UserComponent self, long userId, List<GetGoodsOne> goodsOnes,int goodsChangeType, bool isShowHintPanel)
        {
            bool userOnline = true;
            User user = self.GetUser(userId);
            if (user == null)
            {
                user = await self.Query(userId);
                userOnline = false;
            }
            if (user == null)
            {
                Log.Error("要增加物品的玩家更本不存在UserId:" + userId);
                return null;
            }
            
            user.UpdataGoods(goodsOnes);//更新物品数量
            self.SaveGoodsDealRecord(user, goodsOnes, goodsChangeType); //存储物品 变化记录 只会存储钻石的
            await self.SaveUserDB(user);
            if (userOnline)
            {
                Actor_UserGetGoods actorUserGetGoods = new Actor_UserGetGoods();

                foreach (var goods in goodsOnes)
                {
                    switch (goods.GoodsId)
                    {
                        case GoodsId.Besans:
                            goods.NowAmount = user.Beans;
                            break;
                        case GoodsId.Jewel:
                            goods.NowAmount = user.Jewel;
                            break;
                    }
                }
                actorUserGetGoods.GetGoodsList.AddRange(goodsOnes.ToArray());
                actorUserGetGoods.IsShowHintPanel = isShowHintPanel;
                //向User发送Actor
                user.SendeSessionClientActor(actorUserGetGoods);
            }
            return user;
        }

        //存储物品 变化记录 只会存储钻石的
        public static async void SaveGoodsDealRecord(this UserComponent self, User user, List<GetGoodsOne> goodsOnes,int changeType)
        {
            for (int i = 0; i < goodsOnes.Count; i++)
            {
                if (goodsOnes[i].GoodsId != GoodsId.Jewel)
                {
                    continue;
                }
                GoodsDealRecord goodsDealRecord = ComponentFactory.Create<GoodsDealRecord>();
                goodsDealRecord.UserId = user.UserId;
                goodsDealRecord.Amount = goodsOnes[i].GetAmount;
                goodsDealRecord.Type = changeType;
                goodsDealRecord.Time = TimeTool.GetCurrenTimeStamp();
                goodsDealRecord.FinishNowAmount = (int)user.Jewel;
                await self.dbProxyComponent.Save(goodsDealRecord);
            }
           
        }
    }
}
