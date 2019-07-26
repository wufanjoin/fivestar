using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_VoteDissolveRoomResultHandler : AMHandler<Actor_VoteDissolveRoomResult>
    {
        protected override void Run(ETModel.Session session, Actor_VoteDissolveRoomResult message)
        {
            if (Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame != ToyGameId.CardFiveStar)
            {
                return;//当前是不在游戏就 不接收
            }
            UIComponent.GetUiView<VoteDissolvePanelComponent>().ShowVoteDissolve(message);
        }
    }
}
