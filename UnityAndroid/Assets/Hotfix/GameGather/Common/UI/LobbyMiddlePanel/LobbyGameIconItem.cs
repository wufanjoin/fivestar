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
    public class LobbyGameIconItemAwakeSystem : AwakeSystem<LobbyGameIconItem, GameObject,GameVersionsConfig>
    {
        public override void Awake(LobbyGameIconItem self, GameObject go, GameVersionsConfig data)
        {
            self.Awake(go, data,UIType.LobbyMiddlePanel);
        }
    }
    public class LobbyGameIconItem : BaseItem<GameVersionsConfig>
    {
        private GameObject mHintGo;
        private Text mNameText;
        private Text mHintText;
        private GameObject mScheduleBgGo;
        private Image mScheduleImage;
        private Text mScheduleText;
        public override void Awake(GameObject go, GameVersionsConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHintGo = rc.Get<GameObject>("HintGo");
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mHintText = rc.Get<GameObject>("HintText").GetComponent<Text>();

            mScheduleBgGo = rc.Get<GameObject>("ScheduleBgGo");
            mScheduleImage = rc.Get<GameObject>("ScheduleImage").GetComponent<Image>();
            mScheduleText = rc.Get<GameObject>("ScheduleText").GetComponent<Text>();
            InitPanel();
        }

        public  void InitPanel()
        {
            gameObject.GetComponent<Button>().Add(ClickIocn);
          //  mHintGo.SetActive(mData.IsNeedRenewal);
           // mNameText.text = mData.Name;
            if (mData.Version == 0)
            {
                mHintText.text = "下载";
            }
            else
            {
                mHintText.text = "更新";
            }
        }

        private bool isVerify = !Define.IsAsync;
        BundleDownSchedule bundleDownSchedule = new BundleDownSchedule();
        public void ClickIocn()
        {
            if (isBeDownload)
            {
                return;
            }
            if (!isVerify)
            {
                bundleDownSchedule.pFinishAndDisposeCall = DwonFinishCall;
                bundleDownSchedule.pLoserCall = DwonLoserCall;
             //   ETModel.Game.Scene.GetComponent<BundleDownloaderComponent>().StartAsync(mData.DownloadFolder, mData.VersionFile, bundleDownSchedule);
                isBeDownload = true;
                ShowSchedule();
            }
            else
            {
                EnterGame();
            }

        }

        public void EnterGame()
        {
           // Log.Debug($"开始游戏{mData.Name}");
          //  Game.Scene.GetComponent<ToyGameComponent>().StartGame(mData.Id);
        }

        public bool isBeDownload = false;
        public async void ShowSchedule()
        {
            mScheduleBgGo.SetActive(true);
            while (isBeDownload)
            {
                int scheule = bundleDownSchedule.Progress;
                Log.Debug("下载进度:" + scheule);
                mScheduleImage.fillAmount = scheule / 100.00f;
                mScheduleText.text = scheule + "%";
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
            }
        }
        //下载完成回调
        public void DwonFinishCall()
        {
           // Game.Scene.GetComponent<GameVersionsConfigComponent>().NeedRenewalAndDownDone(mData.Id);
            mHintGo.SetActive(false);
            isVerify = true;
            isBeDownload = false;
            mScheduleBgGo.SetActive(false);
            EnterGame();
        }
        //下载失败回调
        public void DwonLoserCall(string message)
        {
            Log.Error("下载失败"+ message);
            isBeDownload = false;
            mScheduleBgGo.SetActive(false);
          //  UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow($"下载{mData.Name}失败!",null, PopOptionType.Single);
        }
    }

}
