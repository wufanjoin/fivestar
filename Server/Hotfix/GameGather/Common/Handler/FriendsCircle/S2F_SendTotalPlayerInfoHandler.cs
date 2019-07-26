using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 游戏打完一大局游戏 发送过来的战绩信息
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class S2F_SendTotalPlayerInfoHandler : AMHandler<S2F_SendTotalPlayerInfo>
    {
        protected override  async void Run(Session session, S2F_SendTotalPlayerInfo message)
        {
            try
            {
                FriendsCircle friendsCircle= await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    Log.Error("游戏发过得我战绩 没有对应的亲友圈接收:"+ message.FriendsCrircleId);
                    return;
                }
                friendsCircle.RankingGameReult(message.TotalPlayerInfos);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}
