using System.IO.Ports;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    [ToyGame(ToyGameId.CardFiveStar)]
    public class CardFiveStarAisle : ToyGameAisleBase
    {
        public override void Awake(long gameType)
        {
            base.Awake(gameType);

        }

        public override void StartGame(params object[] objs)
        {
            //参数 0房间配置 1房间ID 2房间类型
            base.StartGame();
            Log.Debug("进入游戏卡五星");
          // Game.Scene.AddComponent<CardFiveStarRoom, RepeatedField<int>,int>(roomConfigs, _roomType);

            CardFiveStarRoom  cardFiveStarRoom=Game.Scene.AddComponent<CardFiveStarRoom>();
            cardFiveStarRoom.SetConfigInfo((RepeatedField<int>)objs[0]);
            cardFiveStarRoom._RoomId = (int)objs[1];
            cardFiveStarRoom._RoomType = (int)objs[2];
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().Show();
        }

        //房卡模式正常进入进入游戏 加入房间 创建房间 在等待开局状态重连
        public static void RoomCardEnterRoom(RoomInfo roomInfo)
        {
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.CardFiveStar, roomInfo.RoomConfigLists, roomInfo.RoomId, RoomType.RoomCard);
            CardFiveStarRoom.Ins._RoomState = RoomStateType.AwaitPerson;//状态改为等待中
            CardFiveStarRoom.Ins._FriendsCircleId = roomInfo.FriendsCircleId;//更改亲友圈id
            EventMsgMgr.SendEvent(CardFiveStarEventID.HideAllPlayer);//先隐藏所有玩家
            CardFiveStarRoom.Ins.ShowPlayerInfo(roomInfo.MatchPlayerInfos);//显示服务器发过来的玩家信息
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutRoomCardEnterReadyInUI();//UI切换为房卡等待开始
        }

        //匹配模式进入房间
        public static void MatchingEnterRoom(int roomId,RepeatedField<int> roomConfigs)
        {
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.CardFiveStar, roomConfigs, roomId, RoomType.Match);
            EventMsgMgr.SendEvent(CardFiveStarEventID.HideAllPlayer);//先隐藏所有玩家
            CardFiveStarPlayerFactory.Creator(Game.Scene.GetComponent<UserComponent>().pSelfUser, 1, 0,
                UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().mPlayerUIsGo.transform,(int)UserComponent.Ins.pSelfUser.Beans);//显示自己的信息
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutBeginStartPrepareUI();//UI切换为准备开始匹配
        }
        //创建房间
        public static  void CreateRoom(RepeatedField<int> configs)
        {
            CreateRoom(configs, 0);
        }
        //创建房间
        public static async void CreateRoom(RepeatedField<int> configs,int friendsCircleId)
        {
            C2M_CreateRoom c2MCreateRoom = new C2M_CreateRoom();
            c2MCreateRoom.RoomConfigLists = configs;
            c2MCreateRoom.ToyGameId = ToyGameId.CardFiveStar;
            c2MCreateRoom.FriendsCircleId = friendsCircleId;
            M2C_CreateRoom m2CCreate = (M2C_CreateRoom)await SessionComponent.Instance.Call(c2MCreateRoom);
            if (!string.IsNullOrEmpty(m2CCreate.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(m2CCreate.Message);
                return;
            }
            RoomCardEnterRoom(m2CCreate.RoomInfo);
        }

        //加入房间
        public static void JoinRoom(string url)
        {
            //看是否 有参数加入房间
            if (!string.IsNullOrEmpty(url))
            {
                string roomIdStr = url.GetUrlParameter(GlobalConstant.UrlJionRoomParametName);
                if (!string.IsNullOrEmpty(roomIdStr))
                {
                    JoinRoom(int.Parse(roomIdStr));
                }
            }
        }
        //加入房间
        public static async void JoinRoom(int roomId)
        {
            M2C_JoinRoom m2CJoinRoom=(M2C_JoinRoom)await SessionComponent.Instance.Call(new C2M_JoinRoom() {RoomId = roomId});
            if (!string.IsNullOrEmpty(m2CJoinRoom.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(m2CJoinRoom.Message);
                return;
            }
            RoomCardEnterRoom(m2CJoinRoom.RoomInfo);
            UIComponent.GetUiView<JoinRoomPanelComponent>().Hide();//不管怎么进入 隐藏一下加入面板
        }

        //重连进入游戏
        public static void Reconnection(Actor_FiveStar_Reconnection roomData)
        {
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.CardFiveStar, roomData.Configs, roomData.RoomId, roomData.RoomType);
            CardFiveStarRoom.Ins._RoomState = roomData.CurrRoomStateType;//状态改为等待中
            CardFiveStarRoom.Ins._FriendsCircleId = roomData.FriendsCircleId;//更改亲友圈id
            if (roomData.ResidueCardCount > 0)
            {
                CardFiveStarRoom.Ins._ResideNum = roomData.ResidueCardCount;//剩余牌数赋值
            }
            CardFiveStarRoom.Ins.iEndChuCardSize=roomData.EndPlayCardSize;//出牌的最后一张牌
            CardFiveStarRoom.Ins._CuurRoomOffice = roomData.CurrOfficNum;//当前局数赋值
            CardFiveStarRoom.Ins._RoomId = roomData.RoomId;//roomId赋值
            EventMsgMgr.SendEvent(CardFiveStarEventID.HideAllPlayer);//先隐藏所有玩家
            CardFiveStarRoom.Ins.ShowPlayerInfo(roomData.Players);//显示服务器发过来的玩家信息
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().SetResidueNum(CardFiveStarRoom.Ins._ResideNum);//设置剩余牌数
            CardFiveStarRoom.Ins.SetUserPlayerAsLastSibling();//设置自己的节点到最后
            if (CardFiveStarRoom.Ins._config.RoomNumber == 4)//四人房还有玩家轮休
            {
                CardFiveStarRoom.Ins.PlayerRest(roomData.CurrRestSeatIndex);//当前休息玩家索引
            }
            for (int i = 0; i < roomData.Players.Count; i++)
            {
                CardFiveStarRoom.Ins._ServerSeatIndexInPlayer[roomData.Players[i].SeatIndex].Reconnection(roomData.Players[i],
                    roomData.CurrRoomStateType, roomData.IsDaPiaoBeing);//显示玩家对应的信息
            }
            if (roomData.CurrRoomStateType == RoomStateType.ReadyIn)
            {
                UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutReadyInUI();//UI切换为准备中的状态
            }
            else
            {
                UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().CutGameInUI();//UI切换为游戏中的状态
            }
            
        }
        public override async void EndGame()
        {
            //不一定调   玩家调用直接开启其他游戏就不会调
            base.EndGame();//默认就是进入大厅界面
            if (_UpRoomFriendsCircleId>0)
            {
                await FrienCircleComponet.Ins.ShowFrienCircle();
            }
        }

        private int _UpRoomFriendsCircleId = 0;
        public override  void EndAndStartOtherGame()
        {
            //一定调  不管调用进入其他游戏 还是调用结算本游戏
            base.EndAndStartOtherGame();
            _UpRoomFriendsCircleId = CardFiveStarRoom.Ins._FriendsCircleId;
            Game.Scene.RemoveComponent<CardFiveStarRoom>();
        }
    }
}
