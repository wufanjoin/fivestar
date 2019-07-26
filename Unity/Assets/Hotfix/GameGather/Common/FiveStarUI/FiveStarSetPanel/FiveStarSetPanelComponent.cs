using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.FiveStarSetPanel)]
    public class FiveStarSetPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Slider mMusicSlider;
        private Slider mSoundSlider;
        private Button mOutGameBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mMusicSlider = rc.Get<GameObject>("MusicSlider").GetComponent<Slider>();
            mSoundSlider = rc.Get<GameObject>("SoundSlider").GetComponent<Slider>();
            mOutGameBtn = rc.Get<GameObject>("OutGameBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion

        private MusicSoundComponent _MusicSoundComponent;
        public void InitPanel()
        {
            _MusicSoundComponent = Game.Scene.GetComponent<MusicSoundComponent>();
            mCloseBtn.Add(Hide);
            mOutGameBtn.Add(OutBtnEvent);

          

            mMusicSlider.Add(MusiceSliderEvent);
            mSoundSlider.Add(SoundSliderEvent);
            
        }

        public override async void OnShow()
        {
            base.OnShow();
            mMusicSlider.value = _MusicSoundComponent.MusicVolume;
            mSoundSlider.value = _MusicSoundComponent.SoundVolume;
        }

        public void MusiceSliderEvent(float value)
        {
            _MusicSoundComponent.MusicVolume=value;
        }
        public void SoundSliderEvent(float value)
        {
            _MusicSoundComponent.SoundVolume=value;
        }
        public void OutBtnEvent()
        {
            Hide();
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
        }
    }
}
