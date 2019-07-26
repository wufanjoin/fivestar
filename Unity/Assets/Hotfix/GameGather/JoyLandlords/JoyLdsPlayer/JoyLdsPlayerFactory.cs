using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class JoyLdsPlayerFactory
    {
        public static JoyLdsUserPlayer joyLdsUserPlayer;
        public static JoyLdsUserPlayer CreateUserPlayer(User user, ReferenceCollector referenceCollector, int seatServerIndex)
        {
            if (joyLdsUserPlayer != null)
            {
                joyLdsUserPlayer.mUser = user;
                joyLdsUserPlayer.pSeatServerIndex = seatServerIndex;
                joyLdsUserPlayer.pSeatClinetIndex = 0;
                joyLdsUserPlayer.Show();
                return joyLdsUserPlayer;
            }
            joyLdsUserPlayer = new JoyLdsUserPlayer();
            joyLdsUserPlayer.gameObject = referenceCollector.gameObject;
            joyLdsUserPlayer.mUser = user;
            joyLdsUserPlayer.mNameText = referenceCollector.Get<GameObject>("SelfNameText").GetComponent<Text>();
            joyLdsUserPlayer.mBeansNumText = referenceCollector.Get<GameObject>("BeansNumText").GetComponent<Text>();
            joyLdsUserPlayer.mFigureImage = referenceCollector.Get<GameObject>("FigureAppearanceImage").GetComponent<Image>();
            joyLdsUserPlayer.mOperationResulText = referenceCollector.Get<GameObject>("OperationResulText").GetComponent<Text>();
            joyLdsUserPlayer.mPlayShowCardGo = referenceCollector.Get<GameObject>("PlayShowCardGroupGo");
            joyLdsUserPlayer.mLandlordIconLocation = referenceCollector.Get<GameObject>("LandlordIconLocationGo");
            joyLdsUserPlayer.mPrepareGo = referenceCollector.Get<GameObject>("PrepareGo");
            joyLdsUserPlayer.mSlectHandFoulTextGo = referenceCollector.Get<GameObject>("SlectHandFoulTextGo");
            joyLdsUserPlayer.pSeatServerIndex = seatServerIndex;
            joyLdsUserPlayer.pSeatClinetIndex = 0;
            joyLdsUserPlayer.Init();
            return joyLdsUserPlayer;
        }

        private static Dictionary<int, JoyLdsOtherPlayer> joyLdsOtherPlayersPool = new Dictionary<int, JoyLdsOtherPlayer>();

        public static JoyLdsOtherPlayer CreateOtherPlayer(User user, int seatServerIndex)
        {
            int clinetIndex = ServerIndexConvetorClientIndex(seatServerIndex);
            if (joyLdsOtherPlayersPool.ContainsKey(clinetIndex))
            {
                joyLdsOtherPlayersPool[clinetIndex].mUser = user;
                joyLdsOtherPlayersPool[clinetIndex].pSeatServerIndex = seatServerIndex;
                joyLdsOtherPlayersPool[clinetIndex].pSeatClinetIndex = clinetIndex;
                joyLdsOtherPlayersPool[clinetIndex].Show();
                return joyLdsOtherPlayersPool[clinetIndex];
            }
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            GameObject playerLocationGo;
            if (clinetIndex == 1)
            {
                playerLocationGo = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "PlayerLocationOneGo") as GameObject;
            }
            else 
            {
                playerLocationGo = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "PlayerLocationTwoGo") as GameObject;
            }
            
            GameObject insPlayerLocationGo = UnityEngine.Object.Instantiate(playerLocationGo, UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().gameObject.transform);
            ReferenceCollector referenceCollector = insPlayerLocationGo.GetComponent<ReferenceCollector>();
            JoyLdsOtherPlayer joyLdsBasePlayer = new JoyLdsOtherPlayer();
            joyLdsBasePlayer.gameObject = insPlayerLocationGo;
            joyLdsBasePlayer.mUser = user;
            joyLdsBasePlayer.mFigureImage = referenceCollector.Get<GameObject>("FigureAppearanceImage").GetComponent<Image>();
            joyLdsBasePlayer.mOperationResulText = referenceCollector.Get<GameObject>("OperationResulText").GetComponent<Text>();
            joyLdsBasePlayer.mPlayShowCardGo = referenceCollector.Get<GameObject>("PlayShowCardGroupGo");
            joyLdsBasePlayer.mTimerLocation = referenceCollector.Get<GameObject>("TimerLocationGo");
            joyLdsBasePlayer.mLandlordIconLocation = referenceCollector.Get<GameObject>("LandlordIconLocationGo");
            joyLdsBasePlayer.mRediduePaiNumLocationGo = referenceCollector.Get<GameObject>("RediduePaiNumLocationGo");
            joyLdsBasePlayer.mFigureInfoParentGo = referenceCollector.Get<GameObject>("FigureInfoGo");
            joyLdsBasePlayer.mNameText = referenceCollector.Get<GameObject>("NameText").GetComponent<Text>();
            joyLdsBasePlayer.mBeansNumText = referenceCollector.Get<GameObject>("BeansNumText").GetComponent<Text>();
            joyLdsBasePlayer.mPrepareGo = referenceCollector.Get<GameObject>("PrepareGo");
            joyLdsBasePlayer.pSeatServerIndex = seatServerIndex;
            joyLdsBasePlayer.pSeatClinetIndex = clinetIndex;
            joyLdsBasePlayer.Init();
            joyLdsOtherPlayersPool.Add(clinetIndex, joyLdsBasePlayer);
            return joyLdsBasePlayer;
        }
        private static int ServerIndexConvetorClientIndex(int serverIndex)
        {
            if (joyLdsUserPlayer == null)
            {
                Log.Error("当前用户玩家还没有创建 无法转成客户端索引");
                return 0;
            }
            int currUserPlayerIndex = joyLdsUserPlayer.pSeatServerIndex;
            
            //int currUserPlayerIndex = 0;
            if (serverIndex > currUserPlayerIndex)
            {
                return serverIndex - currUserPlayerIndex;
            }
            else if (serverIndex < currUserPlayerIndex)
            {
                return JoyLdsGameRoom.pRoomMaxNumber - (currUserPlayerIndex - serverIndex);//3是人数的意思 斗地主固定是三人的
            }
            else
            {
                return 0;
            }
        }
    }
}
