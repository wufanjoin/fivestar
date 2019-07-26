using ETModel;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class CardFiveStarRoomAwakeSystem : AwakeSystem<CardFiveStarRoom>
    {
        public override void Awake(CardFiveStarRoom self)
        {
            self.Awake();
        }
    }
    public partial class CardFiveStarRoom : Component
    {
        public static CardFiveStarRoom Ins;
        // public long _RoomMasterUserId=0;//房主的userId 房主就是服务器索引为0的玩
        public int _RoomId = 0;//房间ID

        public int _FriendsCircleId = 0;//对应的亲友圈Id 0就是普通房卡房间
        public int _CuurRoomOffice = 0;//当前房间的局数
        public int _RoomType = RoomType.Match;//房间类型默认是匹配
        public int _RoomState = RoomStateType.None;//房间现在的状态
        public int iEndChuCardSize = 0;//最后出的是哪一张牌
        public int _EndChuCardSeatIndex = 0;//最后出牌玩家的客户端索引
        public CardFiveStarRoomPanelComponent _roomPanel;//房间UI面板
        public FiveStarRoomConfig _config;//房间配置信息
        private int _userServereatIndex = -1;//用户所在牌局的服务器索引
        public Dictionary<int, CardFiveStarPlayer> _ServerSeatIndexInPlayer = new Dictionary<int, CardFiveStarPlayer>();//存储游戏玩家对象 用服务器索引对应
        public List<int> _LiangDaoCanHuCards=new List<int>();//亮倒玩家所有可以胡的牌 自己可胡的牌 不记录
        public Dictionary<int,int> _AllCardResidueNum=new Dictionary<int, int>();//所有手牌剩余数量
        public int _ResideNum = 0;//剩余牌数量

        public  void Awake()
        {
            Ins = this;
            _roomPanel = UIComponent.GetUiView<CardFiveStarRoomPanelComponent>();
            InitCardResidueNum();//初始化剩余牌数
        }

        public override void Dispose()
        {
            Ins = null;
            LittleRoundClearData(); //每小局要清空的数据
            _FriendsCircleId = 0;//对应亲友圈id
            _CuurRoomOffice = 0;//当前房间的局数
            _RoomType = RoomType.Match;//房间类型默认是匹配
            _RoomState = RoomStateType.None;//房间现在的状态
            _config = null;//房间配置信息
            _userServereatIndex = -1;//用户所在牌局的服务器索引
            _ServerSeatIndexInPlayer.Clear();//清空玩家对象
            base.Dispose();
        }
        //每小局要清空的数据
        public void LittleRoundClearData()
        {
            iEndChuCardSize = 0;//最后出的是哪一张牌
            _EndChuCardSeatIndex = 0;//最后出牌玩家的客户端索引
            _LiangDaoCanHuCards.Clear();//亮倒玩家所有可以胡的牌 自己可胡的牌 不记录
            InitCardResidueNum();//初始化剩余牌数
            foreach (var player in _ServerSeatIndexInPlayer)
            {
                player.Value.LittleRoundClearData();
            }
         }
        //初始化牌剩余数量
        public void InitCardResidueNum()
        {
            for (int i = 0; i <= 21; i++)
            {
                _AllCardResidueNum[i] = 4;
            }
            _ResideNum = 84;
        }
        //减少牌的总数量
        public void ReduceCardTotalNum(int cout)
        {
            _ResideNum -= cout;
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().SetResidueNum(_ResideNum);
        }
        //删除一个牌的剩余数量
        public void ReduceCardInNum(int card)
        {
            _AllCardResidueNum[card]--;
        }
        public void ReduceCardInNum(IList<int> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                ReduceCardInNum(cards[i]);
            }
        }

        //添加亮倒玩家可以胡的牌
        public void AddLiangDaoCanHuCards(int clientSeatIndex,List<int> cards)
        {
            if (clientSeatIndex == 0)
            {
                return;
            }
            for (int i = 0; i < cards.Count; i++)
            {
                _LiangDaoCanHuCards.Add(cards[i]);
            }
           
        }

        //获取用户自己的玩家信息
        public CardFiveStarPlayer GetUserPlayerInfo()
        {
           return GetPlayerInfoUserIdIn(UserComponent.Ins.pUserId);
        }
        //清空玩家信息
        public void ClearPlayer()
        {
            _ServerSeatIndexInPlayer.Clear();
        }

        //设置房间配置信息
        public void SetConfigInfo(RepeatedField<int> configs)
        {
            _config = FiveStarRoomConfigFactory.Create(configs);
        }
        
        //显示玩家信息
        public void ShowPlayerInfo(RepeatedField<MatchPlayerInfo> fiveStarPlayers)
        {
            foreach (var player in fiveStarPlayers)
            {
                if (player.User.UserId == Game.Scene.GetComponent<UserComponent>().pUserId)
                {
                    _userServereatIndex = player.SeatIndex;
                }
            }
            foreach (var player in fiveStarPlayers)
            {
                ShowPlayerInfo(player.User, player.SeatIndex);
            }
        }
        //显示玩家信息
        public void ShowPlayerInfo(RepeatedField<FiveStarPlayer> fiveStarPlayers)
        {
            foreach (var player in fiveStarPlayers)
            {
                if (player.User.UserId == Game.Scene.GetComponent<UserComponent>().pUserId)
                {
                    _userServereatIndex = player.SeatIndex;
                }
            }
            foreach (var player in fiveStarPlayers)
            {
                ShowPlayerInfo(player.User,player.SeatIndex, player.NowScore);
            }
        }

        //显示玩家信息
        public void ShowPlayerInfo(User user,int seatIndex,int nowScore=0)
        {
            if (user.UserId == Game.Scene.GetComponent<UserComponent>().pUserId)
            {
                _userServereatIndex = seatIndex;
            }
            CardFiveStarPlayer cardFiveStarPlayer = CardFiveStarPlayerFactory.Creator(user, seatIndex, _userServereatIndex, _config.RoomNumber, _roomPanel.mPlayerUIsGo.transform, nowScore);//创建用户
            _ServerSeatIndexInPlayer[seatIndex] = cardFiveStarPlayer;
        }
        //玩家退出房间
        public void PlayerOutRoom(long userId)
        {
            int outSeatIndex = -1;
            foreach (var player in _ServerSeatIndexInPlayer)
            {
                if (player.Value._user.UserId == userId)
                {
                    outSeatIndex = player.Key;
                }
            }
            if (outSeatIndex < 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("退出的玩家不在房间中");
            }
            else if (outSeatIndex == 0)
            {
                Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Lobby);
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("房主解散了房间");
            }
            else
            {
                _ServerSeatIndexInPlayer[outSeatIndex].Hide();
            }
        }
        //服务器索引 转换客户端索引 
        public int GetClientSeatIndex(int serverSeatIndex)
        {
            return _ServerSeatIndexInPlayer[serverSeatIndex].ClientSeatIndex;
        }
        //传一个UserId 获得玩家信息
        public CardFiveStarPlayer GetPlayerInfoUserIdIn(long userId)
        {
            foreach (var player in _ServerSeatIndexInPlayer)
            {
                if (player.Value._user.UserId == userId)
                {
                    return player.Value;
                }
            }
            Log.Error("房间内没有这个用户 UserID:" + userId);
            return null;
        }
        //获取玩家信息
        public CardFiveStarPlayer GetFiveStarPlayer(int serverSeatIndex)
        {
            if (_ServerSeatIndexInPlayer.ContainsKey(serverSeatIndex))
            {
                return _ServerSeatIndexInPlayer[serverSeatIndex];
            }
            Log.Error("无法获取玩家信息 索引超出范围:"+ serverSeatIndex);
            return null;
        }

    }
}
