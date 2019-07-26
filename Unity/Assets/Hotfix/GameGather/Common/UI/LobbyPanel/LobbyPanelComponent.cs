using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LobbyPanel)]
    public class LobbyPanelComponent:NormalUIView
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
            this.InitChildView(UIType.LobbyTopPanel,UIType.LobbyMiddlePanel,UIType.LobbybottomPanel);
        }
    }
}
