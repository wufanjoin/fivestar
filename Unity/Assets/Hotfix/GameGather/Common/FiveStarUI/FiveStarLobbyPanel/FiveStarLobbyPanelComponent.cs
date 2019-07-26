using System;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
  
    [UIComponent(UIType.FiveStarLobbyPanel)]
    public class FiveStarLobbyPanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private GameObject mMuChuangViewGo;
        private Button mRecordPerformanceBtn;
        private GameObject mRightBtnViewGo;
        private GameObject mUserInfoViewGo;
        private GameObject mTopBtnViewGo;
        private GameObject mBroadcastViewGo;
        private AudioClip mLobbyBgClip;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mMuChuangViewGo = rc.Get<GameObject>("MuChuangViewGo");
            mRecordPerformanceBtn = rc.Get<GameObject>("RecordPerformanceBtn").GetComponent<Button>();
            mRightBtnViewGo = rc.Get<GameObject>("RightBtnViewGo");
            mUserInfoViewGo = rc.Get<GameObject>("UserInfoViewGo");
            mTopBtnViewGo = rc.Get<GameObject>("TopBtnViewGo");
            mBroadcastViewGo = rc.Get<GameObject>("BroadcastViewGo");
            mLobbyBgClip = rc.Get<AudioClip>("lobbybgMusic");
            
            InitPanel();
        }
        #endregion

        public const string UpLoginTimeKey = "UpLoginTimeKey";
        public void InitPanel()
        {
            muChuangView=mMuChuangViewGo.AddItem<MuChuangView>();
            mRightBtnViewGo.AddItem<RightBtnView>();
            mUserInfoViewGo.AddItem<UserInfoView>();
            mTopBtnViewGo.AddItem<TopBtnView>();
            broadcastView=mBroadcastViewGo.AddItem<BroadcastView>();
            mRecordPerformanceBtn.Add(RecordPerformanceBtnEvent);
            SdkCall.Ins.LocationAction = Location;//定位回调
            SdkCall.Ins.UrlOpenAppAction = UrlOpenApp;//网页打开APP回调
            SdkMgr.Ins.GetLocation();//第一次进入大厅 请求定位一下
            long upLoginTime = long.Parse(PlayerPrefs.GetString(UpLoginTimeKey, "0"));
            if (!TimeTool.TimeStampIsToday(upLoginTime))
            {
                //打开签到界面 
                UIComponent.GetUiView<ActivityAndAnnouncementPanelComponent>().ShowSignInActivity();
            }
            PlayerPrefs.SetString(UpLoginTimeKey,TimeTool.GetCurrenTimeStamp().ToString());
        }
        public  void UrlOpenApp(string message)
        {
            if (Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame == ToyGameId.Lobby)
            {
                CardFiveStarAisle.JoinRoom(message);
            }
        }
        public async void Location(string message)
        {
            S2C_UploadingLocationIp s2CUploading=(S2C_UploadingLocationIp)await SessionComponent.Instance.Session.Call(new C2G_UploadingLocationIp() {Location = message});
            UserComponent.Ins.pSelfUser.Ip = s2CUploading.Ip;
            UserComponent.Ins.pSelfUser.Location = message;
            EventMsgMgr.SendEvent(CommEventID.SelfUserInfoRefresh);
        }
        public void RecordPerformanceBtnEvent()
        {
            UIComponent.GetUiView<MilitaryPanelComponent>().ShowMilitary(UserComponent.Ins.pUserId);
        }

        private MuChuangView muChuangView;
        private BroadcastView broadcastView;
        public override void OnShow()
        {
            base.OnShow();
            broadcastView.Show();
            muChuangView.Show();
            MusicSoundComponent.Ins.PlayMusic(mLobbyBgClip); //播放背景音乐
        }
    }
}
