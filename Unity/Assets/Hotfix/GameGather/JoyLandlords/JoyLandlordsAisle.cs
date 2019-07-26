using ETModel;

namespace ETHotfix
{
    [ToyGame(ToyGameId.JoyLandlords)]   
    public class JoyLandlordsAisle : ToyGameAisleBase
    {
        public override void Awake(long gameType)
        {
            base.Awake(gameType);

        }

        public override void StartGame(params object[] objs)
        {
            base.StartGame();
            Log.Debug("进入游戏欢乐斗地主");
            UIComponent.GetUiView<BaseHallPanelComponent>().ShowChangeBaseHallUI(ToyGameId.JoyLandlords);
        }

        public override async void EndGame()
        {
            base.EndGame();//默认就是进入大厅界面
            
        }

        public override void EndAndStartOtherGame()
        {
            base.EndAndStartOtherGame();
            if (Game.Scene.GetComponent<JoyLdsGameRoom>() != null)
            {
                Game.Scene.RemoveComponent<JoyLdsGameRoom>();
            }
        }
    }
}
