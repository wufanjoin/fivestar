using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 上传定位和IP信息
    /// </summary>
    [MessageHandler(AppType.User)]
    public class G2U_UploadingLocationIpHandler : AMRpcHandler<G2U_UploadingLocationIp, S2C_UploadingLocationIp>
    {
        protected override async void Run(Session session, G2U_UploadingLocationIp message, Action<S2C_UploadingLocationIp> reply)
        {
            S2C_UploadingLocationIp response = new S2C_UploadingLocationIp();
            try
            {
                User  user=UserComponent.Ins.GetUser(message.UserId);
                if (user != null)
                {
                    user.Location = message.Location;
                    user.Ip = message.Ip;
                    await UserComponent.Ins.SaveUserDB(user);
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}