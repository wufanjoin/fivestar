using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class WeChatPayComponentAwakeSystem : AwakeSystem<WeChatPayComponent>
    {
        public override void Awake(WeChatPayComponent self)
        {
            new MonitorWxPayHttp();
        }
    }
    public static  class WeChatPayComponentSystem
    {
        /// <summary>
        /// 微信下单
        /// </summary>
        /// <param name="money">金额 单位元</param>
        public static WeChatOrderInfo WeChatPlaceOrder(this WeChatPayComponent weChatPayComponent,float money)
        {
            return PlaceOrder.WeChatPlaceOrder(money);
        }

        /// <summary>
        /// 查询订单情况 true代表付款成功
        /// </summary>
        /// <param name="outTradeNoId"></param>
        /// <returns></returns>
        public static bool WeChatQueryOrder(this WeChatPayComponent weChatPayComponent,string outTradeNoId)
        {
            return PlaceOrder.QueryOrder(outTradeNoId);
        }

        /// <summary>
        /// 订单完成通知
        /// </summary>
        /// <param name="outTradeNoId"></param>
        /// <returns></returns>
        public static void WeChatQueryOrder(this WeChatPayComponent weChatPayComponent, string outTradeNoId,bool isPay)
        {
           
        }

    }
}
