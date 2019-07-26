using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Day3
{
    static class SQLMethods
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["day"].ConnectionString;
        static SqlCommand cmd;
        static SqlDataReader reader;
        public static bool CheckAcc(string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int t = 0;
                try
                {
                    cmd = new SqlCommand("SELECT COUNT(*) from Accounts where UserName like @userName", connection);
                    cmd.Parameters.AddWithValue("@username", userName);
                    t = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
                if (t == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public static bool CheckAccById(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int t = 0;
                try
                {
                    cmd = new SqlCommand("SELECT COUNT(*) from Accounts where Id = @userId", connection);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    t = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
                if (t == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public static void NewAcc(string userName, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    cmd = new SqlCommand("INSERT INTO Accounts(UserName, Password, Balance) " +
        "VALUES (@userName, @password, 0)", connection);
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
        }
        public static int GetUserId(string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int userId;
                try
                {
                    cmd = new SqlCommand("SELECT Id FROM Accounts WHERE UserName like @userName", connection);
                    cmd.Parameters.AddWithValue("@username", userName);
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userId = reader.GetInt32(0);
                    }
                    else
                    {
                        Console.WriteLine("Błąd bazy danych!");
                        Console.ReadKey();
                        userId = -1;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                    userId = -1;
                }
                reader.Close();
                connection.Close();
                return userId;
            }
        }
        public static bool LogIn(string userName,string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int p = 0;
                try
                {
                    cmd = new SqlCommand("SELECT COUNT(*) from Accounts WHERE UserName like @userName AND Password like @password ", connection);
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.Parameters.AddWithValue("@password", password);
                    p = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
                if (p == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static void SqlTransaction(double Amount, string description,int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    cmd = new SqlCommand("UPDATE Accounts SET Balance += @Amount WHERE Id = @userId", connection);
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                try
                {
                    cmd = new SqlCommand("INSERT INTO Transactions(UserId, Amount, Date, Description) " +
                "VALUES (@userId, @amount, @date, @description)", connection);
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
        }
        public static double GetBalance(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                double Balance = 0;
                try
                {
                    cmd = new SqlCommand("Select Balance from Accounts WHERE Id = @userId", connection);
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Balance = reader.GetDouble(0);
                    }
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
                return Balance;
            }
        }
        public static List<Transaction> GetTransactions(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                List<Transaction> transactions = new List<Transaction>();
                try
                {
                    cmd = new SqlCommand("Select * from Transactions WHERE UserId = @userId", connection);
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction(reader.GetInt32(0), reader.GetDouble(2), reader.GetDateTime(3), reader.GetString(4)));
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                reader.Close();
                connection.Close();
                return transactions;
            }
        }
        public static void TransferCash(int fromUserId, int toUserId, double Amount, string fDescription, string tDescription)
        {
            SqlTransaction(-Amount, fDescription, fromUserId);
            SqlTransaction(Amount, tDescription, toUserId);
        }
    }
}
