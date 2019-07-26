using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_BeingInGameHandler : AMHandler<Actor_BeingInGame>
    {
        protected override void Run(ETModel.Session session, Actor_BeingInGame message)
        {
            long currToyGameId= Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame;
            if (message.IsGameBeing)
            {
                if (currToyGameId == ToyGameId.Lobby || currToyGameId == ToyGameId.Login)//在游戏中 当前场景如果在大厅或者登陆界面 就给个提示
                {
                    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("您有一局未完成的游戏点击确定进入游戏", PopConfirmCall, PopOptionType.Single);
                }
                else//如果在其他场景就 直接发起重连
                {
                    PopConfirmCall(true);
                }
            }
            else
            {
                if (currToyGameId != ToyGameId.Lobby && currToyGameId != ToyGameId.Login)//不在游戏中 当前场景 不是大厅和登陆界面 就进入大厅
                {
                    Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Lobby);
                }
                //看是否 有参数加入房间
                if (!string.IsNullOrEmpty(SdkCall.OpenAppUrl))
                {
                    CardFiveStarAisle.JoinRoom(SdkCall.OpenAppUrl);
                }
            }
            SdkCall.OpenAppUrl = string.Empty;
        }

        public async void PopConfirmCall(bool isConfirm)
        {
            M2C_GetReconnectionRoomInfo m2CGetReconnection=(M2C_GetReconnectionRoomInfo) await SessionComponent.Instance.Call(new C2M_GetReconnectionRoomInfo());//请求断线重连数据
            if (m2CGetReconnection.IsGameBeing)
            {
                //这个roomifno 信息 不为空 表示在等待状态
                if (m2CGetReconnection.RoomInfos != null)
                {
                    CardFiveStarAisle.RoomCardEnterRoom(m2CGetReconnection.RoomInfos);//只有卡五星 而且在等待状态重连 只有可能是房卡模式
                }
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("房间已解算");
            }
        }
    }
}
