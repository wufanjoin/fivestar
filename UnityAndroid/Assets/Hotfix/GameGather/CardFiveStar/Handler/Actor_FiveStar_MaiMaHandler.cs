using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_MaiMaHandler : AMHandler<Actor_FiveStar_MaiMa>
    {
        public static bool _IsMaima = false;
        public static int _MaiMaScore = 0;
        protected override async void Run(ETModel.Session session, Actor_FiveStar_MaiMa message)
        {
            _IsMaima = true;
            _MaiMaScore = message.Score;
            await UIComponent.GetUiView<MaiMaPanelComponent>().ShowMaiMaCard(message.Card, message.Score);
            UIComponent.GetUiView<FiveStarSmallResultPanelComponent>().ShowSmallResult(Actor_FiveStar_SmallResultHandler.samllResultMessag);
            _IsMaima = false;


        }
    }
}
