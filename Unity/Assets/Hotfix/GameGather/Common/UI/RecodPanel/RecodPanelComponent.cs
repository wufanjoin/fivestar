using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.RecodPanel)]
    public class RecodPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject mNormalGo;
        private GameObject mCancelGo;
        private GameObject mVolumeMaskGo;
        private Image mResidueTimeImage;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mNormalGo = rc.Get<GameObject>("NormalGo");
            mCancelGo = rc.Get<GameObject>("CancelGo");
            mVolumeMaskGo = rc.Get<GameObject>("VolumeMaskGo");
            mResidueTimeImage = rc.Get<GameObject>("ResidueTimeImage").GetComponent<Image>();
            InitPanel();
        }
        #endregion

        private RectTransform _MaskRecTrm;
        public void InitPanel()
        {
            _MaskRecTrm = mVolumeMaskGo.GetComponent<RectTransform>();
            fillMinus = 1.000f / (SpeexRecordMgr.Ins.SpeakMaxTime * 10);
        }

        private Vector2 _maskSizeData=new Vector2(39,0);
        private const int _maxHeight = 130;
        public async void VolumeBounce()
        {
            while (pViewState== ViewState.Show)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                _maskSizeData.y += 20;
                if (_maskSizeData.y > _maxHeight)
                {
                    _maskSizeData.y = 0;
                }
                _MaskRecTrm.sizeDelta = _maskSizeData;
            }
           
        }
        public void ShowNomalPanel()
        {
            Show();
            mNormalGo.SetActive(true);
            mCancelGo.SetActive(false);
        }
        public void ShowCancelPanel()
        {
            Show();
            mNormalGo.SetActive(false);
            mCancelGo.SetActive(true);
        }

        private float fillMinus;
        public override async void OnShow()
        {
            base.OnShow();
            VolumeBounce();
            mResidueTimeImage.fillAmount = 1;
            while (gameObject.activeInHierarchy)
            {
                mResidueTimeImage.fillAmount -= fillMinus;
                if (mResidueTimeImage.fillAmount <= 0)
                {
                    Hide();
                }
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
            }
        }
    }
}
