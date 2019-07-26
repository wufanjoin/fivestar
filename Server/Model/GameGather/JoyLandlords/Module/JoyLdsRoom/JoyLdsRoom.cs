using System;
using System.Collections.Generic;
using System.Text;
using System.Text;
using Google.Protobuf.Collections;

namespace ETModel
{
    [ObjectSystem]
    public class JoyLdsRoomAwakeSystem : AwakeSystem<JoyLdsRoom>
    {
        public override void Awake(JoyLdsRoom self)
        {
            self.Awake();
        }
    }
    public class JoyLdsRoom:Entity
    {
        public long RoomId;
        public const float MultipleWinBeans = 5.0f;
        public const int RoomNumber=3;//房间人数
        public const int BombMultiple = 2;//炸弹倍数
        public const int KingBombMultiple = 4;//王炸倍数
        public int BesansLowest = 2000;//在房间游戏豆子最低数
        public Dictionary<int, JoyLdsPlayer> pJoyLdsPlayerDic = new Dictionary<int, JoyLdsPlayer>();//玩家索引对应的玩家实体
        public int CurrRoomStateType = JoyLdsRoomStateType.Unoccupied;//当前房间游戏状态
        public int StartCallLandlordSeatIndex=0; //起始叫地主的玩家索引
        public bool StartPlayerIsCallLds = false; //起始玩家是否叫地主
        public bool IsTheEndOnceCallLds = false; //是否是最后一次叫地主
        public int CurrBeOperationSeatIndex = -1; //当前正在操作的玩家索引（叫.抢，出牌,）
        public int SelectCallOrRobLandlordSeatIndex = -1;//当前选择叫或者抢地主 最优先级索引
        public int LandlordSeatIndex = -1;//当前地主的座位索引
        public int CurrPlayCardSeatIndex = -1;//当前出最后一手出牌玩家的索引
        public RepeatedField<int> CurrPlayCardCards;//当前出的牌
        public RepeatedField<int> LdsThreeCard;//当前地主的三张牌
        public int CurrPlayCardType = 0;//当前出最后一手出牌的类型
        public int BaseMultiple = 0;//基础倍数
        public int CurrMultiple = 0;//当前倍数
        

        public void Awake()
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();//组件会在Dispose被销毁
            foreach (var player in pJoyLdsPlayerDic)
            {
                player.Value.Dispose();
            }
            pJoyLdsPlayerDic.Clear();
            RoomId = 0;
            Reset();
        }

        public void Reset()
        {
            StartCallLandlordSeatIndex = 0; //起始叫地主的玩家索引
            StartPlayerIsCallLds = false; //起始玩家是否叫地主
            IsTheEndOnceCallLds = false; //是否是最后一次叫地主
            CurrBeOperationSeatIndex = -1; //当前正在选择叫或者抢地主的索引
            SelectCallOrRobLandlordSeatIndex = -1;//当前选择叫或者抢地主 最优先级索引
            LandlordSeatIndex = -1;//当前地主的座位索引
            CurrPlayCardSeatIndex = -1;//当前出最后一手出牌玩家的索引
            CurrPlayCardCards=null;//当前出的牌
            CurrPlayCardType = 0;//当前出最后一手出牌的类型
            CurrMultiple = BaseMultiple;//倍数重置
            BesansLowest = 0;//当前房间最低豆子数
        }
    }
}
