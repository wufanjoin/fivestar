using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public class PlayCardType
    {
        /// <summary>不成立不能出牌</summary>
        public const int None = 0;
        /// <summary>单牌</summary>
        public const int DanZhang = 1;
        /// <summary>对子</summary>
        public const int DuiZi = 2;
        /// <summary>三张</summary>
        public const int SanZhang = 20;
        /// <summary>连对</summary>
        public const int LianDui = 3;
        /// <summary>顺子</summary>
        public const int ShunZi = 4;
        /// <summary>三带一</summary>
        public const int SanDaiYi = 5;
        /// <summary>三带二</summary>
        public const int SanDaiEr = 6;
        /// <summary>四带二</summary>
        public const int SiDaiEr = 7;
        /// <summary>四带二对</summary>
        public const int SiDaiErDui = 8;
        /// <summary>飞机不带</summary>
        public const int FeiJiBuDai = 21;
        /// <summary>飞机带单张</summary>
        public const int FeiJiDaiDanZhang = 9;
        /// <summary>飞机带对子</summary>
        public const int FeiJiDaiDuiZi = 10;
        /// <summary>炸弹</summary>
        public const int ZhaDan = 11;
        /// <summary>王炸</summary>
        public const int WangZha = 12;
    }
}
