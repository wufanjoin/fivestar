using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class CardFiveRoomMusicType
    {
        public const int RoomBg = 1;
        public const int Win = 2;
        public const int Lose = 3;
    }
    [UIComponent(UIType.CardFiveStarRoomPanel)]
    public class CardFiveStarRoomPanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private GameObject mTimeTurntableGo;
        private Text mTimeText;
        private Button mMilitaryBtn;
        private Text mRoomInfoText;
        private Button mWanFaBtn;
        private Button mSetBtn;
        private Button mVoiceBtn;
        private Button mChatBtn;
        private Button mMatchBtn;
        private GameObject mMatchingInGo;
        private Text mReidueCardNumText;
        private Button mInviteBtn;
        private GameObject mReidueCardNumBgGo;
        public GameObject mPlayerUIsGo;
        private GameObject mWifiAndTimeAndCellGo;
        private AudioClip mRoomBgClip;
        private AudioClip mWinClip;
        private AudioClip mLoseClip;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mTimeTurntableGo = rc.Get<GameObject>("TimeTurntableGo");
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mMilitaryBtn = rc.Get<GameObject>("MilitaryBtn").GetComponent<Button>();
            mRoomInfoText = rc.Get<GameObject>("RoomInfoText").GetComponent<Text>();
            mWanFaBtn = rc.Get<GameObject>("WanFaBtn").GetComponent<Button>();
            mSetBtn = rc.Get<GameObject>("SetBtn").GetComponent<Button>();
            mVoiceBtn = rc.Get<GameObject>("VoiceBtn").GetComponent<Button>();
            mChatBtn = rc.Get<GameObject>("ChatBtn").GetComponent<Button>();
            mMatchBtn = rc.Get<GameObject>("MatchBtn").GetComponent<Button>();
            mMatchingInGo = rc.Get<GameObject>("MatchingInGo");
            mReidueCardNumBgGo= rc.Get<GameObject>("ReidueCardNumBgGo");
            mReidueCardNumText = rc.Get<GameObject>("ReidueCardNumText").GetComponent<Text>();
            mInviteBtn = rc.Get<GameObject>("InviteBtn").GetComponent<Button>();
            mPlayerUIsGo = rc.Get<GameObject>("PlayerUIsGo");
            mWifiAndTimeAndCellGo = rc.Get<GameObject>("WifiAndTimeAndCellGo");
            mRoomBgClip = rc.Get<AudioClip>("roombgMusic");
            mWinClip = rc.Get<AudioClip>("winMusic");
            mLoseClip = rc.Get<AudioClip>("loseMusic");
            InitPanel();
        }
        #endregion

        //隐藏的时候 同时隐藏所有子视图
        public override void OnHideBefore()
        {
            base.OnHideBefore();
            mUIComponent.HideCanvsTypeAllView(CanvasType.PopUpCanvas);// 隐藏所有弹窗视图
        }

        private WifiAndTimeAndCellGoView _wifiAndTimeAndCell;
        public void InitPanel()
        {
            _wifiAndTimeAndCell=mWifiAndTimeAndCellGo.AddItem<WifiAndTimeAndCellGoView>();
            TurntableTimeMgr.Ins.InitUI(mTimeTurntableGo);
            mSetBtn.Add(SetBtnEvent);
            mWanFaBtn.Add(WanFaBtnEvent);
            mMatchBtn.Add(MatchBtnEvent);
            mMilitaryBtn.Add(UIComponent.GetUiView<FiveStarGradePanelComponent>().ShowPlayerGrade);
            mChatBtn.Add(ChatBtnEvent);
            mInviteBtn.Add(InviteBtnEvent);
            ChatMgr.Ins.RegisterVoiceBtn(mVoiceBtn.gameObject);
        }

        public override void OnShow()
        {
            base.OnShow();
            mMilitaryBtn.gameObject.SetActive(false);//战绩按钮 默认不显示
            _wifiAndTimeAndCell.RenewalInfo();//更新wifi 电量信息
            PlayMusic(CardFiveRoomMusicType.RoomBg); //播放背景音乐
            mVoiceBtn.gameObject.SetActive(true);//显示语音按钮
            mChatBtn.gameObject.SetActive(true);//显示聊天按钮
            mSetBtn.gameObject.SetActive(true);//显示设置按钮
        }

        //播放背景音乐
        public void PlayMusic(int musicType)
        {
            switch (musicType)
            {
                case CardFiveRoomMusicType.Lose:
                    MusicSoundComponent.Ins.StopMusic();
                    MusicSoundComponent.Ins.PlayMusicOneShot(mLoseClip); //播放失败音乐
                    break;
                case CardFiveRoomMusicType.Win:
                    MusicSoundComponent.Ins.StopMusic();
                    MusicSoundComponent.Ins.PlayMusicOneShot(mWinClip); //播放胜利音乐
                    break;
                case CardFiveRoomMusicType.RoomBg:
                    MusicSoundComponent.Ins.PlayMusic(mRoomBgClip); //播放背景音乐
                    break;
                default:
                    MusicSoundComponent.Ins.PlayMusic(mRoomBgClip); //播放背景音乐
                    break;
            }
        }

        public void InviteBtnEvent()
        {
            ShareUrlMgr.RoomShare(CardFiveStarRoom.Ins._RoomId, CardFiveStarRoom.Ins._config, CardFiveStarRoom.Ins._ServerSeatIndexInPlayer.Count);//分享房间
        }
        public void ChatBtnEvent()
        {
           UIComponent.GetUiView<ChatImportPanelComponent>().Show();
        }
        public void SetBtnEvent()
        {
            if (CardFiveStarRoom.Ins == null)
            {
                return;
            }
            UIComponent.GetUiView<CardFiveStarRoomSetPanelComponent>().Show();
        }
        public void WanFaBtnEvent()
        {
           UIComponent.GetUiView<WanFaPanelComponent>().Show();
           // Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
        }
        public async void MatchBtnEvent()
        {
            bool isMatchSucceed=await UIComponent.GetUiView<MatchingRoomPanelComponent>().StartMatch(CardFiveStarRoom.Ins._RoomId);
            if(isMatchSucceed)
            {
                CutMatchingUI();
            }
        }
        //切换房卡刚进入房间准备中的UI
        public void CutRoomCardEnterReadyInUI()
        {
            mMatchBtn.gameObject.SetActive(false);
            mMatchingInGo.SetActive(false);
            mReidueCardNumBgGo.SetActive(false);
            mTimeTurntableGo.SetActive(false);
            mInviteBtn.gameObject.SetActive(true);
            EventMsgMgr.SendEvent(CardFiveStarEventID.CutRoomCardEnterReadyIn);
            ShowRoomInfo();
        }

        //切换游戏中的UI
        public void CutGameInUI()
        {
            mMilitaryBtn.gameObject.SetActive(CardFiveStarRoom.Ins._RoomType == RoomType.RoomCard);//战绩按钮 只有房卡模式才显示
            mMatchBtn.gameObject.SetActive(false);
            mMatchingInGo.SetActive(false);
            mTimeTurntableGo.SetActive(true);
            mInviteBtn.gameObject.SetActive(false);
            EventMsgMgr.SendEvent(CardFiveStarEventID.CutGameIn);
            ShowRoomInfo();
        }

        //显示房间上方的信息
        private void ShowRoomInfo()
        {
            if (CardFiveStarRoom.Ins._RoomType == RoomType.Match)
            {
                mRoomInfoText.text = "底分" + CardFiveStarRoom.Ins._config.BottomScore;
            }
            else if (CardFiveStarRoom.Ins._RoomType == RoomType.RoomCard)
            {
                mRoomInfoText.text = "房号:"+ CardFiveStarRoom.Ins._RoomId+"      "+ CardFiveStarRoom.Ins._CuurRoomOffice+"/"+
                                     CardFiveStarRoom.Ins._config.MaxJuShu+"局      "+ CardFiveStarRoom.Ins._config.RoomNumber + "人局";
            }
        }
        //切换准备开始匹配中的UI
        public void CutBeginStartPrepareUI()
        {
            mMatchBtn.gameObject.SetActive(true);
            mMatchingInGo.SetActive(false);
            mReidueCardNumBgGo.SetActive(false);
            mInviteBtn.gameObject.SetActive(false);
            mTimeTurntableGo.SetActive(false);
            EventMsgMgr.SendEvent(CardFiveStarEventID.CutBeginStartPrepare);
            ShowRoomInfo();
        }

        //切换匹配中的UI
        public void CutMatchingUI()
        {
            if (CardFiveStarRoom.Ins._RoomState == RoomStateType.GameIn)
            {
                return;
            }
            EventMsgMgr.SendEvent(CardFiveStarEventID.CutMatchIn);
            mMatchBtn.gameObject.SetActive(false);
            mTimeTurntableGo.SetActive(false);
            mReidueCardNumBgGo.SetActive(false);
            mMatchingInGo.SetActive(true);
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().Hide();//隐藏胡牌提示
        }
        //切换准备中的UI
        public void CutReadyInUI()
        {
            ShowRoomInfo();
            mReidueCardNumBgGo.SetActive(false);
            EventMsgMgr.SendEvent(CardFiveStarEventID.CutReadyIn);
        }
        //切换观看录像中的UI
        public void CutWatchVideoInUI()
        {
            mMatchBtn.gameObject.SetActive(false);
            mInviteBtn.gameObject.SetActive(false);
             mVoiceBtn.gameObject.SetActive(false);
            mChatBtn.gameObject.SetActive(false);
            mSetBtn.gameObject.SetActive(false);//显示设置按钮
            mTimeTurntableGo.SetActive(true);//转盘
        }
        //显示房间上方的信息
        public void SetRoomInfo(string content)
        {
           mRoomInfoText.text = content;
        }
        //设置剩余牌数
        public void SetResidueNum(int count)
        {
            mReidueCardNumBgGo.SetActive(true);
            mReidueCardNumText.text = "剩余:" + count;
        }
    }
}
