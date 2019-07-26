using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using ETHotfix;
using ETModel;
using Google.Protobuf.Collections;
using NLog;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarPlayerAwakeSystem : AwakeSystem<FiveStarPlayer>
    {
        public override void Awake(FiveStarPlayer self)
        {
            self.Awake();
        }
    }
    public partial class FiveStarPlayer : Entity
    {
        public FiveStarRoom FiveStarRoom;//房间信息
        public int MoEndHand=-1;//摸的最后一张手牌
        public int SmallGangScore = 0;//小局杠牌得分
        public readonly RepeatedField<int> canOperateLists=new RepeatedField<int>();//可操作列表 只有检测能不能操作的时候才会使用
        public readonly Dictionary<int,int> canGangCards = new Dictionary<int, int>();//可以杠的牌 只有检测能不能杠的时候才会使用 牌的size对应杠牌的类型
        public bool IsCanPlayCard = false;//是否可以出牌
        public RepeatedField<int> LiangDaoNoneCards;//亮倒后 与胡牌无关牌
        public bool IsRestIn = false;//是否休息中
        public bool IsCollocation = false;//是否托管中
        public bool IsAI = false;//是否是AI机器人
        public int MoCardCount = 0;//摸牌的次数 只有AI才会记录
        public int intData;//intData
        public bool boolData = false;//boolData


        private List<int> _pengGangs = new List<int>();//碰杠
        //获取碰杠列表
        public List<int> PengGangs
        {
            get
            {
                _pengGangs.Clear();
                foreach (var operateInfo in OperateInfos)
                {
                    _pengGangs.Add(operateInfo.Card);
                }
                return _pengGangs;
            }
        }
        public int pHandNum
        {
            get { return Hands.Count; }
        }
        public void Awake()
        {

        }
   
        public override void Dispose()
        {
            base.Dispose();
            LittleRoundClearData();
            SeatIndex = 0;
            User = null;
            FiveStarRoom = null;
            HuPaiCount = 0;//胡牌次数
            FangChongCount = 0;//放冲次数
            ZiMoCount = 0;//自摸次数
            GangPaiCount = 0;//杠牌次数
            NowScore = 0;//重置为0
            IsCollocation = false;//托管状态为false
            IsAI = false;//默认肯定不是机器人啊
        }
        //每小局要清空的数据
        public void LittleRoundClearData()
        {
            Hands.Clear();
            MoCardCount = 0;//摸牌的次数 只有AI才会记录
            IsAlreadyDaPiao = false;//是否已经打过漂
            MoEndHand = -1;//摸的最后一张手牌
            SmallGangScore = 0;//小局杠牌得分
            ReadyState = false;//玩家准备状态
            IsCanPlayCard = false;//是否可以出牌
            LiangDaoNoneCards = null;//亮倒无关的牌
            PlayCards.Clear();//出牌信息
            foreach (var operateInfo in OperateInfos)
            {
                operateInfo.Dispose();
            }
            OperateInfos.Clear();//碰杠信息
            PiaoNum = 0;//漂的分数
            IsLiangDao = false;//是否亮倒
            IsRestIn = false;//是否休息中
        }
    }
}
