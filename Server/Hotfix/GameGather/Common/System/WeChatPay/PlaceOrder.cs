using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ETModel;

namespace ETHotfix
{
    
    public static  class PlaceOrder
    {
        private const string PlaceOrderUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        private const string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
        private const string Pay_App_Id = "wxd22448210abef405";//支付应该的appId
        private const  string  PartnerId = "1542580991";//商户号
        private const string PartnerKey ="123456789123456789123456789abcde";//商户秘钥

        //-----------------------微信查询订单代码----------------------
        public const string Order_SUCCESS = "SUCCESS";//支付成功
        public const string Order_REFUND = "REFUND";//转入退款
        public const string Order_NOTPAY = "NOTPAY";//未支付
        public const string Order_CLOSED = "CLOSED";//已关闭
        public const string Order_REVOKED = "REVOKED";//已撤销（刷卡支付）
        public const string Order_USERPAYING = "USERPAYING";//用户支付中
        public const string Order_PAYERROR = "PAYERROR";//支付失败(其他原因，如银行返回失败)
        //查询订单
        public static bool QueryOrder(string outTradeNoId)
        {
            WXQueryOrderPost post = new WXQueryOrderPost();
            post.appid = Pay_App_Id;
            post.mch_id = PartnerId;
            post.out_trade_no = outTradeNoId;
            post.nonce_str= genNonceStr(32);//随机字符串  **1 不超过32位
            post.sign = GetQueryOrderPostSign(post);

            Dictionary<string,string> firstSignParams = GetQueryOrderParams(post);
            string xmlParams = toXml(firstSignParams);
            string callStr = HttpPost.Post(OrderQueryUrl, xmlParams);
           // Log.Debug("查询订单:");
           // Log.Debug(callStr);
            try
            {
                XElement xe = XElement.Parse(callStr);
                XElement tradeStatElement = xe.Element("trade_state");
                if (tradeStatElement==null)
                {
                    return false;

                }
                return tradeStatElement.Value == Order_SUCCESS;
            }
            catch (Exception e)
            {
                Log.Error(e+"查询微信支付订单失败");
                throw;
            }
        }
        //得到查询xml签名
        private static string GetQueryOrderPostSign(WXQueryOrderPost postParams)
        {
            //拼接排序list
            Dictionary<string, string> packageParams = new Dictionary<string, string>();
            packageParams.Add("appid", postParams.appid);
            packageParams.Add("mch_id", postParams.mch_id);
            packageParams.Add("nonce_str", postParams.nonce_str);
            packageParams.Add("out_trade_no", postParams.out_trade_no);
            StringBuilder sb = new StringBuilder();

            foreach (var packageParam in packageParams)
            {
                sb.Append(packageParam.Key);
                sb.Append('=');
                sb.Append(packageParam.Value);
                sb.Append('&');
            }
            sb.Append("key=");
            sb.Append(PartnerKey);//key设置路径：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
            //这里又用到了从实例代码复制的MD5 可以去上面copy
            String packageSign = MD5Tool.GetMD5(sb.ToString()).ToUpper();
            return packageSign;
        }
        private static Dictionary<string, string> GetQueryOrderParams(WXQueryOrderPost postParams)
        {
            Dictionary<string, string> packageParams = new Dictionary<string, string>();
            packageParams.Add("appid", postParams.appid);
            packageParams.Add("mch_id", postParams.mch_id);
            packageParams.Add("out_trade_no", postParams.out_trade_no);
            packageParams.Add("nonce_str", postParams.nonce_str);
            packageParams.Add("sign", postParams.sign);
            return packageParams;
        }

