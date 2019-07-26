using System.Collections;
using System.Collections.Generic;

namespace ETHotfix
{
    public class FiveStarOperateType
    {
        public const int None = 0;//不能操作 或 放弃操作
        public const int Peng = 1;//碰 
        public const int MingGang = 2;//明杠   不用区分杠的都用这个
        public const int AnGang = 3;//暗杠
        public const int CaGang = 4;//擦杠
        public const int FangChongHu = 5;//放冲胡/胡  不用分自摸还是放冲都用这个
        public const int ZiMo = 6;//自摸
        public const int Liang = 7;//亮牌

        //----暂时只有播放音效的时候才用到---
        public const int MoCard = 10;//摸牌
        public const int ChuCard = 11;//出牌
        public const int ClickCard = 12;//点击牌 
        public const int GameStart = 13;//游戏开始
        public const int ZhiSeZi = 14;//置色子
        public const int ChuCardFall = 15;//出牌落地
    }

    public class FiveStarPlayerResultType
    {
        public const int Normal = 0;//正常 没胡也没放冲
        public const int HuFangChong = 1;//放冲胡
        public const int ZiMoHu = 2;//自摸胡牌
        public const int FangChong = 3;//放冲
    }
}