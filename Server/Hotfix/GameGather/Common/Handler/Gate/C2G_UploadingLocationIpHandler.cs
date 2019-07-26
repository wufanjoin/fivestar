using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 上传定位和IP信息
    /// </summary>
    [MessageHandler(AppType.Gate)]
    public class C2G_UploadingLocationIpHandler : AMRpcHandler<C2G_UploadingLocationIp, S2C_UploadingLocationIp>
    {
        protected override  void Run(Session session, C2G_UploadingLocationIp message, Action<S2C_UploadingLocationIp> reply)
        {
            S2C_UploadingLocationIp response=new S2C_UploadingLocationIp();
            try
            {
                //G2U_UploadingLocationIp g2UUploadingLocationIp=new G2U_UploadingLocationIp();
                //g2UUploadingLocationIp.UserId = session.GetComponent<SessionUserComponent>().UserId;
                //g2UUploadingLocationIp.Location = message.Location;
                //g2UUploadingLocationIp.Ip = session.RemoteAddress.Address.ToString();
                //Session userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
                //response = (S2C_UploadingLocationIp)await userSession.Call(g2UUploadingLocationIp);
                session.GetComponent<SessionUserComponent>().user.Ip= session.RemoteAddress.Address.ToString();//IP和地址 信息只用网关记录 就行 
                session.GetComponent<SessionUserComponent>().user.Location = message.Location;
                response.Ip = session.RemoteAddress.Address.ToString();
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}