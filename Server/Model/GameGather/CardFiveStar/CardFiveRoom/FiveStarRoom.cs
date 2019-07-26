using System;
using System.Collections.Generic;
using System.Text;
using System.Text;
using ETHotfix;
using Google.Protobuf.Collections;

namespace ETModel
{

    [ObjectSystem]
    public class FiveStarRoomAwakeSystem : AwakeSystem<FiveStarRoom>
    {
        public override void Awake(FiveStarRoom self)
        {
            self.Awake();
        }
    }
    public class FiveStarRoom : Entity
    {
        public int MathRoomId = 0;//匹配房间ID 只有是匹配房间才有
        public int RoomId;//房间ID
        public int FriendsCircleId = 0;//对应的亲友圈id 0就是不在亲友圈中
        public int RoomNumber = 0;//房间人数 四人房的话 
        public int NeedJeweNumCount = 0;//需要的钻石数量
        public int RoomType = 0;//房间类型 匹配还是房卡
        public int CurrRoomStateType = RoomStateType.None;//当前房间游戏状态
        public bool IsPause = false;//房间是否暂停中
        public int GangHuNum = 0;//杠之后 等于2 每次出牌减1 如果 胡的时候这个0值大于 就说明是杠胡

        public bool IsGangShangCard//是否 是杠牌之后出的第一张牌
        {
            get { return GangHuNum > 0; }
        }
        public const int FaangChongHuMinHuCardMultiple = 2;//最小胡牌倍数

        public FiveStarRoomConfig RoomConfig;//房间配置信息
        public Dictionary<int, FiveStarPlayer> FiveStarPlayerDic = new Dictionary<int, FiveStarPlayer>();//玩家索引对应的玩家实体

        public int CurrRestSeatIndex = -1;// 当前休息玩家索引 必须为负数 因为2.3人局流局赔付 有误
        public int NextRestSeatIndex = 0;// 下个休息玩家索引

        public RepeatedField<int> ResidueCards;//剩余牌数组 摸的牌从这里面取
        public readonly RepeatedField<int> CanOperatePlayerIndex = new RepeatedField<int>();//当前可以行进操作的玩家索引列表
        public readonly RepeatedField<Actor_FiveStar_OperateResult> BeforeOperateResults = new RepeatedField<Actor_FiveStar_OperateResult>();//多人可以操作 操作记录



        public RepeatedField<FiveStar_SmallPlayerResult> SmallPlayerResults;//小局结算信息
        public ParticularMiltaryRecordDataInfo CurrParticularMiltaryRecordData;//当前小局录像信息
        public readonly List<int> HuPaiPlayerSeatIndexs = new List<int>();//所有胡牌玩家索引 一炮多响时可能有多个
        public readonly RepeatedField<int> LiangDaoCanHuCards = new RepeatedField<int>();//亮倒所有 玩家可以胡的牌
        public readonly RepeatedField<ParticularMiltary> ParticularMiltarys = new RepeatedField<ParticularMiltary>();//小局记录信息
        public int CurrChuPaiIndex = -1;//当前出牌玩家的索引
        public int CurrChuPaiCard = -1;//当前出的哪张牌
        public int FirstMoCardSeatIndex = 0;//最先摸摸牌玩家索引 
        public bool IsDaPiaoBeing = false;//是否在打漂中
        public int EntiretyOprationAleadyNum = 0;//需要整体操作已经操作的玩家数量
        public int MaiMaCard = -1;//买马买的哪一张牌
        public bool IsHaveAI = false;//房间内是否有AI

        public int StartVideoDataId = 0;//起始录像ID
        public int LastMoPaiSeatIndex = -1;//最后一个摸牌玩家的索引
        public int CanPlayCardPlayerIndex = 0;//可以出牌的玩家索引
        public int CurrOfficNum = 0;//当前局数
        public int QiOperateNextStep = 0;//放弃操作的下个步骤
        public IActorMessage EndCanOperateAndCanChuMessage;//最后一条可操作消息 可以碰杠胡 或者出牌
        private long _overTime = 0;//超时时间

