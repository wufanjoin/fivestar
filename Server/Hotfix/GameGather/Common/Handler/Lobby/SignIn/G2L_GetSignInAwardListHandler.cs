using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_GetSignInAwardListHandler : AMRpcHandler<C2L_GetSignInAwardList, L2C_GetSignInAwardList>
    {
        protected override async void Run(Session session, C2L_GetSignInAwardList message, Action<L2C_GetSignInAwardList> reply)
        {
            L2C_GetSignInAwardList response = new L2C_GetSignInAwardList();
            try
            {
                response.SignInAwardList.AddRange(SingInActivityComponent.Ins.GetSignInAwardList());
                response.UserSinginInfo = await SingInActivityComponent.Ins.GetUserSingInState(message.UserId);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        public bool tsasdc(User s)
        {
            return true;
        }
    }
}