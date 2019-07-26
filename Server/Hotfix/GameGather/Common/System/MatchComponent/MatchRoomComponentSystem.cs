using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem : AwakeSystem<MatchRoomComponent>
    {
       
        public override  void Awake(MatchRoomComponent self)
        {
            self.DetectionAiJoin();//检测队列 是否有人 若果有就加入AI 
            self.RoomVoteOvertime();//房间投票计时
        }
    }

    public static partial class MatchRoomComponentSystem
    {
        //已经进行过投票超时的房间
        public static  List<MatchRoom> VoteOverTimeFinshRooms=new List<MatchRoom>();
        //房间投票计时
        public static async void RoomVoteOvertime(this MatchRoomComponent self)
        {
            while (true)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
                VoteOverTimeFinshRooms.Clear();
                for (int i = 0; i < self.VoteInRooms.Count; i++)
                {
                    self.VoteInRooms[i].VoteTimeResidue--;
                    if (self.VoteInRooms[i].VoteTimeResidue <= 0)
                    {
                        self.VoteInRooms[i].RoomVoteOverTime();
                        VoteOverTimeFinshRooms.Add(self.VoteInRooms[i]);
                    }
                }
                for (int i = 0; i < VoteOverTimeFinshRooms.Count; i++)
                {
                    if (self.VoteInRooms.Contains(VoteOverTimeFinshRooms[i]))
                    {
                        self.VoteInRooms.Remove(VoteOverTimeFinshRooms[i]);
                    }
                }
            }
        }


        //检测队列里面 有没有人 如果有 人 就加入机器人在队列里面
        public static async void DetectionAiJoin(this MatchRoomComponent self)
        {
            while (true)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(3000);
                for (int i = 0; i < self.AllMatchRoomConfigs.Length; i++)
                {
                    if (self.mAllQueueDics[self.AllMatchRoomConfigs[i].MatchRoomId].Count > 0)
                    {
                        for (int j = self.mAllQueueDics[self.AllMatchRoomConfigs[i].MatchRoomId].Count; j < self.AllMatchRoomConfigs[i].GameNumber; j++)
                        {
                            //往这个匹配队列里面加入机器人
                            User aiUser = await AIUserFactory.Create(self.AllMatchRoomConfigs[i].BesansLowest);
                            self.mAllQueueDics[self.AllMatchRoomConfigs[i].MatchRoomId].Add(aiUser);
                        }
                        self.DetectionMatchCondition(self.AllMatchRoomConfigs[i].MatchRoomId);
                    }
                }
            }
        }
        public static int JoinMatchQueue(this MatchRoomComponent matchRoomComponent, long matchRoomId, User user, IResponse iResponse)
        {
            try
            {
                if (matchRoomComponent.mUserInQueue.ContainsKey(user.UserId))
                {
                    iResponse.Message = "用户已经在队列里面";
                    return 1;//用户已经在队列里面
                }
                else
                {
                    if (!matchRoomComponent.mAllQueueDics.ContainsKey(matchRoomId))
                    {
                        iResponse.Message = "要加入的匹配房间不存在";
                        return 2;//要加入的匹配房间不存在
                    }
                    MatchRoomConfig matchRoomConfig = Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetMatachRoomInfo(matchRoomId);
                    if (user.Beans<matchRoomConfig.BesansLowest)
                    {
                        iResponse.Message = "豆子不足";
                        return 2;//豆子数不足
                    }
                    matchRoomComponent.mUserInQueue.Add(user.UserId, matchRoomId);
                    matchRoomComponent.mAllQueueDics[matchRoomId].Add(user);
                    iResponse.Message = string.Empty;
                    return 0;//成功加入
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        private static long _TemporarayActorId = 0;
        private static bool _IsAI = false;
        //检测能不能匹配成功
        public static void DetectionMatchCondition(this MatchRoomComponent matchRoomComponent, long matchRoomId)
        {
            try
            {
                MatchRoomConfig matchRoomConfig = Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetMatachRoomInfo(matchRoomId);
                while (matchRoomComponent.mAllQueueDics[matchRoomId].Count >= matchRoomConfig.GameNumber)
                {
                    M2S_StartGame m2SStartGame = new M2S_StartGame();
                    m2SStartGame.RoomConfig = matchRoomConfig;
                    for (int i = 0; i < matchRoomConfig.GameNumber; i++)
                    {
                        _TemporarayActorId = 0;
                        _IsAI = false;
                        if (matchRoomComponent.mAllQueueDics[matchRoomId][i].GetComponent<UserGateActorIdComponent>()!=null)
                        {
                            _TemporarayActorId = matchRoomComponent.mAllQueueDics[matchRoomId][i].GetComponent<UserGateActorIdComponent>().ActorId;
                        }
                        else
                        {
                            _IsAI = true;
                        }
                        MatchPlayerInfo matchPlayer = MatchPlayerInfoFactory.Create(matchRoomComponent.mAllQueueDics[matchRoomId][i], _TemporarayActorId, i, _IsAI);
                        m2SStartGame.MatchPlayerInfos.Add(matchPlayer);
                    }
                    matchRoomComponent.PlayerStartGame(matchRoomId, matchRoomConfig.GameNumber);
                    //随机分配一个游戏服务器并告知他开始一局游戏
                    long toyGameId = Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetMatachRoomInfo(matchRoomId).ToyGameId;
                    matchRoomComponent.RoomStartGame(toyGameId, m2SStartGame);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }

        //一个用户退出匹配队列
        public static void RmoveQueueUser(this MatchRoomComponent matchRoomComponent, long userId)
        {
            try
            {
                if (matchRoomComponent.mUserInQueue.ContainsKey(userId))
                {
                    User userIdUser = null;
                    List<User> userLists = matchRoomComponent.mAllQueueDics[matchRoomComponent.mUserInQueue[userId]];
                    foreach (var user in userLists)
                    {
                        if (user.UserId == userId)
                        {
                            userIdUser = user;
                        }
                    }
                    if (userIdUser != null)
                    {
                        userLists.Remove(userIdUser);
                    }
                    matchRoomComponent.mUserInQueue.Remove(userId);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
        //移除对应的人都是从数组开头开始移除
        private static void PlayerStartGame(this MatchRoomComponent matchRoomComponent, long matchRoomId, int number)
        {
            try
            {
                for (int i = 0; i < number; i++)
                {
                    matchRoomComponent.mUserInQueue.Remove(matchRoomComponent.mAllQueueDics[matchRoomId][0].UserId);
                    matchRoomComponent.mAllQueueDics[matchRoomId].RemoveAt(0);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        //开始游戏 并创建房间记录 返回房间ID
        private static void RoomStartGame(this MatchRoomComponent matchRoomComponent, long toyGameId, M2S_StartGame message)
        {
            try
            {
                //记录玩家信息
                MatchRoom matchRoom = MatchRoomFactory.CreateRandomMatchRoom(toyGameId, matchRoomComponent.RandomRoomId(), message.MatchPlayerInfos, message.RoomConfig);
                matchRoomComponent.MatchRoomDic[matchRoom.RoomId] = matchRoom;
                for (int i = 0; i < message.MatchPlayerInfos.count; i++)
                {
                    if (message.MatchPlayerInfos[i].IsAI)
                    {
                        continue;//如果是AI不记录
                    }
                    matchRoomComponent.UserIdInRoomIdDic[message.MatchPlayerInfos[i].User.UserId] = matchRoom;
                }

                message.RoomType = RoomType.Match;
                Session toyGameSession = Game.Scene.GetComponent<NetInnerSessionComponent>().GetGameServerSession(toyGameId);
                message.RoomId = matchRoom.RoomId;
                toyGameSession.Send(message);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}