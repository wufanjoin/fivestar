using ETModel;

namespace ETHotfix
{
    [ToyGame(ToyGameId.Login)]
    public class LoginAisle : ToyGameAisleBase
    {
        public override void Awake(long gameType)
        {
            base.Awake(gameType);

        }

        public override void StartGame(params object[] objs)
        {
            base.StartGame();
            Log.Debug("进入登陆界面");
            //EventMsgMgr.AllRemoveEvent();
            Game.Scene.GetComponent<KCPUseManage>().InitiativeDisconnect();
          //  Game.Scene.GetComponent<UIComponent>().RemoveAll();
            Game.Scene.GetComponent<UIComponent>().Show(UIType.LoginPanel);
        }

        public override void EndGame()
        {
            //base.EndGame();
           // Log.Error("处于登陆界面无法退出");
            //Game.Scene.GetComponent<UIComponent>().Show(UIType.LobbyPanel);
        }
    }
}
