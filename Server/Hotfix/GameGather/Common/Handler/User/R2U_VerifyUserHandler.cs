using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
 public class R2U_VerifyUserHandler : AMRpcHandler<R2U_VerifyUser, U2R_VerifyUser>
    {
        protected  override async void Run(Session session, R2U_VerifyUser message, Action<U2R_VerifyUser> reply)
        {
            U2R_VerifyUser response = new U2R_VerifyUser();
            try
            {
                AccountInfo accountInfo = (await Game.Scene.GetComponent<UserComponent>().LoginOrRegister(message.DataStr, message.LoginType));
                if (accountInfo == null)
                {
                    response.Message = "登陆验证出错,请重新登陆";
                    reply(response);
                    return;
                }
                if (accountInfo.IsStopSeal)
                {
                    response.Message = $"ID:{accountInfo.UserId}账号已被停封,请联系客服QQ:470499850";
                    reply(response);
                    return;
                }
                response.UserId = accountInfo.UserId;
                response.Password = accountInfo.Password;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
