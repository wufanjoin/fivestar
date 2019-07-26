using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.ActivityLobbyPanel)]
    public class ActivityLobbyPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Button mSingleActivityBtn;
        private GameObject mSigninPanelGo;

        private long mInfoUserId = 0;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mSingleActivityBtn = rc.Get<GameObject>("SingleActivityBtn").GetComponent<Button>();
            mSigninPanelGo = rc.Get<GameObject>("SigninPanelGo");
            RegisterEvent();
        }
        #endregion

        public override void OnShow()
        {
            base.OnShow();
            if (mInfoUserId != Game.Scene.GetComponent<UserComponent>().pUserId)
            {
                InitPanel();
            }
        }


        public void RegisterEvent()
        {
            mCloseBtn.Add(Hide);
        }

        public async void InitPanel()
        {
            try
            {
                //L2C_GetActivityList g2CGetActivityList = (L2C_GetActivityList)await SessionComponent.Instance.Session.Call(new C2L_GetActivityList());
                //mSingleActivityBtn.gameObject.CopyThisNum(g2CGetActivityList.ActivityInfoList.Count);
                //Transform singleActivityBtnParent = mSingleActivityBtn.transform.parent;
                //for (int i = 0; i < g2CGetActivityList.ActivityInfoList.Count; i++)
                //{
                //    g2CGetActivityList.ActivityInfoList[i].ActivityPanel = mSigninPanelGo;
                //    ActivityListBtnItem activityListBtnItem = singleActivityBtnParent.GetChild(0).gameObject.AddItemIfHaveInit<ActivityListBtnItem, ActivityInfo>(g2CGetActivityList.ActivityInfoList[i]);
                //    if (i == 0)
                //    {
                //        ActivityListBtnItem.CurrSelectBtn = null;
                //        activityListBtnItem.SelectSelfEvnet();
                //    }
                //}
                //L2C_GetSignInAwardList g2CGetSignInAwardList = (L2C_GetSignInAwardList)await SessionComponent.Instance.Session.Call(new C2L_GetSignInAwardList());
                //ComponentFactory.Create<SigninActivityPanel, GameObject, L2C_GetSignInAwardList>(mSigninPanelGo, g2CGetSignInAwardList);
                //mInfoUserId = Game.Scene.GetComponent<UserComponent>().pUserId;
            }
            catch (Exception e)
            {
               Log.Error(e);
                throw;
            }
        }
    }
}
