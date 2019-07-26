
using System.Collections.Generic;

using System.Threading.Tasks;
using ETModel;


namespace ETHotfix
{
    public static class GateUserComponentSystem 
    {
        //获取当前网关连接User中的信息
        public static User GetUser(this GateUserComponent gateUserComponent, long userId)
        {
            User user;
            if (!gateUserComponent.mUserDic.TryGetValue(userId, out user))
            {
                //Log.Error($"玩家{userId}不存在或不在游戏中");
            }
            return user;
        }

        //玩家上线事件
        public static async Task<User> UserOnLine(this GateUserComponent gateUserComponent, long userId, long sessionActorId)
        {
            User user=await UserHelp.QueryUserInfo(userId);
            if (user == null)
            {
                return null;
            }
            user.IsOnLine = true;//改变在线状态
            //给其他服务器广播玩家上线消息
            gateUserComponent.BroadcastOnAndOffLineMessage( new G2S_UserOnline() {UserId = userId, SessionActorId = sessionActorId});
            //记录玩家信息
            gateUserComponent.mUserDic[userId] = user;
            return user;
        }
       
        //玩家下线事件
        public static void UserOffline(this GateUserComponent gateUserComponent, long userId)
        {
            long gamerSeesionActorId = 0;
            if (gateUserComponent.mUserDic.ContainsKey(userId))
            {
                gamerSeesionActorId = gateUserComponent.mUserDic[userId].GetUserClientSession().GetComponent<SessionUserComponent>().GamerSessionActorId;
            }
            if (gamerSeesionActorId != 0)
            {
                ActorHelp.SendeActor(gamerSeesionActorId, new Actor_UserOffLine());//告诉游戏服 用户下线
            }
            if (gateUserComponent.mUserDic.ContainsKey(userId))
            {
                gateUserComponent.mUserDic.Remove(userId);
            }
            //给其他服务器广播玩家下线消息
            gateUserComponent.BroadcastOnAndOffLineMessage(new G2S_UserOffline() {UserId = userId});
        }
        //给其他服务器广播用户 上 下线消息
        public static void BroadcastOnAndOffLineMessage(this GateUserComponent gateUserComponent,IMessage iMessage)
        {
            AppType appType = StartConfigComponent.Instance.StartConfig.AppType;
            if (appType==AppType.AllServer)
            {
                gateUserComponent.MatchSession.Send(iMessage);
            }
            else
            {
                gateUserComponent.UserSession.Send(iMessage);
                gateUserComponent.MatchSession.Send(iMessage);
            }
        }
    }
}
