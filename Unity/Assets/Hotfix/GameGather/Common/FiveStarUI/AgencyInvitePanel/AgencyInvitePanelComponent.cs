using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.AgencyInvitePanel)]
    public class AgencyInvitePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mAgencyInfoItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mAgencyInfoItemGo = rc.Get<GameObject>("AgencyInfoItemGo");
            InitPanel();
        }
        #endregion

        private RepeatedField<ServiceInfo> _ServiceInfos;

        public async ETTask<RepeatedField<ServiceInfo>> GetServiceInfos()
        {
            if (_ServiceInfos == null)
            {
                L2C_GetService l2CGetService = (L2C_GetService)await SessionComponent.Instance.Call(new C2L_GetService());
                _ServiceInfos = l2CGetService.ServiceInfos;
            }
            return _ServiceInfos;
        }

        public async void InitPanel()
        {
            mCloseBtn.Add(Hide);

            RepeatedField<ServiceInfo> serviceInfos =await GetServiceInfos();
            if (serviceInfos.Count == 0)
            {
                mAgencyInfoItemGo.SetActive(false);
                return;
            }
            Transform agencyInfoParent = mAgencyInfoItemGo.transform.parent;
            for (int i = 0; i < serviceInfos.Count-1; i++)
            {
                GameObject.Instantiate(mAgencyInfoItemGo, agencyInfoParent);
            }
            for (int i = 0; i < agencyInfoParent.childCount; i++)
            {
                agencyInfoParent.GetChild(i).gameObject
                    .AddItem<CopyTextItem>().InitItem(serviceInfos[i].Type+":" + serviceInfos[i].Number,
                        serviceInfos[i].Number);
            }
        }
    }
}
