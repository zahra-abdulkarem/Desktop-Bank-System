using System;
using System.Data;
using System.Data.SqlClient;

namespace BankProject_DataAccessLayer
{
    public class clsCurrenciesData
    {
        public static bool GetCurrenciesInfoByID(int CurrencyID, ref string CountryName, ref string CurrencyCode, ref string CurrencyName, ref double Rate)
        {
            bool isFound = false;
            SqlConnection connaction = new SqlConnection(clsDataSettings.ConnactionString);
            string query = "SELECT * FROM Currencies WHERE CurrencyID = @CurrencyID";
            SqlCommand command = new SqlCommand(query, connaction);
            command.Parameters.AddWithValue("@CurrencyID", CurrencyID);

            try
            {
                connaction.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    CountryName = (string)reader["CountryName"];
                    CurrencyCode = (string)reader["CurrencyCode"];
                    CurrencyName = (string)reader["CurrencyName"];
                    Rate = (float)reader["Rate"];

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

        public static int AddNewCurrency(string CountryName, string CurrencyCode, string CurrencyName, double Rate)
        {
            int CurrencyID = -1;
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"INSERT INTO Currencies (CountryName,CurrencyCode,CurrencyName,Rate)
                             VALUES (@CountryName,@CurrencyCode,@CurrencyName,@Rate);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);
            command.Parameters.AddWithValue("@CurrencyCode", CurrencyCode);
            command.Parameters.AddWithValue("@CurrencyName", CurrencyName);
            command.Parameters.AddWithValue("@Rate", Rate);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    CurrencyID = insertedID;
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


            return CurrencyID;
        }

        public static DataTable GetAllCurrencies()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataSettings.ConnactionString);

            string query = @"SELECT * FROM Currencies";


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

    }
}
