using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.AdministratorHomePanel)]
    public class AdministratorHomePanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private Text mOnlineNumberText;
        private Button mQueryBtn;
        private InputField mUserIdInputField;
        private Button mCloseBtn;
        private Button mAnewInputPaswordBtn;
        private Button mServerHeavyBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mOnlineNumberText = rc.Get<GameObject>("OnlineNumberText").GetComponent<Text>();
            mQueryBtn = rc.Get<GameObject>("QueryBtn").GetComponent<Button>();
            mUserIdInputField = rc.Get<GameObject>("UserIdInputField").GetComponent<InputField>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mAnewInputPaswordBtn = rc.Get<GameObject>("AnewInputPaswordBtn").GetComponent<Button>();
            mServerHeavyBtn = rc.Get<GameObject>("ServerHeavyBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion

        public const string AdministratorAccountKey = "AdministratorAccountKey";
        public const string AdministratorPasswordKey = "AdministratorPasswordKey";
        public void InitPanel()
        {
            Game.Scene.AddComponent<AdministratorComponent>();//添加后台管理组件

            mAnewInputPaswordBtn.Add(ShowInputAccountPasswordPanle);
            mCloseBtn.Add(ReturnLobby);
            mQueryBtn.Add(QueryBtnEvent);
            mServerHeavyBtn.Add(ServerHeavyBtnEvent);
            ReadAccountPassword();//读取账号密码
            ReadLineOnNumber();//请求在线人数

           
        }

        public void ReturnLobby()
        {
            UIComponent.GetUiView<FiveStarLobbyPanelComponent>().Show();
        }

        public async void ServerHeavyBtnEvent()
        {
            C2M_Reload c2MReload=new C2M_Reload();
            c2MReload.Account = AdministratorRequestHelp.Account;
            c2MReload.Password = AdministratorRequestHelp.Password;
            M2C_Reload m2CReload=(M2C_Reload) await SessionComponent.Instance.Call(c2MReload);
            if (!string.IsNullOrEmpty(m2CReload.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(m2CReload.Message);
                return;
            }
            UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("重载成功");
        }
        public async void QueryBtnEvent()
        {
            long queryUserId = 0;
            try
            {
                queryUserId=long.Parse(mUserIdInputField.text);
            }
            catch (Exception e)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("用户Id格式不对");
                return;
            }
            
            U2C_QueryUserInfo u2CGetOnLineNumber = (U2C_QueryUserInfo)await SessionComponent.Instance.AdministratorCall(new C2U_QueryUserInfo()
            {
                QueryUserId = queryUserId
            });
            if (!string.IsNullOrEmpty(u2CGetOnLineNumber.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetOnLineNumber.Message);
                return;
            }
            AdministratorComponent.Ins.SetExamineUser(u2CGetOnLineNumber);//设置当前查询的UserId
            UIComponent.GetUiView<UserOperatePanelComponent>().Show();//显示用户操作界面


        }
        public async void ReadLineOnNumber()
        {
            U2C_GetOnLineNumber u2CGetOnLineNumber=(U2C_GetOnLineNumber)await SessionComponent.Instance.AdministratorCall(new C2U_GetOnLineNumber());
            if (!string.IsNullOrEmpty(u2CGetOnLineNumber.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetOnLineNumber.Message);
                return;
            }
            mOnlineNumberText.text = "在线人数:" + u2CGetOnLineNumber.OnLineNumber;
        }
        //读取账号密码
        public void ReadAccountPassword()
        {
            //如果本地存有账号 密码 就直接赋值
            if (PlayerPrefs.HasKey(AdministratorAccountKey) && PlayerPrefs.HasKey(AdministratorPasswordKey))
            {
                AdministratorRequestHelp.InputAccountAndPassword(PlayerPrefs.GetString(AdministratorAccountKey), PlayerPrefs.GetString(AdministratorPasswordKey));
            }
            else
            {
                ShowInputAccountPasswordPanle();
            }
        }
        //显示输入账号密码的面板
        public void ShowInputAccountPasswordPanle()
        {
            UIComponent.GetUiView<AlterInputTextPanelComponent>()
                .ShowAlterPanel(SaveAccountPassword, ShowAlterType.Double, "输入管理密码", "账号:", "密码:");
        }

        //存储账号密码
        public void SaveAccountPassword(string account, string password)
        {
            PlayerPrefs.SetString(AdministratorAccountKey, account);
            PlayerPrefs.SetString(AdministratorPasswordKey, password);
            AdministratorRequestHelp.InputAccountAndPassword(account, password);
        }
    }
}
