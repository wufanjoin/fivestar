using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
  public static class AgencyComponentSystem
    {
        //代理卖钻石
        public static async Task SaleJewel(this AgencyComponent agencyComponent,long sellerUserId,long buyerUserId,int jewelNum, IResponse iResponse)
        {
            if (!agencyComponent.JudgeIsAgency(sellerUserId))
            {
                iResponse.Message = "权限不足";
                return;
            }
            if (sellerUserId == buyerUserId)
            {
                iResponse.Message = "不能卖给自己";
                return;
            }
            User sellerUser= await UserHelp.QueryUserInfo(sellerUserId);
            User buyerUser = await UserHelp.QueryUserInfo(buyerUserId);
            if (buyerUser==null)
            {
                iResponse.Message = "买家不存在";
                return;
            }
            if (sellerUser.Jewel >= jewelNum)
            {
                UserHelp.GoodsChange(sellerUserId, GoodsId.Jewel, jewelNum*-1, GoodsChangeType.AgencyDeal);
                UserHelp.GoodsChange(buyerUserId, GoodsId.Jewel, jewelNum, GoodsChangeType.AgencyDeal);
                agencyComponent.RecordMarketInfo(sellerUserId, buyerUser, jewelNum);
                iResponse.Message = "销售成功";
            }
            else
            {
                iResponse.Message = "钻石不足";
            }
        }
        //获取是不是代理
        public static bool JudgeIsAgency(this AgencyComponent agencyComponent, long uerId)
        {
            return agencyComponent.AgecyUserIdList.Contains(uerId);
        }
        //获取销售记录
        public static async Task<List<MarketInfo>> GetMarketRecord(this AgencyComponent agencyComponent, long userId, IResponse iResponse)
        {
            if (!agencyComponent.JudgeIsAgency(userId))
            {
                iResponse.Message = "权限不足";
                return null;
            }
            List<MarketInfo> records = await agencyComponent.dbProxyComponent.Query<MarketInfo>((record) => record.SellUserId == userId);
            return records;
        }
        //记录销售信息
        public static async void RecordMarketInfo(this AgencyComponent agencyComponent, long sellerUserId, User buyerUser,int jewelNum)
        {
            MarketInfo marketInfo=ComponentFactory.Create<MarketInfo>();
            marketInfo.SellUserId = sellerUserId;
            marketInfo.MaiJiaUserId = buyerUser.UserId;
            marketInfo.MaiJiaName = buyerUser.Name;
            marketInfo.JewelNum = jewelNum;
            marketInfo.Time = TimeTool.GetCurrenTimeStamp();
            await agencyComponent.dbProxyComponent.Save(marketInfo);
            marketInfo.Dispose();
        }

        //更改代理等级
        public static async void AlterAgencyLv(this AgencyComponent agencyComponent, long userId, int lv)
        {
            List<AgecyInfo> agecyInfos=await agencyComponent.dbProxyComponent.Query<AgecyInfo>(agecyInfo => agecyInfo.UserId== userId);
            if (agecyInfos.Count <= 0)
            {
                AgecyInfo agecyInfo = ComponentFactory.Create<AgecyInfo>();
                agecyInfo.UserId = userId;
                agecyInfos.Add(agecyInfo);
            }
            agecyInfos[0].Level = lv;
            await  agencyComponent.dbProxyComponent.Save(agecyInfos[0]);
            //设置为0级 就是撤销
            if (lv <= 0)
            {
                if (agencyComponent.AgecyUserIdList.Contains(userId))
                {
                    agencyComponent.AgecyUserIdList.Remove(userId);
                }
            }
            else
            {
                //不是0级就是设置代理
                if (!agencyComponent.AgecyUserIdList.Contains(userId))
                {
                    agencyComponent.AgecyUserIdList.Add(userId);
                }
            }
        }
    }
}
