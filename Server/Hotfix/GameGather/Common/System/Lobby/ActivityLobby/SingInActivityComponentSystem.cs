using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class SingInActivityComponentAwakeSystem : AwakeSystem<SingInActivityComponent>
    {

        public override  void Awake(SingInActivityComponent self)
        {
            //注册每周整点刷新事件
            GameLobby.Ins.WeekRefreshAction -= self.WeekRefreshSingState;
            GameLobby.Ins.WeekRefreshAction += self.WeekRefreshSingState;
        }
    }
    public static class SingInActivityComponentSystem
    {
        //每周刷新一次
        public static async void WeekRefreshSingState(this SingInActivityComponent singInActivityComponent)
        {
            List<UserSingInState> userSingInStates = await singInActivityComponent.dbProxyComponent.Query<UserSingInState>(state => true);
            for (int i = 0; i < userSingInStates.Count; i++)
            {
                userSingInStates[i].SingInDays = 0;//所有人签到的天 都被置为0
            }
            await singInActivityComponent.dbProxyComponent.Save(userSingInStates);
        }
        //获取玩家签到信息
        public static async Task<UserSingInState> GetUserSingInState(this SingInActivityComponent singInActivityComponent, long userId)
        {
            List<UserSingInState> userSingInStates = await singInActivityComponent.dbProxyComponent.Query<UserSingInState>(userSingInState => userSingInState.UserId == userId);
            if (userSingInStates.Count > 0)
            {
                return userSingInStates[0];
            }
            return null;
        }
        //进去签到
        public static async Task<bool> UserTodaySingIn(this SingInActivityComponent singInActivityComponent, long userId)
        {
            List<UserSingInState> userSingInStates = await singInActivityComponent.dbProxyComponent.Query<UserSingInState>(userSingInState => userSingInState.UserId == userId);
            if (userSingInStates.Count > 0)
            {
                if (userSingInStates[0].SingInTime != 0 && TimeTool.TimeStampIsToday(userSingInStates[0].SingInTime))
                {
                    return false;
                }
            }
            UserSingInState sueSingInState = ComponentFactory.CreateWithId<UserSingInState>(userId);
            sueSingInState.SingInTime = TimeTool.GetCurrenTimeStamp();
            sueSingInState.UserId = userId;
            sueSingInState.SingInDays = 1;
            if (userSingInStates.Count > 0)
            {
                sueSingInState.SingInDays = userSingInStates[0].SingInDays + 1;
            }

            await singInActivityComponent.SendUserGetGoods(userId, sueSingInState.SingInDays);
            await singInActivityComponent.dbProxyComponent.Save(sueSingInState);
            return true;
        }

        //发送玩家获得物品的信息
        public static async Task SendUserGetGoods(this SingInActivityComponent singInActivityComponent, long userId, int singInDays)
        {
            SignInAward signInAward = singInActivityComponent.mSignInAwardList[singInDays - 1];
            await UserHelp.GoodsChange(userId, signInAward.GoodsId, signInAward.Amount,GoodsChangeType.SignIn, true);
        }
        //获得签到奖励列表
        public static List<SignInAward> GetSignInAwardList(this SingInActivityComponent singInActivityComponent)
        {
            return singInActivityComponent.mSignInAwardList;
        }

    }
}
