using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    public class M2U_GetAIUserHandler : AMRpcHandler<M2U_GetAIUser, U2M_GetAIUser>
    {
        protected override async void Run(Session session, M2U_GetAIUser message, Action<U2M_GetAIUser> reply)
        {
            U2M_GetAIUser response = new U2M_GetAIUser();
            try
            {
                UserComponent userComponent = Game.Scene.GetComponent<UserComponent>();
                response.users =await userComponent.dbProxyComponent.SortQuery<User>(user => true, user => user.Beans==1,100);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
