using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class WXQueryOrderPost
    {
        public string appid;//微信开放平台审核通过的应用APPID
        public string mch_id;//微信支付分配的商户号
        public string transaction_id;//微信的订单号，优先使用
        public string out_trade_no;//商户系统内部的订单号，当没提供transaction_id时需要传这个
        public string nonce_str;//随机字符串，不长于32位。推荐随机数生成算法
        public string sign;//签名，详见签名生成算法
    }
}
