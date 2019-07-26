using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.AgencyOperatePanel)]
    public class AgencyOperatePanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Text mAgencyLvText;
        private InputField mAlterInputField;
        private Button mConfirmBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mAgencyLvText = rc.Get<GameObject>("AgencyLvText").GetComponent<Text>();
            mAlterInputField = rc.Get<GameObject>("AlterInputField").GetComponent<InputField>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
        }
    }
}
