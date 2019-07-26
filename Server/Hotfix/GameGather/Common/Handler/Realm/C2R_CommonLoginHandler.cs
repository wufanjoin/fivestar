using System;
using System.Net;
using ETModel;
using Google.Protobuf;
using MongoDB.Bson;
 
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_CommonLoginHandler : AMRpcHandler<C2R_CommonLogin, R2C_CommonLogin>
    {
        protected override async void Run(Session session, C2R_CommonLogin message, Action<R2C_CommonLogin> reply)
        {
            R2C_CommonLogin response = new R2C_CommonLogin();
            try
            {
                //向用户服验证（注册/登陆）并获得一个用户ID
                Session userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
                U2R_VerifyUser  u2RVerifyUser=(U2R_VerifyUser)await userSession.Call(new R2U_VerifyUser()
                {
                    LoginType = message.LoginType,
                    PlatformType = message.PlatformType,
                    DataStr = message.DataStr,
                   // IpAddress=session.RemoteAddress.Address.ToString(),
                });
                //如果Message不为空 说明 验证失败
                if (!string.IsNullOrEmpty(u2RVerifyUser.Message))
                {
                    response.Message = u2RVerifyUser.Message;
                    reply(response);
                    return;
                }
                // 随机分配一个Gate
                StartConfig config = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
                IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
                // 向gate请求一个key,客户端可以拿着这个key连接gate
                G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await gateSession.Call(new R2G_GetLoginKey() { UserId = u2RVerifyUser.UserId });

                string outerAddress = config.GetComponent<OuterConfig>().Address2;
                response.Address = outerAddress;
                response.Key = g2RGetLoginKey.Key;
                response.LoginVoucher = u2RVerifyUser.UserId.ToString() + '|' + u2RVerifyUser.Password;
                
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}