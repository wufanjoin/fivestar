using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarLeftPlayer : CardFiveStarPlayer
    {
        public override int HandType
        {
            get { return CardFiveStarCardType.Left_ZhiLi_BeiMian; }
        }
        public override int ChuCardType
        {
            get { return CardFiveStarCardType.Left_DaoDi_ZhengMain; }
        }
        public override int LaingCardType
        {
            get { return CardFiveStarCardType.Left_DaoDi_ZhengMain; }
        }
        public override int PengCardType
        {
            get { return CardFiveStarCardType.Left_DaoDi_ZhengMain; }
        }
        public override int AnGangCardType
        {
            get { return CardFiveStarCardType.Left_DaoDi_BeiMian; }
        }


    }
}
