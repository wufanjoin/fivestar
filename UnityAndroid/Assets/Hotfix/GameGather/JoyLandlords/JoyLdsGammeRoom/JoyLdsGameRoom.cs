using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using NPOI.HSSF.Record;

namespace ETHotfix
{
    [ObjectSystem]
    public class JoyLdsGameRoomAwakeSystem : ETHotfix.AwakeSystem<JoyLdsGameRoom,long>
    {
        public override void Awake(JoyLdsGameRoom self,long roomId)
        {
            self.Awake(roomId);
        }
    }
   
    //进入房间开始游戏的时候会被添加
    public class JoyLdsGameRoom : Entity
    {

        private Dictionary<int, JoyLdsBasePlayer> mRoomPlayerDic=new Dictionary<int, JoyLdsBasePlayer>();//服务器索引对象玩家

        public const int pRoomMaxNumber = 3;//房间最大人数

        private int mCurrGameRoomState = JoyLdsGameState.NoneStart;//当前房间的游戏状态

        private JoyLdsRoomData mJoyLdsRoomData=new JoyLdsRoomData();//这个房间的一些信息

        protected int mLandlordSeatIndex = -1;//当前地主索引

        public int pCurrUserPlayerIndex = 0; //用户所在房间的服务器索引位置

        public RepeatedField<int> UpPlayCardArray;//上次出牌 牌数组

        public int UpPlayCardType= PlayCardType.None;//上次出牌 牌类型

        private JoyLandlordsHandComponent _joyLandlordsHandComponent;//手牌组件

        private JoyLandlordsRoomPanelComponent _joyLandlordsRoomPanelComponent;//打牌房间界面组件
        //添加这个组件相当于进入游戏房间
        public void Awake(long roomId)
        {
            mJoyLdsRoomData.RoomId = roomId;
            Game.Scene.GetComponent<UserComponent>().SetRoomId(roomId);
            _joyLandlordsRoomPanelComponent = UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>();
            _joyLandlordsRoomPanelComponent.Show();//显示房间UI
            RegisterEvent();//注册事件
            _joyLandlordsHandComponent=this.AddComponent<JoyLandlordsHandComponent>();//添加手牌组件
            _joyLandlordsRoomPanelComponent.InitEnterRoomUI();//初始化进入房间UI
            ResetData();//初始化房间数据


           // JoyLdsPlayerFactory.CreateOtherPlayer(Game.Scene.GetComponent<UserComponent>().pSelfUser,1).PlayCard(new RepeatedField<int>() { 40, 39, 12, 50, 10 }, new RepeatedField<int>() { 7 });
           // JoyLdsPlayerFactory.joyLdsUserPlayer.PlayCard(new RepeatedField<int>() { 40, 39, 12, 50, 10 },new RepeatedField<int>(){7});
     
            return;
            //模拟发牌
            RepeatedField<int> cards = new RepeatedField<int>();
            cards.Add(2);
            cards.Add(3);
            cards.Add(4);
            cards.Add(5);
            cards.Add(6);
            cards.Add(7);
            cards.Add(8);
            cards.Add(9);
            cards.Add(10);
            cards.Add(11);
            cards.Add(12);
            cards.Add(13);
            cards.Add(14);
            cards.Add(15);
            cards.Add(16);
            cards.Add(17);
            _joyLandlordsHandComponent.Deal(cards);
        }

