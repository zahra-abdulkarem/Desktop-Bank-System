using System;
using System.Data;
using BankProject_DataAccessLayer;

namespace BankProject_BusinessLayer
{
    public class clsLoginRegister
    {
        public int LoginID { get; set; }
        public DateTime LoginDate { get; set; }
        public int UserID { get; set; }

        public clsUser UserInfo;

        public clsLoginRegister()
        {
            LoginID = -1;
            LoginDate = DateTime.Now;
            UserID = -1;
            
        }

        private clsLoginRegister(int loginID , DateTime loginDate , int userID)
        {
            LoginID = loginID;
            LoginDate = loginDate;
            UserID = userID;
            UserInfo = clsUser.Find(UserID);
        }


        public static DataTable GetAllLoginRegister()
        {
            return clsLoginRegisterData.GetAllLoginRegister();
        }

        public bool AddNewLoginRegister()
        {
            this.LoginID = clsLoginRegisterData.AddNewLoginRegister(this.LoginDate, this.UserID);
            if (this.LoginID != -1)
                return true;
            else
                return false;
        }
    }
}
