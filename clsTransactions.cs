using System;
using System.Data;
using BankProject_DataAccessLayer;

namespace BankProject_BusinessLayer
{
    public class clsTransactionType
    {
        public int TransactionTypeID { get; set; }
        public string TransactionName { get; set; }
        private clsTransactionType(int transactionTypeID, string transactionName)
        {
            TransactionTypeID = transactionTypeID;
            TransactionName = transactionName;
        }

        public static clsTransactionType Find(int TransactionTypeID)
        {
            string TransactionName = "";
            if (clsTransactionsData.GetTransactionType(TransactionTypeID, ref TransactionName))
                return new clsTransactionType(TransactionTypeID, TransactionName);
            else
                return null;
        }
    }

    public class clsTransactions
    {
        //public enum enTransactionType { enDeposit = 0, enWithdraw = 1 };
        public int TransactionID { get; set; }
        public DateTime Date { get; set; }
        public int ClientID { get; set; }
        public clsClient Client { get; set; }
        public int ReceiverClientID { get; set; }
        public clsClient ReceiverClient { get; set; }
        public int CreatedByUserID { get; set; }
        public int Amount { get; set; }
        public int TransactionTypeID { get; set; }
        public clsTransactionType TransactionType { get; set; }

        public clsTransactions()
        {
            TransactionID = -1;
            Date = DateTime.Now;
            ClientID = -1;
            Client = null;
            ReceiverClientID = -1;
            ReceiverClient = null;
            Amount = 0;
            CreatedByUserID = -1;
            TransactionTypeID = -1;
            TransactionType = null;
        }

        private clsTransactions(int transactionID, DateTime date, int clientID, int receiverClientID, int amount, int createdByUserID , int transactionTypeID)
        {
            TransactionID = transactionID;
            Date = date;
            ClientID = clientID;
            Client = clsClient.Find(ClientID);
            ReceiverClientID = receiverClientID;
            ReceiverClient = clsClient.Find(ReceiverClientID);
            Amount = amount;
            CreatedByUserID = createdByUserID;
            TransactionTypeID = transactionTypeID;
            TransactionType = clsTransactionType.Find(TransactionTypeID);
        }

        private bool AddNewTransaction()
        {
            this.TransactionID = clsTransactionsData.AddNewTransaction(this.Date , this.ClientID , this.ReceiverClientID , this.Amount , this.CreatedByUserID , this.TransactionTypeID);
            this.Client = clsClient.Find(ClientID);
            this.ReceiverClient = clsClient.Find(ReceiverClientID);
            this.TransactionType = clsTransactionType.Find(TransactionTypeID);
            return (this.TransactionID != -1);
        }

        public static clsTransactions Find(int TransactionID)
        {
            DateTime Date = DateTime.Now;
            int ClientID = -1, ReceiverClientID = -1, Amount = 0, CreatedByUserID = -1, TransactionTypeID = -1;

            if (clsTransactionsData.GetTransactionInfoByID(TransactionID, ref Date, ref ClientID, ref ReceiverClientID, ref Amount, ref CreatedByUserID, ref TransactionTypeID))
            {
                return new clsTransactions(TransactionID, Date, ClientID, ReceiverClientID, Amount, CreatedByUserID, TransactionTypeID);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllTransactions()
        {
            return clsTransactionsData.GetAllTransactions();

        }

        public static DataTable GetAllClientTransactions(int ClientID)
        {
            return clsTransactionsData.GetAllClientTransactions(ClientID);

        }

        public static DataTable GetAllTransactionsWithTransactionTypeID(int TransactionTypeID)
        {
            return clsTransactionsData.GetAllTransactionsWithTransactionTypeID(TransactionTypeID);

        }

        public static DataTable GetAllCreatedUserTransactions(int UserID) 
        {
            return clsTransactionsData.GetAllCreatedUserTransactions(UserID);

        }



        public bool Save()
        {
            if (this.TransactionTypeID == 1)
            {
                Deposit(this.Client, this.Amount);
                
            }else if(this.TransactionTypeID == 2)
            {
                Withdraw(this.Client, this.Amount);
            }
            else if (this.TransactionTypeID == 3)
            {
                Transfer(this.Client, this.ReceiverClient, this.Amount);
            }
            else
            {
                return false;
            }

            if (this.AddNewTransaction())
                return true;
            else
                return false;
        }

        private bool Deposit(clsClient Client , int Amount)
        {
            if(Client != null)
            {
                Client.AccountBalance += Amount;
                if(Client.Save())
                    return true;
            }
            return false;
        }

        private bool Withdraw(clsClient Client, int Amount)
        {
            if (Client != null)
            {
                Client.AccountBalance -= Amount;
                if (Client.Save())
                    return true;
            }
            return false;
        }

        private bool Transfer(clsClient Client, clsClient ReceiverClient , int Amount)
        {
            if (Deposit(ReceiverClient, Amount))
                if (Withdraw(Client, Amount))
                    return true;
                else
                    return false;
            else
                return false;
        }
    }
}
