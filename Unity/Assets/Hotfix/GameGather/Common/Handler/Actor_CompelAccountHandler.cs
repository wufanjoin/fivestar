using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_CompelAccountHandler : AMHandler<Actor_CompelAccount>
    {
        protected override void Run(ETModel.Session session, Actor_CompelAccount message)
        {
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
            UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("您的账号在别处登陆",null, PopOptionType.Single);
        }
    }
}
