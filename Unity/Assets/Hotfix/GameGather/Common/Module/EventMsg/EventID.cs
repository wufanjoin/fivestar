
namespace ETHotfix
{
    public static class CommEventID
    {
        private static int _incEventId = 0;

        static string GetUniqueString()
        {
            _incEventId++;
            return "UniqueEventID_" + _incEventId.ToString();
        }
        public static readonly string CallRequest = GetUniqueString(); //Call发送消息
        public static readonly string CallResponse = GetUniqueString(); //Call收到消息

        public static readonly string SelfUserInfoRefresh = GetUniqueString(); //自己的User信息刷新
        public static readonly string OnOtherNormalViewShow = GetUniqueString(); //其他Normal视图显示
        public static readonly string UserFinshSignIn = GetUniqueString(); //用户完成签到
        public static readonly string GetGodds = GetUniqueString(); //得到物品
        public static readonly string ReceiveChatInfo = GetUniqueString(); //收到聊天信息
        public static readonly string FrienCircleCutShowWanFa = GetUniqueString(); //亲友群切换显示玩法
        public static readonly string UserOffLine = GetUniqueString(); //房间内用户下线
        public static readonly string UserOnLine = GetUniqueString(); //房间内用户上线

    }
}
