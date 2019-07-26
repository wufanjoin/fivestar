using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_CollocationChangeHandler : AMHandler<Actor_FiveStar_CollocationChange>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_CollocationChange message)
        {
            if (message.IsCollocation)
            {
                UIComponent.GetUiView<CollocationPanelComponent>().Show();
            }
            else
            {
                UIComponent.GetUiView<CollocationPanelComponent>().Hide();
            }
        }
    }
}
