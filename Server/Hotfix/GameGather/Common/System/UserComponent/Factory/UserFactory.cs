using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
 public  static class UserFactory
    {
        public static async Task<AccountInfo> EditRegisterCreatUser(string account)
        {
            User user = ComponentFactory.Create<User>();
            user.Icon = string.Empty;
            user.Name = "小明" + RandomTool.Random(0, 1000);
            user.Beans = UserConfigComponent.Ins.InitUserBeans.Value;
            user.Jewel = UserConfigComponent.Ins.InitUserJewel.Value;
            user.UserId = ++UserConfigComponent.Ins.MaximumUserId;
            user.Sex = SexType.WoMan;//游客或者编辑登陆 性别默认是女
            await Game.Scene.GetComponent<DBProxyComponent>().Save(user);

            AccountInfo accountInfo = ComponentFactory.Create<AccountInfo>();
            accountInfo.UserId = user.UserId;
            accountInfo.Account = account;
            accountInfo.Password = string.Empty;
            accountInfo.IsStopSeal = false;
            await Game.Scene.GetComponent<DBProxyComponent>().Save(accountInfo);
            return accountInfo;
        }

        public static async Task<AccountInfo> WeChatRegisterCreatUser(WeChatJsonData weChatJsonData)
        {
            User user = ComponentFactory.Create<User>();
            user.Icon = weChatJsonData.headimgurl;
            user.Name = weChatJsonData.nickname;
            user.Beans = UserConfigComponent.Ins.InitUserBeans.Value;
            user.Jewel = UserConfigComponent.Ins.InitUserJewel.Value;
            user.UserId = ++UserConfigComponent.Ins.MaximumUserId;
            user.Sex = weChatJsonData.sex;
            await Game.Scene.GetComponent<DBProxyComponent>().Save(user);

            AccountInfo accountInfo = ComponentFactory.Create<AccountInfo>();
            accountInfo.UserId = user.UserId;
            accountInfo.Account = weChatJsonData.unionid;
            accountInfo.IsStopSeal = false;
            await Game.Scene.GetComponent<DBProxyComponent>().Save(accountInfo);
            return accountInfo;
        }

        //匹配AI创建User
        public static async Task<User> AICreatUser(long userId)
        {
            User user = ComponentFactory.Create<User>();
            user.UserId = userId;
            user.Beans = 0;
            if (user.UserId % 2 == 0)
            {
                user.Sex = SexType.Man;
            }
            else
            {
                user.Sex = SexType.WoMan;
            }
            await Game.Scene.GetComponent<DBProxyComponent>().Save(user);
            return user;
        }
        //AIUser复制一个user对象
        public static  User AIUserCopy(User user)
        {
            User newUser = ComponentFactory.Create<User>();
            newUser.UserId = user.UserId;
            newUser.Name = user.Name;
            newUser.Icon = user.Icon;
            newUser.Beans = user.Beans;
            newUser.Sex = user.Sex;
            newUser.IsOnLine = true;
            return newUser;
        }
    }
}
