using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    /// <summary>
    /// 玩家选择是否投票解散房间
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class Actor_VoteDissolveSelectHandler : AMHandler<Actor_VoteDissolveSelect>
    {
        protected override void Run(Session session, Actor_VoteDissolveSelect message)
        {

            try
            {
                MatchRoom matchRoom=MatchRoomComponent.Ins.GetRoomUserIdIn(message.UserId);
                if (matchRoom != null)
                {
                    matchRoom.PlayerVoteDissolveRoom(message.UserId, message.IsConsent);//玩家投票
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }


    }
}