
namespace ETHotfix
{
    public static class JoyLandlordsEventID
    {
        private static int _incEventId = 0;

        static string GetUniqueString()
        {
            _incEventId++;
            return "JoyLandlordsEventID_" + _incEventId.ToString();
        }

        public static readonly string PlayCardHint = GetUniqueString(); //玩家点击出牌提示
        public static readonly string RequestPlayCard = GetUniqueString(); //玩家请求出牌
        public static readonly string StarMathch = GetUniqueString(); //开始匹配
        public static readonly string CanAddTwice = GetUniqueString(); //玩家可以加倍
        public static readonly string AddTwice = GetUniqueString(); //玩家加倍
        public static readonly string AnewDeal = GetUniqueString(); //重新发牌
        public static readonly string CanCallLanlord = GetUniqueString(); //可以叫地主
        public static readonly string CallLanlord = GetUniqueString(); //叫地主
        public static readonly string CanPlayCard = GetUniqueString(); //可以出牌
        public static readonly string PlayCard = GetUniqueString(); //玩家出牌
        public static readonly string CanRobLanlord = GetUniqueString(); //可以抢地主
        public static readonly string RobLanlord = GetUniqueString(); //抢地主
        public static readonly string Deal = GetUniqueString(); //发牌
        public static readonly string DissolveRoom = GetUniqueString(); //房间解散
        public static readonly string DontPlay = GetUniqueString(); //不出
        public static readonly string GameResult = GetUniqueString(); //游戏结算
        public static readonly string EnrerRoom = GetUniqueString(); //进入房间
        public static readonly string OutRoom = GetUniqueString(); //退出房间
        public static readonly string Prepare = GetUniqueString(); //玩家准备
        public static readonly string StartGame = GetUniqueString(); //开始游戏
        public static readonly string ConfirmCamp = GetUniqueString(); //确定阵营
        public static readonly string RequestOutRoom = GetUniqueString(); //玩家点击退出房间按钮

    }
}
