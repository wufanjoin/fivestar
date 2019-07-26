using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.CardFiveStarVideoPanel)]
    public class CardFiveStarVideoPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private Button mOutBtn;
        private Button mAnewStartBtn;
        private Button mPauseBtn;
        private Button mMultiplierBtn;
        private Button mPlayBtn;
        private Text mScheduleText;
        private Text mMultiplierSpeedText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mOutBtn = rc.Get<GameObject>("OutBtn").GetComponent<Button>();
            mAnewStartBtn = rc.Get<GameObject>("AnewStartBtn").GetComponent<Button>();
            mPauseBtn = rc.Get<GameObject>("PauseBtn").GetComponent<Button>();
            mMultiplierBtn = rc.Get<GameObject>("MultiplierBtn").GetComponent<Button>();
            mPlayBtn = rc.Get<GameObject>("PlayBtn").GetComponent<Button>();
            mScheduleText = rc.Get<GameObject>("ScheduleText").GetComponent<Text>();
            mMultiplierSpeedText = rc.Get<GameObject>("MultiplierSpeedText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mOutBtn.Add(OutBtnEvent);
            mAnewStartBtn.Add(AnewStartBtnEvent);
            mPauseBtn.Add(PauseBtnEvent);
            mMultiplierBtn.Add(MultiplierBtnEvent);
            mPlayBtn.Add(PlayBtnEvent);
        }

        public void SetMultiplierSpeed(int speed)
        {
            mMultiplierSpeedText.text = speed + "倍速";
        }
        private int _TotalCount;
        public void SetTotalScheduleText(int totalCount)
        {
            _TotalCount = totalCount;
            SetCurrScheduleText(0);
        }

      
        public void SetCurrScheduleText(int currCount)
        {
            mScheduleText.text = $"进度{currCount}/{_TotalCount}";
        }
        public void OutBtnEvent()
        {
            FiveStarVideoRoom.Ins.OutPlay();
        }
        public void AnewStartBtnEvent()
        {
            FiveStarVideoRoom.Ins.AnewStart();
        }
        public void PauseBtnEvent()
        {
            FiveStarVideoRoom.Ins.Pause();
            CutPauseInUI();
        }
        public void MultiplierBtnEvent()
        {
            FiveStarVideoRoom.Ins.MultiplierSpeed();
        }
        public void PlayBtnEvent()
        {
            FiveStarVideoRoom.Ins.Play();
            CutPlayInUI();
        }

        public void CutPlayInUI()
        {
            mPlayBtn.gameObject.SetActive(false);
            mPauseBtn.gameObject.SetActive(true);
        }
        public void CutPauseInUI()
        {
            mPlayBtn.gameObject.SetActive(true);
            mPauseBtn.gameObject.SetActive(false);
        }
        public override void OnShow()
        {
            base.OnShow();
            CutPlayInUI();
            SetMultiplierSpeed(1);
        }
    }
}
