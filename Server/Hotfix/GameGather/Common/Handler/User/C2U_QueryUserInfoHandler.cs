using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询用户信息
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_QueryUserInfoHandler : AMRpcHandler<C2U_QueryUserInfo, U2C_QueryUserInfo>
    {
        protected override async void Run(Session session, C2U_QueryUserInfo message, Action<U2C_QueryUserInfo> reply)
        {
            U2C_QueryUserInfo response = new U2C_QueryUserInfo();
            try
            {
                User user=await UserComponent.Ins.Query(message.QueryUserId);
                if (user == null)
                {
                    response.Message = "没有该用户";
                    reply(response);
                    return;
                }
                response.User = user;
                List<AccountInfo> accountInfos=await UserComponent.Ins.dbProxyComponent.Query<AccountInfo>(
                    accountInfo => accountInfo.UserId == message.QueryUserId);
                if (accountInfos.Count > 0)
                {
                    response.IsStopSeal = accountInfos[0].IsStopSeal;
                    response.LastLoginTime = accountInfos[0].LastLoginTime;
                }
                else
                {
                    response.Message = "查询账号信息错误";
                }
                //查询代理等级
                List<AgecyInfo> agecyInfos = await UserComponent.Ins.dbProxyComponent.Query<AgecyInfo>(agecyInfo => agecyInfo.UserId == message.QueryUserId);
                if (agecyInfos.Count>0)
                {
                    response.AgecyLv = agecyInfos[0].Level;
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