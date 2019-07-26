using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_DealHandler : AMHandler<Actor_FiveStar_Deal>
    {
        protected override async void Run(ETModel.Session session, Actor_FiveStar_Deal message)
        {
            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().Hide();//隐藏打漂面板
            EventMsgMgr.SendEvent(CardFiveStarEventID.ZhiSeZi);//发起玩家置赛事件
            await UIComponent.GetUiView<SifterAnimPanelComponent>().RandomPlaySifterNum();//先掷筛子
            EventMsgMgr.SendEvent(CardFiveStarEventID.Deal, message.Cards); 
        }
    }
}
