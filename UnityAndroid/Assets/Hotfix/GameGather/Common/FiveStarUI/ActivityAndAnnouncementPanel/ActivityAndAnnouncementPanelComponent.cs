using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ActivityAndAnnouncementType
    {
        public const int Activity = 1;
        public const int Announcement = 2;
    }
    [UIComponent(UIType.ActivityAndAnnouncementPanel)]
    public class ActivityAndAnnouncementPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Toggle mActivityToggle;
        private Toggle mAnnouncementToggle;
        private GameObject mAnnouncementViewGo;
        private GameObject mActivityViewGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mActivityToggle = rc.Get<GameObject>("ActivityToggle").GetComponent<Toggle>();
            mAnnouncementToggle = rc.Get<GameObject>("AnnouncementToggle").GetComponent<Toggle>();
            mAnnouncementViewGo = rc.Get<GameObject>("AnnouncementViewGo");
            mActivityViewGo = rc.Get<GameObject>("ActivityViewGo");
            InitPanel();
        }
        #endregion

        public ActivityView _ActivityView;
        public AnnouncementView _AnnouncementView;
        public void InitPanel()
        {
            _ActivityView = mActivityViewGo.AddItem<ActivityView>();
            _AnnouncementView = mAnnouncementViewGo.AddItem<AnnouncementView>();

            mCloseBtn.Add(Hide);
            mActivityToggle.Add(ActivityToggleEvent);
            mAnnouncementToggle.Add(AnnouncementToggleEvent);
        }

        public void ShowPanel(int type)
        {
            Show();
            if (ActivityAndAnnouncementType.Activity == type)
            {
                mActivityToggle.isOn = true;
            }
            else
            {
                mAnnouncementToggle.isOn = true;
            }
        }
        //显示签到活动
        public void ShowSignInActivity()
        {
            ShowPanel(ActivityAndAnnouncementType.Activity);
            _ActivityView.SignInActivityToggleEvent(true);
        }
        public void ActivityToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _ActivityView.Show();
                _AnnouncementView.Hide();
            }
        }
        public void AnnouncementToggleEvent(bool isOn)
        {
            if (isOn)
            {
                _AnnouncementView.Show();
                _ActivityView.Hide();
            }
        }
    }
}
