using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.BaseHallPanel)]
    public class BaseHallPanelComponent:NormalUIView
    {
        #region 脚本工具生成的代码
        private GameObject mRoomItemGo;
        private Image mFigureImage;
        private Button mSpeedinessBtn;
        private Text mGameNameText;
        public Transform HandPatent;
        public Transform NewHandPatent;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mRoomItemGo=rc.Get<GameObject>("RoomItemGo");
            mFigureImage=rc.Get<GameObject>("FigureImage").GetComponent<Image>();
            mSpeedinessBtn=rc.Get<GameObject>("SpeedinessBtn").GetComponent<Button>();
            mGameNameText=rc.Get<GameObject>("GameNameText").GetComponent<Text>();
            HandPatent = rc.Get<GameObject>("HandPatent").transform;
            NewHandPatent = rc.Get<GameObject>("NewHandPatent").transform;
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            this.InitChildView(UIType.LobbyTopPanel);
            mSpeedinessBtn.Add(TestRecord);
        }

        private bool isRcordIn = false;
        public void TestRecord()
        {
            if (isRcordIn)
            {
                Log.Debug("结束录音");
                byte[] recordBytes=SpeexRecordMgr.Ins.EndRecording();
                if (recordBytes != null)
                {
                    MusicSoundComponent.Ins.PlaySound(SpeexRecordMgr.Ins.RecordClip);
                }
                isRcordIn = false;
               
            }
            else
            {
                Log.Debug("开始录音");
                SpeexRecordMgr.Ins.StartRecording();
                isRcordIn = true;
            }
        }
        private GameVersionsConfig mCurrGameVersionsConfig;
        public async void ShowChangeBaseHallUI(long toyGameType)
        {
            Show();
         //  mCurrGameVersionsConfig= Game.Scene.GetComponent<GameVersionsConfigComponent>().GetGameVersionsConfig(toyGameType);
           // mGameNameText.text = mCurrGameVersionsConfig.Name;
            if (toyGameType == ToyGameId.JoyLandlords)
            {
                L2C_GetMatchRoomConfigs g2CGetMatchRoomConfigs =(L2C_GetMatchRoomConfigs)await SessionComponent.Instance.Session.Call(new C2L_GetMatchRoomConfigs(){ToyGameId = toyGameType });
                InitMatchRoomInfo(g2CGetMatchRoomConfigs.MatchRoomConfigs);
               Log.Info(g2CGetMatchRoomConfigs.MatchRoomConfigs[0].MatchRoomId.ToString()); 
            }
        }
            

        public void InitMatchRoomInfo(RepeatedField<MatchRoomConfig> matchRoomConfigs)
        {
            mRoomItemGo.CopyThisNum(matchRoomConfigs.count);
            Transform roomItemGoParent = mRoomItemGo.transform.parent;
            for (int i = 0; i < matchRoomConfigs.count; i++)
            {
                roomItemGoParent.GetChild(i).AddItemIfHaveInit<MatchRoomItem, MatchRoomConfig>(matchRoomConfigs[i]);
            }
        }
    }
}
