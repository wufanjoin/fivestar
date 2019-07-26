using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_TotalResultHandler : AMHandler<Actor_FiveStar_TotalResult>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_TotalResult message)
        {
            UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().ShowTotalResultInfo(message);
            UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().Hide();
            // CardFiveStarRoom.Ins.SmallStarGame(message);
        }
    }
}
