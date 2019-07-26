using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
  public static class TurntableComponentSystem
    {
        //获取剩余免费抽奖次数
        public static async Task<int>  GetFreeDrawLotteryCount(this TurntableComponent turntableComponent, long userId)
        {
            List<FreeDrawLottery> freeDrawInfo = await turntableComponent.dbProxyComponent.Query<FreeDrawLottery>((free) => free.UserId == userId);
            if (freeDrawInfo.Count > 0)
            {
               return freeDrawInfo[0].Count;
            }
            return 0;
        }
        //玩家完成一局游戏 增加每日抽奖的次数
        public static async void FinishTaskAddLotteryCount(this TurntableComponent turntableComponent,IList<long> userIds)
        {
            //如果本来有免费 抽奖次数信息 就直接加次数
            List<FreeDrawLottery> freeDrawInfos = await turntableComponent.dbProxyComponent.Query<FreeDrawLottery>((free) => userIds.Contains(free.UserId));
            for (int i = 0; i < freeDrawInfos.Count; i++)
            {
                if (!TimeTool.TimeStampIsToday(freeDrawInfos[i].UpAddFreeDrawLotteryTime))
                {
                    freeDrawInfos[i].Count++;
                }
            }
            await turntableComponent.dbProxyComponent.Save(freeDrawInfos);
            if (userIds.Count == freeDrawInfos.Count)
            {
                return;
            }
            //如果有的玩家之前没有记录 就创建 免费抽奖信息
            for (int i = 0; i < userIds.Count; i++)
            {
                if (freeDrawInfos.UserIdIsExist(userIds[i]))
                {
                    
                }
                else
                {
                    FreeDrawLottery freeDrawLottery = FreeDrawLotteryFactory.Create(userIds[0]);
                    await turntableComponent.dbProxyComponent.Save(freeDrawLottery);
                }
            }
            
        }
        //判断免费抽奖信息列表里面 是否存在userid
        private static bool UserIdIsExist(this List<FreeDrawLottery> freeDrawLotteries,long userId)
        {
            for (int i = 0; i < freeDrawLotteries.Count; i++)
            {
                if (freeDrawLotteries[i].UserId == userId)
                {
                    return true;
                }
            }
            return false;
        }
        //获取中奖记录
        public static async Task<List<WinPrizeRecord>> GetWinPrizeRecord(this TurntableComponent turntableComponent, long userId)
        {
            List< WinPrizeRecord >  records=await turntableComponent.dbProxyComponent.Query<WinPrizeRecord>((record) => record.UserId == userId);
            return records;
        }
        //获取奖品列表
        public static RepeatedField<TurntableGoods> GetTurntableGoodss(this TurntableComponent turntableComponent)
        {
            return turntableComponent.mTurntableGoodsesRepeatedField;
        }

        public const int DarwLotteryConsumeJewelNum = 10;//抽奖消耗的钻石数量
        //玩家抽奖
        public static async Task<TurntableGoods>  DarwLottery(this TurntableComponent turntableComponent,long userId, IResponse response)
        {
           
            List<FreeDrawLottery> freeDrawInfo=await turntableComponent.dbProxyComponent.Query<FreeDrawLottery>((free) => free.UserId == userId);
           
            if (freeDrawInfo.Count>0&&freeDrawInfo[0].Count> 0)
            {
                freeDrawInfo[0].Count--;
                await turntableComponent.dbProxyComponent.Save(freeDrawInfo[0]);
                return turntableComponent.RandomDarwLottery(userId);
            }
            else
            {
                User user = await UserHelp.QueryUserInfo(userId);
            
                if (user.Jewel >= DarwLotteryConsumeJewelNum)
                {
                    UserHelp.GoodsChange(userId, GoodsId.Jewel , DarwLotteryConsumeJewelNum * -1,GoodsChangeType.DrawLottery);//改变物品给用户服
                    return turntableComponent.RandomDarwLottery(userId);
                }
                else
                {
                    response.Error = 1;
                    response.Message = "钻石不足";
                }
            }
            return null;
        }
        //直接随机出现一个抽奖物品
        public static TurntableGoods RandomDarwLottery(this TurntableComponent turntableComponent, long userId)
        {
           int randNum=RandomTool.Random(0, 100);//默认概率是 100 所有物品的概率加起来也是要100
            int currProbability = 0;
            for (int i = 0; i < turntableComponent.mTurntableGoodses.Count; i++)
            {
                currProbability += turntableComponent.mTurntableGoodses[i].Probability;
                if (randNum < currProbability)
                {
                    if (turntableComponent.mTurntableGoodses[i].GoodsId != GoodsId.None)
                    {
                        UserHelp.GoodsChange(userId, turntableComponent.mTurntableGoodses[i].GoodsId
                            ,turntableComponent.mTurntableGoodses[i].Amount, GoodsChangeType.DrawLottery);//改变物品给用户服
                    }
                    turntableComponent.RecordWinPrizeInfo(userId, turntableComponent.mTurntableGoodses[i]);//记录中奖信息
                    return turntableComponent.mTurntableGoodses[i];
                }
            }
            Log.Error("概率随机出现错误");
            return null;
        }
        //记录中奖信息
        public static async void RecordWinPrizeInfo(this TurntableComponent turntableComponent, long userId,
            TurntableGoods goods)
        {
            if (goods.GoodsId == GoodsId.None)
            {
                return;
            }
            WinPrizeRecord winPrizeRecord=ComponentFactory.Create<WinPrizeRecord>();
            if (goods.GoodsId == GoodsId.Besans || goods.GoodsId == GoodsId.Jewel)
            {
                winPrizeRecord.Type = 1;//默认Type是0 其他就是兑奖了
            }
            winPrizeRecord.WinPrizeId = turntableComponent.GetWinPrizeId();//获取中奖记录Id
            winPrizeRecord.UserId = userId;
            winPrizeRecord.Time = TimeTool.GetCurrenTimeStamp();
            winPrizeRecord.Amount = goods.Amount;
            winPrizeRecord.GoodsId = goods.GoodsId;
            await turntableComponent.dbProxyComponent.Save(winPrizeRecord);
        }
    }
}
