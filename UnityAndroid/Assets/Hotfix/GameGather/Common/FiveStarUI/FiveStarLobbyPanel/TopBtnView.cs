using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class TopBtnView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mMailBtn;
        private Button mAgencyMgrBtn;
        private Button mShareBtn;
        private Button mCooperationBtn;
        private Button mActivityBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mMailBtn = rc.Get<GameObject>("MailBtn").GetComponent<Button>();
            mAgencyMgrBtn = rc.Get<GameObject>("AgencyMgrBtn").GetComponent<Button>();
            mShareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mCooperationBtn = rc.Get<GameObject>("CooperationBtn").GetComponent<Button>();
            mActivityBtn = rc.Get<GameObject>("ActivityBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion

       
        public async void InitPanel()
        {
            mShareBtn.Add(ShareBtnEvent);
            mCooperationBtn.Add(CooperationBtnEvent);
            mActivityBtn.Add(ActivityBtnEvent);
            L2C_GetAgencyStatu l2CGetAgencyStatu=(L2C_GetAgencyStatu)await SessionComponent.Instance.Call(new C2L_GetAgencyStatu());
            mAgencyMgrBtn.gameObject.SetActive(l2CGetAgencyStatu.IsAgency|| ETModel.Init.IsAdministrator);
            mAgencyMgrBtn.Add(AgencyMgrBtneEvent);
        }

        public void AgencyMgrBtneEvent()
        {
            UIComponent.GetUiView<AgencyMgrPanelComponent>().Show();
            if (ETModel.Init.IsAdministrator)
            {
                //打开后台 管理界面
                UIComponent.GetUiView<AdministratorHomePanelComponent>().Show();
            }
        }
        public void ShareBtnEvent()
        {
            ShareUrlMgr.NormalShare(WxShareSceneType.Friend);
        }

        public void CooperationBtnEvent()
        {

            UIComponent.GetUiView<AgencyInvitePanelComponent>().Show();
        }

        public void ActivityBtnEvent()
        {
            UIComponent.GetUiView<ActivityAndAnnouncementPanelComponent>().ShowPanel(ActivityAndAnnouncementType.Activity);
        }
    }
}
