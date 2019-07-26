using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class SignInActivityView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mSignInAwardItemGo;
        private GameObject mSeventhDayAwardGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mSignInAwardItemGo = rc.Get<GameObject>("SignInAwardItemGo");
            mSeventhDayAwardGo = rc.Get<GameObject>("SeventhDayAwardGo");
            InitPanel();
        }
        #endregion

        public static int _SingInDays = 0;
        public async void InitPanel()
        {
            Hide();
            L2C_GetSignInAwardList  l2CGetSignInAwardList= (L2C_GetSignInAwardList)await SessionComponent.Instance.Call(new C2L_GetSignInAwardList());
            Show();
            bool isToDaySingIn = false;
            if (l2CGetSignInAwardList.UserSinginInfo == null)
            {
                l2CGetSignInAwardList.UserSinginInfo=new UserSingInState();
            }
            else
            {
                isToDaySingIn = TimeTool.TimeStampIsToday(l2CGetSignInAwardList.UserSinginInfo.SingInTime);//今天是否签到
            }
            if (!isToDaySingIn)
            {
                L2C_TodaySignIn l2CTodaySign = (L2C_TodaySignIn)await SessionComponent.Instance.Call(new C2L_TodaySignIn());
                if (l2CTodaySign.Error == 0)
                {
                    l2CGetSignInAwardList.UserSinginInfo.SingInDays++;
                }
            }
            _SingInDays = l2CGetSignInAwardList.UserSinginInfo.SingInDays;

            Transform sininAwarParent=mSignInAwardItemGo.transform.parent;
            sininAwarParent.CreatorChildCount(l2CGetSignInAwardList.SignInAwardList.Count-1);
            for (int i = 0; i < l2CGetSignInAwardList.SignInAwardList.Count-1; i++)
            {
                sininAwarParent.GetChild(i).AddItemIfHaveInit<GetSignInAwardItem, SignInAward>(l2CGetSignInAwardList.SignInAwardList[i]);
            }
            mSeventhDayAwardGo.AddItemIfHaveInit<GetSignInAwardItem, SignInAward>(l2CGetSignInAwardList.SignInAwardList[l2CGetSignInAwardList.SignInAwardList.Count-1]);


           
        }
    }
}
