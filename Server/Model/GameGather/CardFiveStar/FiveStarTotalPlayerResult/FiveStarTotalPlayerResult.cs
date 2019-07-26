using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public partial class FiveStarTotalPlayerResult : Entity
    {
        public override void Dispose()
        {
            base.Dispose();
            SeatIndex = 0;
            HuPaiCount = 0;
            ZiMoCount = 0;
            FangChongCount = 0;
            GangPaiCount = 0;
            TotalSocre = 0;
        }
    }
}
