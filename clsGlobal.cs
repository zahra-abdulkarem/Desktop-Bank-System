using System;
using BankProject_BusinessLayer;

namespace BankProject_BusinessLayer
{
    public class clsGlobal
    {
        public enum enPermissions
        {
            pAll = -1, pListClients = 1, pAddNewClient = 2, pDeleteClient = 4,
            pUpdateClients = 8, pFindClient = 16, pTranactions = 32, pLoginRegister = 64, pManageUsers = 128
        };




        public static clsUser CurrentUser;

        public static void SetCurrentUser(string UserName , string Password)
        {
            if (clsUser.IsUserExist(UserName , Password))
            {
                CurrentUser = clsUser.Find(UserName, Password);
            }
        }

        public static bool CheckAccessPermission(enPermissions Permission)
        {
            int intPermission = Convert.ToInt32(Permission);

            if (CurrentUser.Permissions == Convert.ToInt32(enPermissions.pAll))
                return true;

            if ((intPermission & CurrentUser.Permissions) == intPermission)
                return true;
            else
                return false;
        }

        
    }
}
