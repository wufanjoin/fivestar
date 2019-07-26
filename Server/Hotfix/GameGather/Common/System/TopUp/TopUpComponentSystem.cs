using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class TopUpComponentSystem
    {
        //请求购买商品 如果要微信支付就会返回 如果只是需要钻石就会直接购买
        public static async Task<WeChatOrderInfo>  RequestTopUp(this TopUpComponent topUpComponent, long userId,long commodityId,IResponse iResponse)
        {
            Commodity commodity=ShoppingCommodityComponent.Ins.GetCommdity(commodityId);
            if (commodity.MonetaryType == GoodsId.CNY)
            {
                WeChatOrderInfo weChatOrderInfo=WeChatPayComponent.Ins.WeChatPlaceOrder(commodity.Price);
                TopUpRecord  topUpRecord=TopUpRecordFactory.Create(weChatOrderInfo.outTradeNo, userId, commodity);
                await Game.Scene.GetComponent<DBProxyComponent>().Save(topUpRecord);//存储充值记录
                return weChatOrderInfo;
            }
            else if (commodity.MonetaryType == GoodsId.Jewel)
            {
                User user=await UserHelp.QueryUserInfo(userId);
                if (user.Jewel >= commodity.Price)
                {
                   await user.GoodsChange(GoodsId.Jewel, commodity.Price * -1,GoodsChangeType.ShopPurchase);//减少钻石
                   await user.GoodsChange(commodity.CommodityType, commodity.Amount, GoodsChangeType.ShopPurchase);//增加商品数量
                }
                else
                {
                    iResponse.Message = "钻石不足";
                }
            }
            return null;
        }
        //完成支付
        public static async void FinishPay(this TopUpComponent topUpComponent, string outTradeNo,bool isRepairOrder=false)
        {
            DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            List<TopUpRecord> topUpRecords=await dbProxyComponent.Query<TopUpRecord>(topUpRecord => topUpRecord.OrderId.Equals(outTradeNo));
            if (topUpRecords.Count <= 0)
            {
                Log.Error("没有充值记录 订单id:"+ outTradeNo);
            }
            else
            {
                TopUpRecord topUpRecord = topUpRecords[0];
                if (topUpRecord.TopUpState == TopUpStateType.NoPay)
                {
                    await UserHelp.GoodsChange(topUpRecord.TopUpUserId, topUpRecord.GoodsId, topUpRecord.GoodsAmount, GoodsChangeType.ShopPurchase);
                    topUpRecord.TopUpState = TopUpStateType.AlreadyPay;
                    if (isRepairOrder)
                    {
                        topUpRecord.TopUpState = TopUpStateType.RepairOrder;
                    }
                    await dbProxyComponent.Save(topUpRecord);
                }
                else
                {
                    Log.Error("订单id:" + outTradeNo+"订单已经完成 重复收到完成消息");
                }
         
            }
        }
        //补单 就是完成支付
        public static  void RepairOrder(this TopUpComponent topUpComponent, string outTradeNo,IResponse iResponse)
        {
            if (WeChatPayComponent.Ins.WeChatQueryOrder(outTradeNo))
            {
                topUpComponent.FinishPay(outTradeNo, true);
            }
            else
            {
                iResponse.Message = "未支付,无法补单";
            }
             
        }
    }
}
