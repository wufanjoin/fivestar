using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
  public  class WeChatOrderInfo:Component
  {
      public string prepayId;//订单id app用于发起的订单id
      public string nonceStr;//随机字符串
      public string outTradeNo;//商户内部订单id 也是查询id
  }
}
