using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class PopOptionType
    {
        public const string Single = "Single";
        public const string Double = "Double";
    }


    [UIComponent(UIType.PopUpHintPanel)]
    public class PopUpHintPanelComponent:PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mNoBtn;
        private Button mYesBtn;
        private Button mConfirmBtn;
        private Text mContentText;
        private Text mNoNameText;
        private Text mYesNameText;
        private Text mConfirmNameText;
        
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mNoBtn=rc.Get<GameObject>("NoBtn").GetComponent<Button>();
            mYesBtn=rc.Get<GameObject>("YesBtn").GetComponent<Button>();
            mConfirmBtn=rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mContentText=rc.Get<GameObject>("ContentText").GetComponent<Text>();
            mNoNameText=rc.Get<GameObject>("NoNameText").GetComponent<Text>();
            mYesNameText=rc.Get<GameObject>("YesNameText").GetComponent<Text>();
            mConfirmNameText=rc.Get<GameObject>("ConfirmNameText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mNoBtn.Add(NoBtnEvent);
            mYesBtn.Add(YesBtnEvent);
            mConfirmBtn.Add(YesBtnEvent);
        }

        public void NoBtnEvent()
        {
            ClickPopOption(false);
        }
        public void YesBtnEvent()
        {
            ClickPopOption(true);
        }

        public void ClickPopOption(bool option)
        {
            this.Hide();
            if (mBtnClickCall != null)
            {
                mBtnClickCall(option);
            }
        }
        private Action<bool> mBtnClickCall;
        public void ShowOptionWindow(string content, Action<bool> call, string optionType = PopOptionType.Double, string yesName = "确 认", string noName = "取 消")
        {
            Show();
            mContentText.text = content;
            mConfirmNameText.text = yesName;
            mNoNameText.text = noName;
            mYesNameText.text = yesName;
            mBtnClickCall = call;
            ChangOptionType(optionType);
        }

      

        public  void ChangOptionType(string optionType)
        {
            switch (optionType)
            {
                case PopOptionType.Single:
                    mNoBtn.gameObject.SetActive(false);
                    mYesBtn.gameObject.SetActive(false);
                    mConfirmBtn.gameObject.SetActive(true);
                    break;
                case PopOptionType.Double:
                    mNoBtn.gameObject.SetActive(true);
                    mYesBtn.gameObject.SetActive(true);
                    mConfirmBtn.gameObject.SetActive(false);
                    break;
                default:
                    Log.Error("显示弹窗类型错误type："+optionType);
                    break;
            }
        }

        public override void MaskClickEvent()
        {
            
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}
