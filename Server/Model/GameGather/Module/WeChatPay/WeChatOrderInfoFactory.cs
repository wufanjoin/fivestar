using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ETModel
{
  public  class WeChatOrderInfoFactory
    {
        public static WeChatOrderInfo Creator(string prepayId, string nonceStr,string outTradeNo)
        {
            WeChatOrderInfo weChatOrderInfo = ComponentFactory.Create<WeChatOrderInfo>();
            weChatOrderInfo.prepayId = prepayId;
            weChatOrderInfo.nonceStr = nonceStr;
            weChatOrderInfo.outTradeNo = outTradeNo;
            return weChatOrderInfo;
        }
    }
}
