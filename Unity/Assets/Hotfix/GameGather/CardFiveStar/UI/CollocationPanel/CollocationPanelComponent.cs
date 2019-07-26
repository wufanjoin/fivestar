using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.CollocationPanel)]
    public class CollocationPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            gameObject.GetComponent<Button>().Add(CancelCollocation,false);
        }

        public void CancelCollocation()
        {
            SessionComponent.Instance.Send(new Actor_FiveStar_CollocationChange(){IsCollocation = false});
        }
    }
}
