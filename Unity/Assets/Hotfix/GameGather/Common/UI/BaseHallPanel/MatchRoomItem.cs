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
    [ObjectSystem]
    public class MatchRoomItemAwakeSystem : AwakeSystem<MatchRoomItem, GameObject, MatchRoomConfig>
    {
        public override void Awake(MatchRoomItem self, GameObject go, MatchRoomConfig data)
        {
            self.Awake(go, data, UIType.BaseHallPanel);
        }
    }

    public class MatchRoomItem : BaseItem<MatchRoomConfig>
    {
        private Text mRoomNameText;
        private Text mImposeQuantityText;
        private Text mBottomScoreText;
        public override void Awake(GameObject go, MatchRoomConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            mRoomNameText = rc.Get<GameObject>("RoomNameText").GetComponent<Text>();
            mImposeQuantityText = rc.Get<GameObject>("ImposeQuantityText").GetComponent<Text>();
            mBottomScoreText = rc.Get<GameObject>("BottomScoreText").GetComponent<Text>();
            InitPanel();
        }

        public  void InitPanel()
        {
            this.gameObject.GetComponent<Button>().Add(EnterRoom);
            mRoomNameText.text = mData.Name;
            mImposeQuantityText.text = mData.BesansLowest.ConvertorTenUnit();
            mBottomScoreText.text = "底分:" + mData.BaseScore;
        }

        public void EnterRoom()
        {
            if (Game.Scene.GetComponent<UserComponent>().pSelfUser.Beans < mData.BesansLowest)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel($"豆子不足{mData.BesansLowest}无法进入");
                return;
            }
            switch (Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame)
            {
                case ToyGameId.JoyLandlords:
                    Game.Scene.AddComponent<JoyLdsGameRoom,long>(mData.MatchRoomId); 
                    break;
                default:
                    Log.Error("现在只能进入斗地主");
                    break;
            }
            //G2C_StartMatch g2CStartMatch =
            //    (G2C_StartMatch)await SessionComponent.Instance.Session.Call(new C2G_StartMatch() { MatchRoomId = mData.MatchRoomId });
      


        }
    }
}
