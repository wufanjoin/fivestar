using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyAwakeSystem : AwakeSystem<GameLobby>
    {

        public override async void Awake(GameLobby self)
        {
            long nextModayGapStamp=TimeTool.GetNextModayGapTimeStamp();
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(TimeTool.TicksConvertMillisecond(nextModayGapStamp));
            Session friendsSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.FriendsCircle);//获取亲友圈 Session
            while (true)
            {
                self.WeekRefreshAction?.Invoke();//调用整点刷新
                friendsSession.Send(new L2S_WeekRefresh());//现在 每周刷新 只有亲友圈服务需要
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(TimeTool.TicksConvertMillisecond(TimeTool.WeekTime));
            }
        }
    }
    public static class GameLobbySystem
    {
       
    }
}
