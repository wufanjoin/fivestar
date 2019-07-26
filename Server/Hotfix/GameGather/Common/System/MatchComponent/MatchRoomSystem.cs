using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static partial class MatchRoomSystem
    {
        //玩家加入房间
        public static bool UserJoinRoom(this MatchRoom matchRoom, User user, long sessionActorId)
        {
            for (int i = 0; i < matchRoom.RoomConfig.GameNumber; i++)
            {
                if (!matchRoom.PlayerInfoDic.ContainsKey(i))
                {
                    MatchPlayerInfo matchPlayerInfo = MatchPlayerInfoFactory.Create(user, sessionActorId, i);
                    matchRoom.BroadcastActorMessage(new Actor_OtherJoinRoom() { PlayerInfo = matchPlayerInfo });//广播其他玩家加入房间信息
                    matchRoom.PlayerInfoDic[i] = matchPlayerInfo;
                    return true;
                }
            }
            return false;
        }
        //玩家退出房间
        public static bool UserOutRoom(this MatchRoom matchRoom, long userId)
        {
            if (matchRoom.RoomType == RoomType.Match)
            {
                return true;//随机匹配房间无法退出 只能等待游戏结束 回复成功让玩家回到大厅
            }
            //如果正在游戏接发起投票解散房间
            if (matchRoom.IsGameBeing)
            {
                matchRoom.RommEnterVoteDissolve(userId);//开始投票
                return false;
            }
            matchRoom.intData = -1;
            //如果是房卡 并且还没开始 就退出房间
            foreach (var player in matchRoom.PlayerInfoDic)
            {
                if (player.Value.User.UserId == userId)
                {
                    matchRoom.intData = player.Key;
                    matchRoom.PlayerInfoDic.Remove(player.Key);
                    matchRoom.BroadcastActorMessage(new Actor_OtherOutRoom() { UserId = userId });
                    break;
                }
            }
            //如果是房主退出房间 直接移除房间
            if (matchRoom.intData == 0)
            {
                Game.Scene.GetComponent<MatchRoomComponent>().RemoveRoom(matchRoom.RoomId);
            }
            else if (matchRoom.intData == -1)
            {
                Log.Error("退出房间玩家不在房间中");
            }
            return true;
        }
        //进入投票状态
        public static void RommEnterVoteDissolve(this MatchRoom matchRoom, long userId)
        {
            if (matchRoom.IsVoteDissolveIn)
            {
                return;//已经在投票状态 无法再次投票
            }
            matchRoom.IsVoteDissolveIn = true;//状态改为投票中
            matchRoom.VoteTimeResidue = FiveStarOverTime.DissolveOverTime;//更改投票 超时剩余时间
            MatchRoomComponent.Ins.VoteInRooms.Add(matchRoom);//把自己房间 加入到 投票超时房间里面
            //让游戏服的房间暂停游戏
            ActorHelp.SendeActor(matchRoom.GameServeRoomActorId, new Actor_PauseRoomGame() { IsPause = true });
            matchRoom.VoteDissolveResult = new Actor_VoteDissolveRoomResult();//new 出发送个客户端的投票结果
            matchRoom.VoteDissolveResult.SponsorUserId = userId;//记录发起人的UserId
            matchRoom.PlayerVoteDissolveRoom(userId, true);//发起投票解散房间
        }
        //投票超时处理
        public static void RoomVoteOverTime(this MatchRoom matchRoom)
        {
            //超时默认结果为同意
            matchRoom.VoteDissolveResult.Result = VoteResultType.Consent;
            //广播投票的消息
            matchRoom.BroadcastActorMessage(matchRoom.VoteDissolveResult);
            //通知游戏服 房间解散 销毁游戏服的房间 游戏服会通知匹配服的
            ActorHelp.SendeActor(matchRoom.GameServeRoomActorId, new Actor_RoomDissolve());
        }
        //玩家投票解散房间
        public static void PlayerVoteDissolveRoom(this MatchRoom matchRoom, long userId, bool isConsent)
        {
            if (!matchRoom.IsVoteDissolveIn)//不是投票中收到投票消息 不予理会
            {
                return;
            }
            VoteInfo voteInfo = new VoteInfo();
            voteInfo.UserId = userId;
            voteInfo.IsConsent = isConsent;
            matchRoom.VoteDissolveResult.VoteInfos.Add(voteInfo);
            matchRoom.VoteDissolveResult.Result = VoteResultType.BeingVote;
            if (!isConsent)
            {
                //一个人不同意 直接不解散
                matchRoom.VoteDissolveResult.Result = VoteResultType.NoConsent;
            }
            else
            {
                //检测是否所有人都同意
                if (matchRoom.VoteDissolveResult.VoteInfos.Count == matchRoom.PlayerInfoDic.Count)
                {
                    matchRoom.VoteDissolveResult.Result = VoteResultType.Consent;
                }
            }
            //广播投票的消息
            matchRoom.BroadcastActorMessage(matchRoom.VoteDissolveResult);
            if (matchRoom.VoteDissolveResult.Result != VoteResultType.BeingVote)//如果结论不是投票中 就表示已经投票结束
            {
                if (MatchRoomComponent.Ins.VoteInRooms.Contains(matchRoom))
                {
                    MatchRoomComponent.Ins.VoteInRooms.Remove(matchRoom);//把自己房间 从投票超时房间里面移除
                }
                if (matchRoom.VoteDissolveResult.Result == VoteResultType.NoConsent)
                {
                    //让游戏服的房间开始游戏
                    ActorHelp.SendeActor(matchRoom.GameServeRoomActorId, new Actor_PauseRoomGame() { IsPause = false });
                }
                else if (matchRoom.VoteDissolveResult.Result == VoteResultType.Consent)
                {
                    //通知游戏服 房间解散 销毁游戏服的房间 游戏服会通知匹配服的
                    ActorHelp.SendeActor(matchRoom.GameServeRoomActorId, new Actor_RoomDissolve());
                }
                matchRoom.IsVoteDissolveIn = false;//改变投票状态
                matchRoom.VoteDissolveResult = null;//清空投票信息
               
            }
        }

        //玩家下线
        public static void PlayerOffline(this MatchRoom matchRoom, long userId)
        {
            for (int i = 0; i < matchRoom.PlayerInfoDic.Count; i++)
            {
                if (matchRoom.PlayerInfoDic[i].User.UserId == userId)
                {
                    matchRoom.PlayerInfoDic[i].User.IsOnLine = false;
                    matchRoom.PlayerInfoDic[i].SessionActorId = 0;
                    matchRoom.PlayerInfoDic[i].User.GetComponent<UserGateActorIdComponent>().ActorId = 0;
                    break;
                }
            }
            matchRoom.BroadcastActorMessage(new Actor_UserOffline() { UserId = userId });
        }
        //获取房间内玩家信息
        public static MatchPlayerInfo GetPlayerInfo(this MatchRoom matchRoom, long userId)
        {
            foreach (var player in matchRoom.PlayerInfoDic)
            {
                if (player.Value.User.UserId == userId)
                {
                    return player.Value;
                }
            }
            return null;
        }

        //玩家上线
        public static void PlayerOnLine(this MatchRoom matchRoom, long userId, long sessionActorId)
        {
            MatchPlayerInfo playerInfo = matchRoom.GetPlayerInfo(userId);
            if (playerInfo == null)
            {
                Log.Error(matchRoom.RoomId + "房间没有玩家" + userId);
            }
            playerInfo.SessionActorId = sessionActorId;
            playerInfo.User.GetComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
            playerInfo.User.IsOnLine = true;
            matchRoom.BroadcastActorMessage(new Actor_UserOnLine() { UserId = userId });

        }

        //检测可不可以开始游戏
        public static bool DetetionMayStartGame(this MatchRoom matchRoom)
        {
            return matchRoom.PlayerInfoDic.Count == matchRoom.RoomConfig.GameNumber;
        }
        //开始游戏
        public static void StartGame(this MatchRoom matchRoom)
        {
            M2S_StartGame m2SStartGame = new M2S_StartGame();
            m2SStartGame.RoomId = matchRoom.RoomId;
            m2SStartGame.RoomType = RoomType.RoomCard;
            m2SStartGame.MatchPlayerInfos.AddRange(matchRoom.PlayerInfoDic.Values);
            m2SStartGame.RoomConfig = matchRoom.RoomConfig;
            m2SStartGame.NeedJeweNumCount = matchRoom.NeedJeweNumCount;
            m2SStartGame.FriendsCircleId = matchRoom.FriendsCircleId;
            Session toyGameSession = Game.Scene.GetComponent<NetInnerSessionComponent>().GetGameServerSession(matchRoom.RoomConfig.ToyGameId);
            toyGameSession.Send(m2SStartGame);
        }
        //游戏结束扣除玩家钻石
        public static async Task DeductJewel(this MatchRoom matchRoom)
        {
            if (matchRoom.RoomType != RoomType.RoomCard)
            {
                return;//不是房卡 不扣除钻石
            }
            //亲友圈房间 亲友圈主人扣除钻石
            if (matchRoom.FriendsCreateUserId > 0)
            {
               await UserHelp.GoodsChange(matchRoom.FriendsCreateUserId, GoodsId.Jewel, matchRoom.NeedJeweNumCount*-1,
                    GoodsChangeType.RoomCard, false);
            }
            else if(matchRoom.IsAADeductJewel)
            {
                //如果是AA所有人都有扣除钻石
                foreach (var player in matchRoom.PlayerInfoDic)
                {
                   await player.Value.User.GoodsChange(GoodsId.Jewel, matchRoom.NeedJeweNumCount* - 1,
                        GoodsChangeType.RoomCard, false);
                }
            }
            else
            {
                //如果不是亲友圈 也不是AA就房主 索引0扣除钻石
                if (matchRoom.PlayerInfoDic.ContainsKey(0))
                {
                   await  matchRoom.PlayerInfoDic[0].User.GoodsChange( GoodsId.Jewel, matchRoom.NeedJeweNumCount* - 1,
                        GoodsChangeType.RoomCard, false);
                }
                
            }
          
        }
        //游戏结束扣除玩家豆子
        public static async Task DeductBeans(this MatchRoom matchRoom)
        {
            if (matchRoom.RoomType== RoomType.Match)
            {
                foreach (var player in matchRoom.PlayerInfoDic)
                {
                    if (player.Value.IsAI)//如果是AI是不会扣除豆子的
                    {
                        continue;
                    }
                    await player.Value.User.GoodsChange(GoodsId.Besans, matchRoom.RoomConfig.CostConsume*-1,
                        GoodsChangeType.None, false);
                }
            }
        }

        //游戏通知匹配服开始游戏了
            public static void GameServeStartGame(this MatchRoom matchRoom, long serveRoomActorId)
        {
            matchRoom.IsGameBeing = true;//状态改为在游戏中
            matchRoom.GameServeRoomActorId = serveRoomActorId;
        }
        //广播消息
        public static void BroadcastActorMessage(this MatchRoom matchRoom, IActorMessage iActorMessage)
        {
            foreach (var player in matchRoom.PlayerInfoDic)
            {
                player.Value.User.SendeSessionClientActor(iActorMessage);
            }
        }
    }

}