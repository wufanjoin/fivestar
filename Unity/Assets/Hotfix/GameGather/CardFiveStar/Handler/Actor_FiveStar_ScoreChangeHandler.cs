using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_ScoreChangeHandler : AMHandler<Actor_FiveStar_ScoreChange>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_ScoreChange message)
        {
            CardFiveStarRoom.Ins.ScoreChang(message.GetScore, message.NowScore);
        }
    }
}
