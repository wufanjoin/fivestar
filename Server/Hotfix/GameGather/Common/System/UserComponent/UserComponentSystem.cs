using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
 public static class UserComponentSystem
    {
        public static User GetUser(this UserComponent userComponent, long userId)
        {
            User user;
            if (!userComponent.mOnlineUserDic.TryGetValue(userId, out user))
            {
                //Log.Error($"玩家{userId}不存在或不在游戏中");
            }
            return user;
        }

        //玩家上线事件
        public static async Task<User> UserOnLine(this UserComponent userComponent, long userId, long sessionActorId)
        {
            User user = userComponent.GetUser(userId);
            if (user != null)
            {
                user.ByCompelAccount();
                user.GetComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
                return user;//如果User本就在线 发送强制下线消息 就改变一下actorid 
            }
            user = await userComponent.Query(userId);
            if (user != null)
            {
                user.AddComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
                userComponent.mOnlineUserDic[user.UserId] = user;
                user.IsOnLine = true;
            }
            return user;
        }
        //玩家下线事件
        public static async void UserOffline(this UserComponent userComponent, long userId)
        {
            if (userComponent.mOnlineUserDic.ContainsKey(userId))
            {
                userComponent.mOnlineUserDic[userId].IsOnLine = false;
                userComponent.mOnlineUserDic[userId].RemoveComponent<UserGateActorIdComponent>();
                await userComponent.dbProxyComponent.Save(userComponent.mOnlineUserDic[userId]);
                userComponent.mOnlineUserDic.Remove(userId);
              
            }
        }
        //根据userId查询User
        public static async Task<User> Query(this UserComponent userComponent, long userId)
        {
            User userInId = userComponent.GetUser(userId);
            if (userInId != null)
            {
                return userInId;
            }
            List<User> userList = await userComponent.dbProxyComponent.Query<User>(user => user.UserId == userId);
            if (userList.Count > 0)
            {
                return userList[0];
            }
            return null;
        }


       
        //用户登陆
        public static async Task<AccountInfo> LoginOrRegister(this UserComponent userComponent, string dataStr, int loginType)
        {
            AccountInfo accountInfo=null;
            switch (loginType)
            {
                case LoginType.Editor:
                case LoginType.Tourist:
                    accountInfo=await userComponent.EditorLogin(dataStr);
                    break;
                case LoginType.WeChat:
                    accountInfo=await userComponent.WeChatLogin(dataStr);
                    break;
                case LoginType.Voucher://检验凭证
                    accountInfo=await userComponent.VoucherLogin(dataStr);
                    break;
                default:
                    Log.Error("不存在的登陆方式:" + loginType);
                    break;
            }
            //更新最后登录时间
            if (accountInfo!=null)
            {
                accountInfo.LastLoginTime = TimeTool.GetCurrenTimeStamp();
                await userComponent.dbProxyComponent.Save(accountInfo);
            }
            return accountInfo;
        }

        public static async Task<User> SaveUserDB(this UserComponent userComponent, User user)
        {
            await userComponent.dbProxyComponent.Save(user);
            return user;
        }
    }
}