        public long OverTime
        {
            get { return _overTime; }
            set
            {
                if (CurrRoomStateType == RoomStateType.GameIn)
                {
                    _overTime = value;
                }
                else
                {
                    _overTime = 0;
                }
            }
        }//超时时间 0就是没超时事件
        public bool boolData = false;
        public int intData;
        public int intData2;
        public void Awake()
        {
            CurrParticularMiltaryRecordData = ComponentFactory.Create<ParticularMiltaryRecordDataInfo>();
        }

        //初始化小结算信息
        public void InitSamllResultInfo()
        {
            if (SmallPlayerResults == null)
            {
                SmallPlayerResults = new RepeatedField<FiveStar_SmallPlayerResult>();
                for (int i = 0; i < RoomNumber; i++)
                {
                    SmallPlayerResults.Add(new FiveStar_SmallPlayerResult());
                }
            }
            for (int i = 0; i < SmallPlayerResults.Count; i++)
            {
                SmallPlayerResults[i].SeatIndex = 0;
                SmallPlayerResults[i].PlayerResultType = 0;
                SmallPlayerResults[i].GetScore = 0;
                SmallPlayerResults[i].Hands = null;
                SmallPlayerResults[i].WinCard = 0;
                SmallPlayerResults[i].HuPaiTypes.Clear();
            }

        }
        public override void Dispose()
        {
            base.Dispose();//组件会在Dispose被销毁
            LittleRoundClearData();
            foreach (var player in FiveStarPlayerDic)
            {
                player.Value.Dispose();
            }
            FiveStarPlayerDic.Clear();
            RoomConfig.Dispose();
            RoomId = 0;
            FriendsCircleId = 0;
            RoomNumber = 0;
            NeedJeweNumCount = 0;
            FriendsCircleId = 0;
            SmallPlayerResults = null;
            IsHaveAI = false;
            IsPause = false;//房间是否暂停中
            LiangDaoCanHuCards.Clear();
            CurrOfficNum = 0;//当前局数
            FirstMoCardSeatIndex = 0;//第一个摸牌玩家的索引 第一局就是0
            CurrRestSeatIndex = -1;// 当前休息玩家索引 必须为负数 因为2.3人局流局赔付 有误
            NextRestSeatIndex = 0;// 下个休息玩家索引
            CurrRoomStateType = RoomStateType.None;//当前房间游戏状态
            ParticularMiltarys.Clear();//小局战绩 记录信息清除
        }
        //每小局要清空的数据
        public void LittleRoundClearData()
        {
            ResidueCards = null;
            CanOperatePlayerIndex.Clear();//当前可以行进操作的玩家索引列表
            BeforeOperateResults.Clear();
            HuPaiPlayerSeatIndexs.Clear();
            LiangDaoCanHuCards.Clear();//亮倒 所有玩家可以胡的牌
            CurrChuPaiIndex = -1;//当前出牌玩家的索引
            CurrChuPaiCard = -1;//当前出的哪张牌
            IsDaPiaoBeing = false;
            EntiretyOprationAleadyNum = 0;
            LastMoPaiSeatIndex = -1;//最后一个摸牌玩家的索引
            CanPlayCardPlayerIndex = 0;//正在操作的玩家索引
            QiOperateNextStep = 0;
            _overTime = 0;
            CurrParticularMiltaryRecordData = ComponentFactory.Create<ParticularMiltaryRecordDataInfo>();//重置录像小局数据
            EndCanOperateAndCanChuMessage = null;//最后一条可操作消息清除
            MaiMaCard = -1;//买马买的哪一张牌
            foreach (var player in FiveStarPlayerDic)
            {
                player.Value.LittleRoundClearData();
            }
        }
    }
}
