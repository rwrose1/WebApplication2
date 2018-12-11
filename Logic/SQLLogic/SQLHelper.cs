using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using NazdaqSearch.Models;
using System.Text;
using System.Security.Cryptography;

namespace NazdaqSearch.Logic.SQLLogic
{
    public static class SQLHelper
    {

        private static string INSERTUSER = "dbo.UserInsert";
        private static string INSERTDATA = "dbo.IntoNazdata";
        private static string CONNECTIONSTRING = CONNECTIONSTRING = @"Server=tcp:nazdaq-db-server.database.windows.net,1433;Initial Catalog=NazdaqSearchDB;Persist Security Info=False;User ID=rwrose56;Password=RigintheHouse!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        public static List<NDData> CompareData(List<NDData> symbols, String date)
        {

            List<NDData> returnList = new List<NDData>();

            foreach (NDData item in symbols)
            {
                if (item.TimeandDate.Contains(date))
                {
                    returnList.Add(item);
                }
            }

            return returnList;
        }

        public static void Insert(NDData item, userModel user)
        {

            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRING))
            using (SqlCommand cmd = new SqlCommand(INSERTDATA, cnn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title", item.Title);
                cmd.Parameters.AddWithValue("@Date", item.TimeandDate);
                cmd.Parameters.AddWithValue("@Symbol", item.Symbol);
                cmd.Parameters.AddWithValue("@Data", item.Data);
                cmd.Parameters.AddWithValue("@UserID", user.ID);

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public static List<NDData> GetAllWithSymbol(String symbol, userModel user)
        {

            List<NDData> data = new List<NDData>();

            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRING))
            using (SqlCommand cmd = new SqlCommand("select * from NazData", cnn)) 
            {
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        String test = reader["Symbol"].ToString();
                        int idTest = int.Parse(reader["UserID"].ToString());
                        if (test == symbol && idTest == user.ID)
                        {
                            NDData entry = new NDData();
                            entry.Symbol = test;
                            entry.Title = reader["Title"].ToString();
                            entry.Data = reader["DataText"].ToString();
                            entry.TimeandDate = reader["DateandTime"].ToString();

                            data.Add(entry);
                        }
                    }

                }
                cnn.Close();
            }

            return data;
        }

        public static List<NDData> GetAllWithDate(String Date, userModel user)
        {

            List<NDData> data = new List<NDData>();

            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRING))
            using (SqlCommand cmd = new SqlCommand("select * from NazData", cnn))
            {
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        String test = reader["DateandTime"].ToString();
                        int testId = int.Parse(reader["UserID"].ToString());
                        if (test.Contains(Date.Trim()) && testId == user.ID)
                        {
                            NDData entry = new NDData();
                            entry.TimeandDate = test;
                            entry.Title = reader["Title"].ToString();
                            entry.Data = reader["DataText"].ToString();
                            entry.Symbol = reader["Symbol"].ToString();

                            data.Add(entry);
                        }
                    }

                }
                cnn.Close();
            }

            return data;
        }

        public static Boolean AddUser(userModel item)
        {

            //userModel check = ValidateUser(item);

            //if (check == null) return false;

            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRING))
            using (SqlCommand cmd = new SqlCommand(INSERTUSER, cnn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User", item.UserID);
                cmd.Parameters.AddWithValue("@UPassword", Crypt(item.Password));

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }

            return true;

        }



        public static userModel ValidateUser(userModel item)
        {

            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRING))
            using (SqlCommand cmd = new SqlCommand("Select * from Users", cnn))
            {
                cnn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        String user = reader["UserID"].ToString();
                        String password = Derypt(reader["UPassword"].ToString());
                        if (user.Equals(item.UserID) && password.Equals(item.Password))
                        {
                            item.ID = int.Parse(reader["ID"].ToString());
                            return item;
                        }

                    }
                }
                cnn.Close();
            }

            return null;
        }

        public static string Crypt(string text)
        {
            byte [] entropy = { 9, 8, 7, 6, 5 };

            //String convert = Convert.ToBase64String(
            //    ProtectedData.Protect(Encoding.Unicode.GetBytes(text), entropy, DataProtectionScope.CurrentUser));
            return text;
        } 

        public static string Derypt(string text)
        {
            byte [] entropy = { 9, 8, 7, 6, 5 };

            //String convert = Encoding.Unicode.GetString(
            //    ProtectedData.Unprotect(Convert.FromBase64String(text), entropy, DataProtectionScope.CurrentUser));
            return text;
        }

    }
}