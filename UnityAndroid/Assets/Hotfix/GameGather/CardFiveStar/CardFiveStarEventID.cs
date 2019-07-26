using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public static class CardFiveStarEventID
    {

        private static int _incEventId = 0;

        static string GetUniqueString()
        {
            _incEventId++;
            return "CardFiveStarEventID_" + _incEventId.ToString();
        }

        public static readonly string TestEvnet = GetUniqueString(); //测试事件
        public static readonly string PlayerSelectLiangCardState = GetUniqueString(); //玩家选择亮牌的状态
        public static readonly string Deal = GetUniqueString(); //发牌
        public static readonly string ZhiSeZi = GetUniqueString(); //开始掷塞子
        public static readonly string HideAllPlayer = GetUniqueString(); //隐藏所有玩家
        public static readonly string CutMatchIn = GetUniqueString(); //切换匹配中的状态
        public static readonly string CutReadyIn = GetUniqueString(); //切换准备中的状态
        public static readonly string CutGameIn = GetUniqueString(); //切换匹游戏中的状态
        public static readonly string CutBeginStartPrepare = GetUniqueString(); //切换准备匹配的状态
        public static readonly string CutRoomCardEnterReadyIn = GetUniqueString(); //房卡模式等人齐的状态
        public static readonly string PlayerLiangDao = GetUniqueString(); //玩家亮倒
        public static readonly string UserChuCard = GetUniqueString(); //用户出牌
        public static readonly string CanChuCardHaveHuHint = GetUniqueString(); //可以出牌的时候有胡牌提示
    }
}
