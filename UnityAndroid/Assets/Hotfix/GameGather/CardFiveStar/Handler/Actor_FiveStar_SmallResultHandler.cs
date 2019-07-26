using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_SmallResultHandler : AMHandler<Actor_FiveStar_SmallResult>
    {
        public static Actor_FiveStar_SmallResult samllResultMessag;
        protected override void Run(ETModel.Session session, Actor_FiveStar_SmallResult message)
        {
            //赢家的手牌中 去掉赢的牌
            AwayWinnerWinCard(message);
            if (Actor_FiveStar_MaiMaHandler._IsMaima)
            {
                UIComponent.GetUiView<FiveStarSmallResultPanelComponent>().Hide();
                samllResultMessag = message;
            }
            else
            {
                UIComponent.GetUiView<FiveStarSmallResultPanelComponent>().ShowSmallResult(message);
            }
            CardFiveStarRoom.Ins.SmllResult(message);
            UIComponent.GetUiView<CollocationPanelComponent>().Hide();//小结算托管肯定结束了 隐藏一下
        }

        public static void AwayWinnerWinCard(Actor_FiveStar_SmallResult message)
        {
            //赢家的手牌中 去掉赢的牌
            for (int i = 0; i < message.SmallPlayerResults.Count; i++)
            {
                if (message.SmallPlayerResults[i].PlayerResultType == FiveStarPlayerResultType.HuFangChong || message.SmallPlayerResults[i].PlayerResultType == FiveStarPlayerResultType.ZiMoHu)
                {
                    message.SmallPlayerResults[i].Hands.Remove(message.SmallPlayerResults[i].WinCard);
                }
            }

        }
    }
 
}
