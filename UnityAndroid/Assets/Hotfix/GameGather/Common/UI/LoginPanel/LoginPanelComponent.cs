using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LoginPanel)]
    public class LoginPanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码

        private Button mTouristLoginBtn;
        private Button mWeChatLoginBtn;
        private Toggle mAgreeToggle;
        private Button mLoginBtn;
        private InputField mAccountInputField;
        private GameObject mTestLoginParentGo;

        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mTouristLoginBtn = rc.Get<GameObject>("TouristLoginBtn").GetComponent<Button>();
            mWeChatLoginBtn = rc.Get<GameObject>("WeChatLoginBtn").GetComponent<Button>();
            mAgreeToggle = rc.Get<GameObject>("AgreeToggle").GetComponent<Toggle>();
            mLoginBtn = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            mAccountInputField = rc.Get<GameObject>("AccountInputField").GetComponent<InputField>();
            mTestLoginParentGo = rc.Get<GameObject>("TestLoginParentGo");
            InitPanel();
        }

        #endregion


        public void InitPanel()
        {
            if (Application.platform== RuntimePlatform.WindowsEditor|| Application.platform == RuntimePlatform.WindowsPlayer)
            {
                mTestLoginParentGo.SetActive(true);
            }
            else
            {
                mTestLoginParentGo.SetActive(ETModel.Init.IsAdministrator);
            }
            mLoginBtn.Add(LoginBtnEvent);
            mTouristLoginBtn.Add(TouristLoginBtnEvent);
            mWeChatLoginBtn.Add(WeChatLoginBtnEvent);

            if (Application.isMobilePlatform && PlayerPrefs.HasKey(GlobalConstant.LoginVoucher))
            {
                string loginVoucher = PlayerPrefs.GetString(GlobalConstant.LoginVoucher, string.Empty);
                if (!string.IsNullOrEmpty(loginVoucher))
                {
                    Game.Scene.GetComponent<KCPUseManage>().LoginAndConnect(LoginType.Voucher, loginVoucher);//如果记录有凭证 直接发送凭证登陆
                }
            }
        }

        private void WeChatLoginBtnEvent()
        {
            if (!mAgreeToggle.isOn)
            {
                ShowAgreeHint();
                return;
            }
            SdkCall.Ins.WeChatLoginAction = WeChatLogin;//微信回调
            SdkMgr.Ins.WeChatLogin();//发起微信登陆
        }

        public void WeChatLogin(string message)
        {
            KCPUseManage.Ins.LoginAndConnect(LoginType.WeChat, message);
        }
        private void TouristLoginBtnEvent()
        {
            if (!mAgreeToggle.isOn)
            {
                ShowAgreeHint();
                return;
            }
            Log.Debug("DOTO 发送游客登录协议账号");
        }

        public void ShowAgreeHint()
        {
            UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("请勾选协议");
        }

        private void LoginBtnEvent()
        {
            try
            {
                if (string.IsNullOrEmpty(mAccountInputField.text))
                {
                    mAccountInputField.text = "0";
                }
                Game.Scene.GetComponent<KCPUseManage>().LoginAndConnect(LoginType.Editor, mAccountInputField.text);
            }
            catch (Exception e)
            {
                Log.Error("登陆失败" + e);
                throw;
            }

        }
    }

}
