using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.CardFiveStarRoomSetPanel)]
    public class CardFiveStarRoomSetPanelComponent : PopUpUIView
    {
        public override float MaskLucencyValue => 0f;
        #region 脚本工具生成的代码
        private Button mOutBtn;
        private Text mOutDescText;
        private Button mRuleBtn;
        private Toggle mHuaLi3dToggle;
        private Toggle mJingDian2dToggle;
        private Slider mMusicSlider;
        private Slider mSoundSlider;
        private Toggle mExpressionToggle;
        private Button mCloseBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mOutBtn = rc.Get<GameObject>("OutBtn").GetComponent<Button>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mOutDescText = rc.Get<GameObject>("OutDescText").GetComponent<Text>();
            mRuleBtn = rc.Get<GameObject>("RuleBtn").GetComponent<Button>();
            mHuaLi3dToggle = rc.Get<GameObject>("HuaLi3dToggle").GetComponent<Toggle>();
            mJingDian2dToggle = rc.Get<GameObject>("JingDian2dToggle").GetComponent<Toggle>();
            mMusicSlider = rc.Get<GameObject>("MusicSlider").GetComponent<Slider>();
            mSoundSlider = rc.Get<GameObject>("SoundSlider").GetComponent<Slider>();
            mExpressionToggle = rc.Get<GameObject>("ExpressionToggle").GetComponent<Toggle>();
            InitPanel();
        }
        #endregion
        public override bool isShakeAnimation
        {
            get { return false; }
        }
        private MusicSoundComponent _MusicSoundComponent;
        public void InitPanel()
        {
            _MusicSoundComponent = Game.Scene.GetComponent<MusicSoundComponent>();
            mCloseBtn.Add(Hide);
            mOutBtn.Add(OutBtnEvent);
            mRuleBtn.Add(RuleBtnEvent);
           
            mMusicSlider.Add(MusiceSliderEvent);
            mSoundSlider.Add(SoundSliderEvent);
        }
        public void MusiceSliderEvent(float value)
        {
            _MusicSoundComponent.MusicVolume = value;
        }
        public void SoundSliderEvent(float value)
        {
            _MusicSoundComponent.SoundVolume = value;
        }
        public void RuleBtnEvent()
        {
            UIComponent.GetUiView<RulePanelComponent>().Show();
        }
        public void OutBtnEvent()
        {
            if (CardFiveStarRoom.Ins._RoomType == RoomType.RoomCard)
            {
                if ( CardFiveStarRoom.Ins._RoomState == RoomStateType.GameIn||
                    CardFiveStarRoom.Ins._RoomState == RoomStateType.ReadyIn)
                {
                    //申请解算房间
                    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("是否申请解散房间？", SendOutRoom);
                }
                else 
                {
                    if (CardFiveStarRoom.Ins._ServerSeatIndexInPlayer[0]._user.UserId == UserComponent.Ins.pUserId)
                    {
                        //房主退出房间
                        UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("未开始一局游戏,是否解散房间？", SendOutRoom);
                    }
                    else
                    {
                        //非房主退出房间
                        UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("是否退出房间？", SendOutRoom);
                    }
                }
            }
            else
            {
                if (CardFiveStarRoom.Ins._RoomState == RoomStateType.GameIn)
                {
                    //匹配模式下离开房间 
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("无法退出!请等待游戏结束");
                }
                else
                {
                    Hide();
                    Game.Scene.GetComponent<ToyGameComponent>().EndGame();
                }
                   
            }
        }

        public async void SendOutRoom(bool isConfirm)
        {
            if (!isConfirm)
            {
                return;
            }
            M2C_OutRoom m2COut=(M2C_OutRoom)await SessionComponent.Instance.Call(new C2M_OutRoom());
            if (!string.IsNullOrEmpty(m2COut.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(m2COut.Message);
            }
            if (CardFiveStarRoom.Ins._RoomState == RoomStateType.AwaitPerson)
            {
                Game.Scene.GetComponent<ToyGameComponent>().EndGame();
            }
            Hide();
        }
        public override void OnShow()
        {
            base.OnShow();
            mMusicSlider.value = _MusicSoundComponent.MusicVolume;
            mSoundSlider.value = _MusicSoundComponent.SoundVolume;
            if (CardFiveStarRoom.Ins._RoomType == RoomType.RoomCard)
            {
                if (CardFiveStarRoom.Ins._ServerSeatIndexInPlayer[0]._user.UserId == UserComponent.Ins.pUserId ||
                    CardFiveStarRoom.Ins._RoomState == RoomStateType.GameIn ||
                    CardFiveStarRoom.Ins._RoomState == RoomStateType.ReadyIn)
                {
                    mOutDescText.text = "解散房间";
                    return;
                }
            }
            mOutDescText.text = "退出游戏";
        }
    }
}
