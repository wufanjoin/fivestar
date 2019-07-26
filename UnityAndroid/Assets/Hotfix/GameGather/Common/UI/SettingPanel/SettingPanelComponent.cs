using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.SettingPanel)]
    public class SettingPanelComponent:PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mExitBtn;
        private Button mCloseBtn;
        private Toggle mMusicToggle;
        private Toggle mSoundToggle;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mExitBtn=rc.Get<GameObject>("ExitBtn").GetComponent<Button>();
            mCloseBtn=rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mMusicToggle=rc.Get<GameObject>("MusicToggle").GetComponent<Toggle>();
            mSoundToggle=rc.Get<GameObject>("SoundToggle").GetComponent<Toggle>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mMusicToggle.Add(MusicToggleEvent);
            mSoundToggle.Add(SoundToggleEvent);
            mExitBtn.Add(ExitBtnEvnet);
            mCloseBtn.Add(Hide);
        }

        public void ExitBtnEvnet()
        {
            Hide();
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
        }
        public void MusicToggleEvent(bool bol)
        {
            MusicSoundComponent.Ins.MusicMuteSwitch(!bol);
        }
        public void SoundToggleEvent(bool bol)
        {
            MusicSoundComponent.Ins.SoundMuteSwitch(!bol);
        }
    }
}
