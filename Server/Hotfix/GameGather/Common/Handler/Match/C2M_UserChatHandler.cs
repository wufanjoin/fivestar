using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    public class C2M_UserChatHandler : AMRpcHandler<C2M_UserChat, M2C_UserChat>
    {
        protected override void Run(Session session, C2M_UserChat message, Action<M2C_UserChat> reply)
        {
            M2C_UserChat response = new M2C_UserChat();
            try
            {
                if (!Game.Scene.GetComponent<MatchRoomComponent>().UserChat(message.UserId, message.ChatInfo))
                {
                    response.Message = "广播聊天信息失败";
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}