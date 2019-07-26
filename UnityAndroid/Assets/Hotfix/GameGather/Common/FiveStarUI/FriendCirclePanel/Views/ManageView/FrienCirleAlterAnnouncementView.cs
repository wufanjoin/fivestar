using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FrienCirleAlterAnnouncementView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Button mConfirmBtn;
        private InputField mContentInputField;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mContentInputField = rc.Get<GameObject>("ContentInputField").GetComponent<InputField>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mContentInputField.text = FrienCircleComponet.Ins.CuurSelectFriendsCircle.Announcement;
            mConfirmBtn.Add(ConfirmBtnEvent);
        }

        public async void ConfirmBtnEvent()
        {
            if (mContentInputField.text.Length == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("内容不能为空");
                return;
            }

            if (mContentInputField.text.Length >15)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("内容不能超过15个字符");
                return;
            }
            F2C_AlterAnnouncement f2CAlterAnnouncement=(F2C_AlterAnnouncement)await SessionComponent.Instance.Call(new C2F_AlterAnnouncement()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId,
                Announcement = mContentInputField .text
            });
            if (string.IsNullOrEmpty(f2CAlterAnnouncement.Message))
            {
                FrienCircleComponet.Ins.CuurSelectFriendsCircle.Announcement = mContentInputField.text;
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("修改成功");
                Hide();
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(f2CAlterAnnouncement.Message);
            }
        }
    }
}
