using System;
using System.Data;
using System.Data.SqlClient;

namespace BankProject_DataAccessLayer
{
    public class clsLoginRegisterData
    {
        public static bool GetLoginInfoByID(int LoginID , ref DateTime LoginDate , ref int UserID)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "select * from LoginRegister WHERE LoginID = @LoginID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@LoginID", LoginID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LoginDate = (DateTime)reader["LoginDate"];
                    UserID = (int)reader["UserID"];
                    

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


        public static int AddNewLoginRegister(DateTime LoginDate, int LoginUserID)
        {
            int LoginID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"INSERT INTO LoginRegister (LoginDate,LoginUserID)
                             VALUES (@LoginDate, @LoginUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LoginDate", LoginDate);
            command.Parameters.AddWithValue("@LoginUserID", LoginUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LoginID = insertedID;
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


            return LoginID;
        }

        public static DataTable GetAllLoginRegister()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"select * from LoginRegister";


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


        public static bool IsLoginRegisterExist(int LoginID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = "select * from LoginRegister WHERE LoginID = @LoginID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LoginID", LoginID);

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
