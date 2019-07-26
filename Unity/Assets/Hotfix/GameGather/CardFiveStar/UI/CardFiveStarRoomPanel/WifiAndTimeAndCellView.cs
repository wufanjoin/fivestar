using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class WifiAndTimeAndCellGoView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mWifiGo;
        private GameObject mMobileNetworkGo;
        private GameObject mNoneNetworkGo;
        private Text mTimeText;
        private Image mCellImage;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mWifiGo = rc.Get<GameObject>("WifiGo");
            mMobileNetworkGo = rc.Get<GameObject>("MobileNetworkGo");
            mNoneNetworkGo = rc.Get<GameObject>("NoneNetworkGo");
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mCellImage = rc.Get<GameObject>("CellImage").GetComponent<Image>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            
        }

        private bool isRenewalIn = false;
        public async void RenewalInfo()
        {
            if (isRenewalIn)
            {
                return;
            }
            isRenewalIn = true;
            while (gameObject.activeInHierarchy)
            {
                int netwrokType = SdkMgr.Ins.GetNetworkInfo();
                mWifiGo.SetActive(false);
                mMobileNetworkGo.SetActive(false);
                mNoneNetworkGo.SetActive(false);
                switch (netwrokType)
                {
                    case NetworkType.None:
                        mNoneNetworkGo.SetActive(true);
                        break;
                    case NetworkType.Mobile:
                        mMobileNetworkGo.SetActive(true);
                        break;
                    case NetworkType.Wifi:
                        mWifiGo.SetActive(true);
                        break;
                }
                Log.Debug("当前电量:"+ SdkMgr.Ins.GetBatteryElectric());
                mCellImage.fillAmount = SdkMgr.Ins.GetBatteryElectric() / 100.00f;

                DateTime nowTime = TimeTool.GetCurrenDateTime();
                mTimeText.text = nowTime.Hour + ":" + nowTime.Minute;
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(10000);
            }
            isRenewalIn = false;
        }
    }
}
