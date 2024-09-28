using System;
using System.Collections.Generic;
using System.Data;
using BankProject_DataAccessLayer;
using static BankProject_BusinessLayer.clsGlobal;

namespace BankProject_BusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public int Permissions { set; get; }

        public clsPerson Person { set; get; }


        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            Permissions = 0;
            Person = new clsPerson();
        }

        private clsUser(int userid, int personid, string username , string password, int permissions)
        {
            UserID = userid;
            PersonID = personid;
            UserName = username;
            Password = password;
            Permissions = permissions;
            Person = clsPerson.Find(PersonID);
        }





        private bool _AddNewUser()
        {
            this.UserID = UserData.AddNewUser(this.PersonID, this.UserName, this.Password , this.Permissions);

            return (this.UserID != -1);
        }


        private bool _UpdateUser()
        {
            return UserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.Permissions);

        }

        public static clsUser Find(int UserID)
        {
            int PersonID = -1, Permissions = 0;
            string UserName = "", Password = "";

            if (UserData.GetUserInfoByUserID(UserID , ref PersonID, ref UserName , ref Password , ref Permissions))
            {
                return new clsUser(UserID, PersonID, UserName, Password, Permissions);
            }
            else
            {
                return null;
            }

        }

        public static clsUser Find(string UserName , string Password)
        {
            int PersonID = -1, Permissions = 0, UserID = -1;

            if (UserData.GetUserInfoByUserNameAndPassword(UserName, Password, ref UserID, ref PersonID, ref Permissions))
            {
                return new clsUser(UserID, PersonID, UserName, Password, Permissions);
            }
            else
            {
                return null;
            }

        }

        public static clsUser FindUserByPersonID(int PersonID)
        {
            int UserID = -1, Permissions = 0;
            string UserName = "", Password = "";

            if (UserData.GetUserInfoByPersonID(PersonID, ref UserID , ref UserName, ref Password, ref Permissions))
            {
                return new clsUser(UserID, PersonID, UserName, Password, Permissions);
            }
            else
            {
                return null;
            }

        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }


        public static DataTable GetAllUsers()
        {
            return UserData.GetAllUsers();

        }

        public static bool DeleteUser(int ID)
        {
            return UserData.DeleteUser(ID);
        }

        public static bool IsUserExist(int ID)
        {
            return UserData.IsUserExist(ID);
        }

        public static bool IsUserExist(string UserName, string Password)
        {
            return UserData.IsUserExist(UserName, Password);
        }


        //---------------------------------------------------------------------------



        public static string GetAllPermissionsString(int Permission)
        {
            string[] strPer = { "List Clients", "Add New Client", "Delete Client", "Update Clients", "Find Client", "Tranactions", "Login Register", "Manage Users" };
            int[] intPer = { 1, 2, 4, 8, 16, 32, 64, 128 };

            string strPermissions = "";
            string Separetor = " - ";
            if (Permission == -1)
            {
                strPermissions = "Have Full Permissions\nto Acecces All Screens.";
                return strPermissions;
            }
            else
            {
                for (int i = 0; i > intPer.Length; i++)
                {
                    if (i == 3)
                        strPermissions += "\n";


                    if ((intPer[i] & Permission) == intPer[i])
                    {
                        if (strPermissions != "")
                            strPermissions += Separetor;

                        strPermissions += strPer[i];
                    }
                }
            }


            /*
            if ((1 & Permission) == 1)
            {
                strPermissions += "List Clients";
            }

            if ((2 & Permission) == 2)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Add New Client";
            }

            if ((4 & Permission) == 4)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Delete Client";
            }

            if ((8 & Permission) == 8)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Update Clients";
            }

            if ((16 & Permission) == 16)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Find Client";
            }

            if ((32 & Permission) == 32)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Tranactions";
            }

            if ((64 & Permission) == 64)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Login Register";
            }

            if ((128 & Permission) == 128)
            {
                if (strPermissions != "")
                    strPermissions += Separetor;

                strPermissions += "Manage Users";
            }
            */

            return strPermissions;
        }

    }
}
