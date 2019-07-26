using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class AnnouncementView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mRobidGambleGo;
        private GameObject mNormalAnnouncementGo;
        private GameObject mAnnouncementItemGo;
        private Text mNormalAnnouncementContentText;
        private Text mNormalAnnouncementTitleText;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mRobidGambleGo = rc.Get<GameObject>("RobidGambleGo");
            mNormalAnnouncementGo = rc.Get<GameObject>("NormalAnnouncementGo");
            mAnnouncementItemGo = rc.Get<GameObject>("AnnouncementItemGo");
            mNormalAnnouncementTitleText = rc.Get<GameObject>("NormalAnnouncementTitleText").GetComponent<Text>();
            mNormalAnnouncementContentText = rc.Get<GameObject>("NormalAnnouncementContentText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            InitAnnouncementList();
        }

        private List<AnnouncementConfig> _AnnouncementConfigs=new List<AnnouncementConfig>();
        public async void InitAnnouncementList()
        {
            IConfig[] iConfigs=Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(AnnouncementConfig));
            for (int i = 0; i < iConfigs.Length; i++)
            {
                _AnnouncementConfigs.Add(iConfigs[i] as AnnouncementConfig);
            }
            Transform announcemetToggleParent = mAnnouncementItemGo.transform.parent;
            announcemetToggleParent.CreatorChildCount(_AnnouncementConfigs.Count);
            for (int i = 0; i < announcemetToggleParent.childCount; i++)
            {
                AnnouncementBtnItem announcementBtnItem=announcemetToggleParent.GetChild(i)
                    .AddItemIfHaveInit<AnnouncementBtnItem, AnnouncementConfig>(_AnnouncementConfigs[i]);
                if (i == announcemetToggleParent.childCount - 1)
                {
                    await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(200);
                    announcementBtnItem._Toggle.isOn = true;
                }
               
            }
        }

        public void ShowAnnouncement(AnnouncementConfig announcementConfig)
        {
            mNormalAnnouncementTitleText.text = announcementConfig.Title;
            mNormalAnnouncementContentText.text = announcementConfig.Content;
            mNormalAnnouncementGo.SetActive(AnnouncementConfig.NormalAnnouncementType == announcementConfig.Type);
            mRobidGambleGo.SetActive(AnnouncementConfig.ForbidGambleType == announcementConfig.Type); 
        }
    }
}
