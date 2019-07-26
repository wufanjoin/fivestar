using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ShowAlterType
    {
        public const int Single = 1;
        public const int Double = 2;
    }

    [UIComponent(UIType.AlterInputTextPanel)]
    public class AlterInputTextPanelComponent :PopUpUIView
    {
        #region 脚本工具生成的代码
        private Text mTitleText;
        private InputField mAInputField;
        private Text mATitleText;
        private InputField mBInputField;
        private Text mBTitleText;
        private Button mConfirmBtn;
        private Button mCloseBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mTitleText = rc.Get<GameObject>("TitleText").GetComponent<Text>();
            mAInputField = rc.Get<GameObject>("AInputField").GetComponent<InputField>();
            mATitleText = rc.Get<GameObject>("ATitleText").GetComponent<Text>();
            mBInputField = rc.Get<GameObject>("BInputField").GetComponent<InputField>();
            mBTitleText = rc.Get<GameObject>("BTitleText").GetComponent<Text>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mConfirmBtn.Add(ConfirmBtnEvent);
            mCloseBtn.Add(Hide);
        }

        public void ConfirmBtnEvent()
        {
            _ConfirmAction?.Invoke(mAInputField.text, mBInputField.text);
            Hide();
        }
        private Action<string, string> _ConfirmAction;
        public void ShowAlterPanel(Action<string,string> confirmAction,int alterType,string title="标题",string titlea="输入A",string titleb="输入B")
        {
            Show();
            _ConfirmAction = confirmAction;
            if (alterType == ShowAlterType.Single)
            {
                mAInputField.gameObject.SetActive(true);
                mBInputField.gameObject.SetActive(false);
            }
            else
            {
                mAInputField.gameObject.SetActive(true);
                mBInputField.gameObject.SetActive(true);
            }
            mTitleText.text = title;
            mATitleText.text = titlea;
            mBTitleText.text = titleb;
        }
    }
}
