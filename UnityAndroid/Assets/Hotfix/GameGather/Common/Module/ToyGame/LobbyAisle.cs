using ETModel;

namespace ETHotfix
{
    [ToyGame(ToyGameId.Lobby)]
    public class LobbyAisle : ToyGameAisleBase
    {
        public override void Awake(long gameType)
        {
            base.Awake(gameType);

        }

        public override void StartGame(params object[] objs)
        {
            base.StartGame();
            Log.Debug("进入大厅");
            UIComponent.GetUiView<FiveStarLobbyPanelComponent>().Show();
        }

        public override void EndGame()
        {
            //base.EndGame();
            pToyGameComponent.StartGame(ToyGameId.Login);
        }
    }
}
