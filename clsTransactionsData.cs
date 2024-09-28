using System;
using System.Data;
using System.Data.SqlClient;

namespace BankProject_DataAccessLayer
{
    public class clsTransactionsData
    {
        public static bool GetTransactionInfoByID(int TransactionID, ref DateTime Date, ref int ClientID, ref int ReceiverClientID, ref int Amount, ref int CreatedByUserID, ref int TransactionTypeID)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM Transactions WHERE TransactionID = @TransactionID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@TransactionID", TransactionID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    
                    Date = (DateTime)reader["Date"];
                    ClientID = (int)reader["ClientID"];

                    if (reader["ReceiverClientID"] != DBNull.Value)
                    {
                        ReceiverClientID = (int)reader["ReceiverClientID"];
                    }
                    else
                    {
                        ReceiverClientID = -1;
                    }
                    


                    Amount = (int)reader["Amount"];
                    CreatedByUserID = (int)reader["CreateByUserID"];
                    TransactionTypeID = (int)reader["TransactionTypeID"];

                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connaction.Close();
            }
            return isFound;
        }

        public static int AddNewTransaction(DateTime Date, int ClientID, int ReceiverClientID, int Amount, int CreatedByUserID, int TransactionTypeID)
        {
            int TransactionID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"INSERT INTO Transactions (Date,ClientID,ReceiverClientID,Amount,CreateByUserID,TransactionTypeID)
                             VALUES (@Date,@ClientID,@ReceiverClientID,@Amount,@CreateByUserID,@TransactionTypeID);
                             SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@ClientID", ClientID);

            if (ReceiverClientID != -1 && ReceiverClientID != null)
                command.Parameters.AddWithValue("@ReceiverClientID", ReceiverClientID);
            else
                command.Parameters.AddWithValue("@ReceiverClientID", System.DBNull.Value);

            command.Parameters.AddWithValue("@Amount", Amount);
            command.Parameters.AddWithValue("@CreateByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@TransactionTypeID", TransactionTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TransactionID = insertedID;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex);
            }

            finally
            {
                connection.Close();
            }


            return TransactionID;
        }

        public static bool DeleteTransaction(int TransactionID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"Delete Transactions
                                where TransactionID = @TransactionID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TransactionID", TransactionID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static DataTable GetAllTransactions()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT * FROM Transactions";


            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static DataTable GetAllTransactionsWithTransactionTypeID(int TransactionTypeID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT * FROM Transactions WHERE TransactionTypeID = @TransactionTypeID";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TransactionTypeID", TransactionTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static DataTable GetAllClientTransactions(int ClientID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT * FROM Transactions WHERE ClientID = @ClientID";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClientID", ClientID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static DataTable GetAllCreatedUserTransactions(int UserID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT * FROM Transactions WHERE CreateByUserID = @UserID";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static bool GetTransactionType(int TransactionTypeID, ref string TransactionName)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM TransactionTypes WHERE TransactionTypeID = @TransactionTypeID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@TransactionTypeID", TransactionTypeID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TransactionName = (string)reader["TransactionName"];
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connaction.Close();
            }
            return isFound;
        }
    }
}
