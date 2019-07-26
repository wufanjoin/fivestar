using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace  ETHotfix
{
  public static class CardFiveStarPlayerFactory
    {
        private static Dictionary<int, CardFiveStarPlayer> _playerDic=new Dictionary<int, CardFiveStarPlayer>();
        private static Dictionary<int,string> _PlayerInPrefabName=new Dictionary<int, string>()
        {
            {0,"PlayerDownUI" },
            {1,"PlayerRightUI" },
            {2,"PlayerUpUI" },
            {3,"PlayerLeftUI" },
        };
        public static CardFiveStarPlayer Creator(User user, int serverSeatIndex,int userServerSeatIndex, int roomNumber,Transform parentTrm,int nowScore)
        {
            int clientSeat=SeatIndexTool.GetClientSeatIndex(serverSeatIndex, userServerSeatIndex, roomNumber);
            return Creator(user, serverSeatIndex, clientSeat, parentTrm, nowScore);
        }
        public static CardFiveStarPlayer Creator(User user,int serverSeatIndex, int clientSeatIndex,Transform parentTrm, int nowScore)
        {
            if (_playerDic.ContainsKey(clientSeatIndex))
            {
                _playerDic[clientSeatIndex].ServerSeatIndex = serverSeatIndex;
                _playerDic[clientSeatIndex].InitUI(user);
                _playerDic[clientSeatIndex].RegisterEvent();
                _playerDic[clientSeatIndex].ScoreChang(nowScore);
                return _playerDic[clientSeatIndex];
            }
            GameObject  prefabGo=ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, _PlayerInPrefabName[clientSeatIndex]) as GameObject;
            GameObject playerGo=GameObject.Instantiate(prefabGo, parentTrm);
            CardFiveStarPlayer cardFiveStarPlayer=null;
            switch (clientSeatIndex)
            {
                case 0:
                    cardFiveStarPlayer=ComponentFactory.Create<CardFiveStarDownPlayer>();
                    break;
                case 1:
                    cardFiveStarPlayer = ComponentFactory.Create<CardFiveStarRightPlayer>();
                    break;
                case 2:
                    cardFiveStarPlayer = ComponentFactory.Create<CardFiveStarUpPlayer>();
                    break;
                case 3:
                    cardFiveStarPlayer = ComponentFactory.Create<CardFiveStarLeftPlayer>();
                    break;
                   
            }
            cardFiveStarPlayer.ClientSeatIndex = clientSeatIndex;
            cardFiveStarPlayer.ServerSeatIndex = serverSeatIndex;
            cardFiveStarPlayer.Init(playerGo);
            cardFiveStarPlayer.InitUI(user);
            cardFiveStarPlayer.RegisterEvent();
            _playerDic[clientSeatIndex] = cardFiveStarPlayer;
            cardFiveStarPlayer.ScoreChang(nowScore);
            return cardFiveStarPlayer;
        }
    }
}