        //-----------------------微信下单代码----------------------
        //微信统一下单
        public static WeChatOrderInfo WeChatPlaceOrder(float money)
        {
            WXPrePost post = new WXPrePost();
            post.appid = Pay_App_Id;
            post.mch_id = PartnerId;
            post.nonce_str = genNonceStr(32);//随机字符串  **1 不超过32位
            post.body = "商品购买";//商品标题
            post.detail = "用于购买商品";//详细信息
            post.out_trade_no = TopUpComponent.GetTradeNo(); //商户订单号 **2
            post.total_fee = (int)(money*100);//单位是分
            post.spbill_create_ip = "127.0.0.1";//ip地址  **3
            
            post.notify_url = GetOuterPostAddress();//这里是后台接受支付结果通知的url地址
            post.trade_type = "APP";
            post.sign = genPackageSign(post);//签名

            Dictionary<string, string> firstSignParams = getFirstSignParams(post);
            string xmlParams = toXml(firstSignParams);

            string callStr = HttpPost.Post(PlaceOrderUrl, xmlParams);

           // Console.WriteLine("收到下单回调:");
           // Console.WriteLine(callStr);
            XElement xe = XElement.Parse(callStr);
            string prepayId = xe.Element("prepay_id").Value;
            string nonceStr= xe.Element("nonce_str").Value;
            // Console.WriteLine("prepay_id:"+ prepayId);
            // Console.WriteLine("nonce_str:" + nonceStr);
           return WeChatOrderInfoFactory.Creator(prepayId, nonceStr, post.out_trade_no);
        }

        private static string outerPostAddressStr=string.Empty;
        //获取微信Post外网地址
        public static string GetOuterPostAddress()
        {
            if (string.IsNullOrEmpty(outerPostAddressStr))
            {
                ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
                HttpListenerAddressConfig httpListenerAddressConfig = (HttpListenerAddressConfig)configComponent.Get(typeof(HttpListenerAddressConfig), 0);
                outerPostAddressStr = httpListenerAddressConfig.OuterPostAddress;
            }
            return outerPostAddressStr;
        }


        private static string toXml(Dictionary<string, string> postParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var postParam in postParams)
            {
                sb.Append("<" + postParam.Key + ">");
                sb.Append(postParam.Value);
                sb.Append("</" + postParam.Key + ">");
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
        private static Dictionary<string,string> getFirstSignParams(WXPrePost postParams)
        {
            Dictionary<string, string> packageParams =new Dictionary<string, string>();
            packageParams.Add("appid", postParams.appid);
            packageParams.Add("body", postParams.body);
            packageParams.Add("detail", postParams.detail);
            packageParams.Add("mch_id", postParams.mch_id);
            packageParams.Add("nonce_str", postParams.nonce_str);
            packageParams.Add("notify_url", postParams.notify_url);
            packageParams.Add("out_trade_no", postParams.out_trade_no);
            packageParams.Add("spbill_create_ip", postParams.spbill_create_ip);
            packageParams.Add("total_fee", postParams.total_fee + "");
            packageParams.Add("trade_type", postParams.trade_type);
            packageParams.Add("sign", postParams.sign);
            return packageParams;
        }
        public static char[]  noceStrPool=new char[]
        {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c',
            'd','e','f','g','h','i','k','p','o','z','x','c','d','n','m'
        };
        private static string genNonceStr(int lenth)
        {
            string noceStr = "";
            Random random=new Random();
            for (int i = 0; i < lenth; i++)
            {
                noceStr+= noceStrPool[random.Next(0, noceStrPool.Length)];
            }
            return noceStr;
        }

        private static string genPackageSign(WXPrePost postParams)
        {
            //拼接排序list
            Dictionary<string,string> packageParams=new Dictionary<string, string>();
            packageParams.Add("appid", postParams.appid);
            packageParams.Add("body", postParams.body);
            packageParams.Add("detail", postParams.detail);
            packageParams.Add("mch_id", postParams.mch_id);
            packageParams.Add("nonce_str", postParams.nonce_str);
            packageParams.Add("notify_url", postParams.notify_url);
            packageParams.Add("out_trade_no", postParams.out_trade_no);
            packageParams.Add("spbill_create_ip", postParams.spbill_create_ip);
            packageParams.Add("total_fee", postParams.total_fee + "");
            packageParams.Add("trade_type", postParams.trade_type);
            StringBuilder sb = new StringBuilder();

            foreach (var packageParam in packageParams)
            {
                sb.Append(packageParam.Key);
                sb.Append('=');
                sb.Append(packageParam.Value);
                sb.Append('&');
            }
            sb.Append("key=");
            sb.Append(PartnerKey);//key设置路径：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
            //这里又用到了从实例代码复制的MD5 可以去上面copy
            String packageSign = MD5Tool.GetMD5(sb.ToString()).ToUpper();
            return packageSign;
        }
    }
}
