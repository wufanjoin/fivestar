using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class AdministratorRequestHelp
    {
        public static string Account = "";
        public static string Password = "";

        //输入账号 和密码
        public static void InputAccountAndPassword(string account,string password)
        {
            Account = account;
            Password = password;
            SessionComponent.Instance.AdministratorCall(new C2U_GetOnLineNumber());
        }

        //Call后台管理协议
        public static ETTask<IResponse> AdministratorCall(this SessionComponent sessionComponent, IAdministratorRequest administratorRequest)
        {
            administratorRequest.Account = Account;
            administratorRequest.Password = Password;
            return sessionComponent.Call(administratorRequest);
        }
    }
}