        public void RegisterEvent()
        {
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.RequestOutRoom, RequestOutRoom);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.RequestPlayCard, RequestPlayCard);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.StarMathch, StarMathch);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.RobLanlord, RobLanlord);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.AddTwice, AddTwice);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.CallLanlord, CallLanlord);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.CanCallLanlord, CanCallLanlord);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.CanPlayCard, CanPlayCard);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.CanRobLanlord, CanRobLanlord); 
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.DissolveRoom, DissolveRoom);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.DontPlay, DontPlay);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.PlayCard, PlayCard);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.Prepare, Prepare);
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.ConfirmCamp, ConfirmCamp);
        }
        //退出房间请求
        public void RequestOutRoom(params object[] objs)
        {
            switch (mCurrGameRoomState)
            {
                case JoyLdsGameState.BeingMatching:
                     //SessionComponent.Instance.Send(new C2M_CancelMatch());
                     break;
                default:
                    break;
            }
            OutRoom();
        }
        //退出房间
        public void OutRoom()
        {
            Game.Scene.GetComponent<ToyGameComponent>().EndGame();
        }
        //出牌请求
        public void RequestPlayCard(params object[] objs)
        {
            int thePlayCardType = PlayCardType.None;
            RepeatedField<int> pithCards = _joyLandlordsHandComponent.GetPithCards();
            if (isFirstPlayCard)
            {
                thePlayCardType=JoyLdsGamePlayHandLogic.PlayerPlayHandIsRational(pithCards);
            }
            else
            {
                thePlayCardType=JoyLdsGamePlayHandLogic.PlayerPlayHandIsRational(UpPlayCardType, UpPlayCardArray, pithCards);
            }
            if (thePlayCardType != PlayCardType.None)
            {
                Actor_JoyLds_PlayCard actorJoyLdsPlayCard =
                    new Actor_JoyLds_PlayCard() {Cards = pithCards, PlayCardType = thePlayCardType};
              SessionComponent.Instance.Send(actorJoyLdsPlayCard as IMessage);  
            }
            else
            {
                _joyLdsUserPlayer.ShowSlectHandFoulTextGo();
            }
        }
        private int _PlayAddTwiceCount = 0;
        //玩家加倍
        public void AddTwice(params object[] objs)
        {
            int seatIndex = (int) objs[0];
            bool result = (bool)objs[1];
            mRoomPlayerDic[seatIndex].AddTwice(result);
            _PlayAddTwiceCount++;
            if (_PlayAddTwiceCount >= pRoomMaxNumber)
            {
                UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().ShowOrHideWaitAddTwice(false);
            }
            
        }
        //玩家叫地主
        public void CallLanlord(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            bool result = (bool)objs[1];
            mRoomPlayerDic[seatIndex].CallLanlord(result);
        }
        //玩家可以叫地主
        public void CanCallLanlord(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            mRoomPlayerDic[seatIndex].CanCallLanlord(10);
        }
        //是否首次出牌
        private bool isFirstPlayCard = false;
        //玩家可以出牌
        public void CanPlayCard(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            isFirstPlayCard = (bool)objs[1];
            mRoomPlayerDic[seatIndex].CanPlayCard(isFirstPlayCard,10);
            if (isFirstPlayCard)
            {
                UpPlayCardArray = null;
            }
        }
        //确定阵营
        public void ConfirmCamp(params object[] objs)
        {
            int landlordSeatIndex = (int)objs[0];
            RepeatedField<int> hands = (RepeatedField<int>)objs[1];
            RepeatedField<int> threeHand = (RepeatedField<int>)objs[2];
            _joyLandlordsRoomPanelComponent.ShowLandlordThreeCard(threeHand);
            mRoomPlayerDic[landlordSeatIndex].TurnLandlord(hands, threeHand);//玩家成为地主
        }
        //可以抢地主
        public void CanRobLanlord(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            mRoomPlayerDic[seatIndex].CanRobLanlord(10);
        }
        //解散房间
        public void DissolveRoom(params object[] objs)
        {
            mCurrGameRoomState = JoyLdsGameState.AlreadyDissolv;
        }
        //不出
        public void DontPlay(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            mRoomPlayerDic[seatIndex].DontPlay();
        }
        //出牌
        public void PlayCard(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            UpPlayCardType = (int)objs[1];
            RepeatedField<int> cards = objs[2] as RepeatedField<int>;
            RepeatedField<int> hands = objs[3] as RepeatedField<int>;
            UpPlayCardArray = cards;
            mRoomPlayerDic[seatIndex].PlayCard(cards, hands);
        }
        //准备
        public void Prepare(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            mRoomPlayerDic[seatIndex].Prepare();
        }
        //玩家抢地主
        public void RobLanlord(params object[] objs)
        {
            int seatIndex = (int)objs[0];
            bool result = (bool)objs[1];
            mRoomPlayerDic[seatIndex].RobLanlord(result);
        }
        //玩家开始匹配
        public async void StarMathch(params object[] objs)
        {
            Log.Debug(SessionComponent.Instance.Session.Id.ToString());
            M2C_StartMatch g2CStartMatch = (M2C_StartMatch)await SessionComponent.Instance.Session.Call(
                new C2M_StartMatch() { MatchRoomId = mJoyLdsRoomData.RoomId });

            if (mCurrGameRoomState == JoyLdsGameState.BeingPlayCards)
            {
                return;
            }
            UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().StartMath();
            mCurrGameRoomState = JoyLdsGameState.BeingMatching;//当前房间的游戏状态
        }

        public JoyLdsUserPlayer _joyLdsUserPlayer;
        //开始游戏
        public void StartGame(Actor_JoyLds_StartGame joyLdsPlayerInfos)
        {
            ResetData();//重置数据
            mCurrGameRoomState = JoyLdsGameState.BeingPlayCards;//当前房间的游戏状态
            UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().MathSucceed();//匹配成功
            long selfUserId = Game.Scene.GetComponent<UserComponent>().pUserId;
            //创建用户角色
            for (int i = 0; i < joyLdsPlayerInfos.PlayerInfos.Count; i++)
            {
                if (joyLdsPlayerInfos.PlayerInfos[i].User.UserId == selfUserId)
                {
                    _joyLdsUserPlayer = UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().ShowUserPlayerInfo(joyLdsPlayerInfos.PlayerInfos[i].SeatIndex);
                    pCurrUserPlayerIndex=joyLdsPlayerInfos.PlayerInfos[i].SeatIndex;
                    mRoomPlayerDic[i] = _joyLdsUserPlayer;
                }
            }
            //创建其他玩家
            for (int i = 0; i < joyLdsPlayerInfos.PlayerInfos.Count; i++)
            {
                if (joyLdsPlayerInfos.PlayerInfos[i].User.UserId != selfUserId)
                {
                    JoyLdsBasePlayer joyLdsBasePlayer = UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().ShowOtherPlayer(joyLdsPlayerInfos.PlayerInfos[i].User, joyLdsPlayerInfos.PlayerInfos[i].SeatIndex);
                    mRoomPlayerDic[i] = joyLdsBasePlayer;
                }
            }
        }

        public void ResetData()
        {
            mCurrGameRoomState = JoyLdsGameState.NoneStart;
            pCurrUserPlayerIndex = 0;//当前玩家服务器座位索引
            mLandlordSeatIndex = -1;//当前地主索引
            _PlayAddTwiceCount = 0;//当前已经加倍的人数
            _joyLdsUserPlayer = null;
            UpPlayCardArray = null;
            isFirstPlayCard = false;
        }
        //退出的时候会被销毁
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
