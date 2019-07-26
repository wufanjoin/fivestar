using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class AdministratorHelp
    {
        public const string Account = "wufan";
        public const string Password = "123";

        public static bool VerifyAdministrator(IAdministratorRequest iAdministratorRequest)
        {
            if (iAdministratorRequest.Account.Equals(Account) && iAdministratorRequest.Password.Equals(Password))
            {
                return true;
            }
            return false;
        }
    }
}
