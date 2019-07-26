using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.AnnouncementPanel)]
    public class AnnouncementPanelComponent:PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Text mContentText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn=rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mContentText=rc.Get<GameObject>("ContentText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
        }

        public void ChangAnnouncementContent(string content)
        {
            Show();
            mContentText.text = content;
        }
    }
}
