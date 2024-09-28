using System;
using BankProject_DataAccessLayer;
using System.Data;

namespace BankProject_BusinessLayer
{
    public class clsClient
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ClientID { get; set; }
        public int PersonID { get; set; }
        public string AccountNumber { get; set; }
        public string PinCode { get; set; }
        public int AccountBalance { get; set; }
        public int CreatedByUserID {  get; set; }
        public clsPerson Person;

        public clsClient()
        {
            ClientID = -1;
            PersonID = -1;
            AccountNumber = "";
            PinCode = "";
            AccountBalance = 0;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        private clsClient(int clientID, int personID, string accountNumber, string pinCode, int accountBalance, int createdByUserID)
        {
            ClientID = clientID;
            PersonID = personID;
            AccountNumber = accountNumber;
            PinCode = pinCode;
            AccountBalance = accountBalance;
            CreatedByUserID = createdByUserID;
            Person = clsPerson.Find(PersonID);
            
            Mode = enMode.Update;
        }

        private bool _AddNewClient()
        {
            this.ClientID = clsClientData.AddNewClient(this.PersonID, this.AccountNumber , this.PinCode , this.AccountBalance, this.CreatedByUserID);

            return (this.ClientID != -1);
        }


        private bool _UpdateClient()
        {
            return clsClientData.UpdateClient(this.ClientID, this.PersonID, this.AccountNumber, this.PinCode, this.AccountBalance, this.CreatedByUserID);

        }

        public static clsClient Find(int _ClientID)
        {

            int _PersonID = -1, _CreatedByUserID = -1;
            string _AccountNumber = "", _PinCode = "";
            int _AccountBalance = 0;

            if (clsClientData.GetClientInfoByID(_ClientID , ref _PersonID , ref _AccountNumber , ref _PinCode , ref _AccountBalance , ref _CreatedByUserID))
            {
                return new clsClient(_ClientID, _PersonID, _AccountNumber, _PinCode, _AccountBalance, _CreatedByUserID);
            }
            else
            {
                return null;
            }

        }

        public static clsClient Find(string _AccountNumber)
        {

            int _ClientID = -1, _PersonID = -1, _CreatedByUserID = -1;
            string _PinCode = "";
            int _AccountBalance = 0;

            if (clsClientData.GetClientInfoByAccountNumber(_AccountNumber, ref _PersonID, ref _ClientID , ref _PinCode, ref _AccountBalance, ref _CreatedByUserID))
            {
                return new clsClient(_ClientID, _PersonID, _AccountNumber, _PinCode, _AccountBalance, _CreatedByUserID);
            }
            else
            {
                return null;
            }

        }


        public static clsClient FindByPersonID(int PersonID)
        {

            int ClientID = -1, CreatedByUserID = -1;
            string AccountNumber = "", PinCode = "";
            int AccountBalance = 0;

            if (clsClientData.GetClientInfoByPersonID(PersonID, ref ClientID,ref AccountNumber, ref PinCode, ref AccountBalance, ref CreatedByUserID))
            {
                return new clsClient(PersonID , ClientID, AccountNumber, PinCode, AccountBalance, CreatedByUserID);
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
                    if (_AddNewClient())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateClient();

            }

            return false;
        }


        public static DataTable GetAllClients()
        {
            return clsClientData.GetAllClients();

        }

        public static bool DeleteClient(int ID)
        {
            return clsClientData.DeleteClient(ID);
        }

        public static bool IsClientExist(int ID)
        {
            return clsClientData.IsClientExist(ID);
        }

        public static bool IsClientExist(string AccountNumber)
        {
            return clsClientData.IsClientExist(AccountNumber);
        }

        public static DataTable GetAllTotalBalances()
        {
            return clsClientData.GetAllTotalBalances();
        }

    }
}
