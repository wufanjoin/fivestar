using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_JoyLds_StartGameHandler : AMHandler<Actor_JoyLds_StartGame>
    {
        protected override void Run(ETModel.Session session, Actor_JoyLds_StartGame message)
        {
            Game.Scene.GetComponent<JoyLdsGameRoom>()?.StartGame(message);
        }
    }
}
