using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.RulePanel)]
    public class RulePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Text mRuleText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mRuleText = rc.Get<GameObject>("RuleText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mRuleText.text=GetResoure<TextAsset>("rule").text;
        }
    }
}
