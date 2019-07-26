using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_FiveStar_LiangDaoHandler : AMHandler<Actor_FiveStar_LiangDao>
    {
        protected override void Run(ETModel.Session session, Actor_FiveStar_LiangDao message)
        {
            CardFiveStarRoom.Ins.PlayerLiangDao(message.SeatIndex, message.Hnads);
        }
    }
}
