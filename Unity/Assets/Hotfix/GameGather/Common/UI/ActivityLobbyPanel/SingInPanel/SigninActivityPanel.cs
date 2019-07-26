using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class SigninActivityPanelAwakeSystem : AwakeSystem<SigninActivityPanel, GameObject, L2C_GetSignInAwardList>
    {
        public override void Awake(SigninActivityPanel self, GameObject go, L2C_GetSignInAwardList data)
        {
            self.Awake(go, data, UIType.ActivityLobbyPanel);
        }
    }
    public class SigninActivityPanel : BaseItem<L2C_GetSignInAwardList>
    {
        private Button mGetAwardBtn;

        private GameObject mDayItem;

        private GameObject mSoecialDatItem;

        public static ActivityListBtnItem CurrSelectBtn;
        public override void Awake(GameObject go, L2C_GetSignInAwardList data, string uiType)
        {
            base.Awake(go, data, uiType);
            mDayItem = rc.Get<GameObject>("DayItem");
            mSoecialDatItem = rc.Get<GameObject>("SoecialDatItem");
            mGetAwardBtn = rc.Get<GameObject>("GetAwardBtn").GetComponent<Button>();
            InitItem();
        }

        private void InitItem()
        {
            if (mData.UserSinginInfo == null)
            {
                mData.UserSinginInfo = new UserSingInState();
            }
            for (int i = 0; i < mData.SignInAwardList.count - 1; i++)
            {
                mData.SignInAwardList[i].isDoneSignIn =
                    mData.SignInAwardList[i].NumberDays <= mData.UserSinginInfo.SingInDays;
            }
            mDayItem.CopyThisNum(mData.SignInAwardList.count-1);
            Transform  mDayItemParent=mDayItem.transform.parent;
            for (int i = 0; i < mData.SignInAwardList.count-1; i++)
            {
                mDayItemParent.GetChild(i).gameObject.AddItemIfHaveInit<SignInAwardItem, SignInAward>(mData.SignInAwardList[i]);
            }
            mSoecialDatItem.AddItemIfHaveInit<SignInAwardItem, SignInAward>(
                mData.SignInAwardList[mData.SignInAwardList.count - 1]);
            if (TimeTool.TimeStampIsToday(mData.UserSinginInfo.SingInTime))
            {
                FinshSingIn();
            }
            else
            {

                mGetAwardBtn.enabled = true;
                mGetAwardBtn.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                mGetAwardBtn.Add(GetAwardBtnEvent);
            }
            
        }

        private async void GetAwardBtnEvent()
        {
            L2C_TodaySignIn g2CTodaySign =(L2C_TodaySignIn)await SessionComponent.Instance.Session.Call(new C2L_TodaySignIn());
            if (g2CTodaySign.Error==0)
            {
                EventMsgMgr.SendEvent(CommEventID.UserFinshSignIn, ++mData.UserSinginInfo.SingInDays);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(g2CTodaySign.Message);
            }
            FinshSingIn();
        }

        public void FinshSingIn()
        {
            mGetAwardBtn.enabled = false;
            mGetAwardBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }

    }
}
