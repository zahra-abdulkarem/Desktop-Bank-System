using System;
using System.Data;
using System.Data.SqlClient;

namespace BankProject_DataAccessLayer
{
    public class clsClientData
    {
        public static bool GetClientInfoByID(int ClientID, ref int PersonID, ref string AccountNumber, ref string PinCode, ref int AccountBalance, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM Clients WHERE ClientID = @ClientID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@ClientID", ClientID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    
                    PersonID = (int)reader["PersonID"];                  
                    AccountNumber = (string)reader["AccountNumber"];
                    PinCode = (string)reader["PinCode"];
                    AccountBalance = (int)reader["AccountBalance"]; // Logical Error      
                    CreatedByUserID = (int)reader["CreatedByUserID"];
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


        public static bool GetClientInfoByAccountNumber(string AccountNumber , ref int ClientID, ref int PersonID, ref string PinCode, ref int AccountBalance, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM Clients WHERE AccountNumber = @AccountNumber";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@AccountNumber", AccountNumber);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClientID = (int)reader["ClientID"];
                    PersonID = (int)reader["PersonID"];
                    AccountNumber = (string)reader["AccountNumber"];
                    PinCode = (string)reader["PinCode"];
                    AccountBalance = (int)reader["AccountBalance"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
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



        public static bool GetClientInfoByPersonID(int PersonID , ref int ClientID, ref string AccountNumber, ref string PinCode, ref int AccountBalance, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM Clients WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClientID = (int)reader["ClientID"];
                    AccountNumber = (string)reader["AccountNumber"];
                    PinCode = (string)reader["Gendar"];
                    AccountBalance = (int)reader["AccountBalance"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

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


        public static int AddNewClient(int PersonID, string AccountNumber, string PinCode, float AccountBalance, int CreatedByUserID)
        {
            int ClientID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"INSERT INTO Clients (PersonID,AccountNumber,PinCode,AccountBalance,CreatedByUserID)
                             VALUES (@PersonID,@AccountNumber,@PinCode,@AccountBalance,@CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
            command.Parameters.AddWithValue("@PinCode", PinCode);
            command.Parameters.AddWithValue("@AccountBalance", AccountBalance);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ClientID = insertedID;
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


            return ClientID;
        }


        public static bool UpdateClient(int ClientID, int PersonID, string AccountNumber, string PinCode, float AccountBalance, int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"Update  Clients
                         set PersonID = @PersonID,
                                AccountNumber = @AccountNumber,  
                                PinCode = @PinCode,
                                AccountBalance = @AccountBalance,
                                CreatedByUserID=@CreatedByUserID, 
                          WHERE ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@AccountNumber", AccountNumber);
            command.Parameters.AddWithValue("@PinCode", PinCode);
            command.Parameters.AddWithValue("@AccountBalance", AccountBalance);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static DataTable GetAllClients()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT Clients.ClientID, Clients.PersonID, People.FirstName, People.LastName,
                            People.Email, People.Phone, People.Gendar, Clients.AccountNumber, Clients.PinCode,
                            Clients.AccountBalance, Clients.CreatedByUserID
                            FROM   Clients INNER JOIN
                             People ON Clients.PersonID = People.PersonID";


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


        public static DataTable GetAllTotalBalances()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT Clients.ClientID, FullName = People.FirstName + '' + People.LastName, Clients.AccountNumber, Clients.AccountBalance
                        FROM   Clients INNER JOIN
                       People ON Clients.PersonID = People.PersonID";


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


        public static bool DeleteClient(int ClientID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"Delete Clients
                                where ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

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



        public static bool IsClientExist(int ClientID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = "SELECT Found=1 FROM Clients WHERE ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool IsClientExist(string AccountNumber)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = "SELECT Found=1 FROM Clients WHERE AccountNumber = @AccountNumber";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AccountNumber", AccountNumber);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


    }
}
