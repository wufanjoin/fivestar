using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
   public class FiveStarOverTime
    {
        public const int CanOperateOverTime = 10;//可操作超时时间
        public const int CanPlayCardOverTime = 12;//可出牌剩超时时间
        public const int CanDaPiaoOverTime = 8;//打漂时间
        public const int DissolveOverTime = 60;//投票超时时间
    }

    public class FiveStarOverTimeType
    {
        public const int OperateType = 1;
        public const int PlayCardType = 2;
        public const int DaPiaoType = 3;
    }
}
