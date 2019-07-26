using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_UserChatInfoHandler : AMHandler<Actor_UserChatInfo>
    {
        protected override void Run(ETModel.Session session, Actor_UserChatInfo message)
        {
            ChatMgr.Ins.ReceiveChatInfo(message.UserId,message.ChatInfo);
        }
    }
}
