using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class NoneFriendCircleView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mJoinBtn;
        private Button mCreatorBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mJoinBtn = rc.Get<GameObject>("JoinBtn").GetComponent<Button>();
            mCreatorBtn = rc.Get<GameObject>("CreatorBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mJoinBtn.Add(JoinBtnEvent);
            mCreatorBtn.Add(CreatorBtnEvent);
        }
        public void CreatorBtnEvent()
        {
            UIComponent.GetUiView<CreateFriendCiclePanelComponent>().Show();
        }
        public void JoinBtnEvent()
        {
            UIComponent.GetUiView<JoinFriendCiclePanelComponent>().ShowPanel(JoinFrienPanelShowType.Join);
        }
    }
}
