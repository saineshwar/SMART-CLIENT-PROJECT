using DemoSmartClient.Interface;
using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Data;

namespace DemoSmartClient.Concrete
{
    public class LoginConcrete : ILogin
    {
        /// <summary>
        /// Checking is User Exists in Database
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string CheckUserExists(string Username, string Password)
        {
            string result = string.Empty;
            DataSet ds = new DataSet();
            using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
            {
                con.SetPassword("@@#DEMOSMART#@@");
                con.Open();

                using (SQLiteCommand com = new SQLiteCommand(con))
                {
                    try
                    {
                        com.CommandText = "select CLientIDToken from logintb Where Username='" + Username + "'and Password='" + Password + "'";
                        com.CommandType = CommandType.Text;
                        SQLiteDataAdapter da = new SQLiteDataAdapter();
                        da.SelectCommand = com;
                        da.Fill(ds);
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                result = Convert.ToString(ds.Tables[0].Rows[0]["CLientIDToken"]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        ds.Dispose();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Inserting User Credentials into Database 
        /// </summary>
        /// <param name="LoginModel"></param>
        /// <returns></returns>
        public int InsertLoginData(UserLogin LoginModel)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
                {
                    con.SetPassword("@@#DEMOSMART#@@");
                    con.Open();

                    using (SQLiteCommand com = new SQLiteCommand(con))
                    {
                        string SQLQuery = "insert into LoginTB (Username,Password,CLientIDToken) values ('" + LoginModel.Username + "','" + LoginModel.Password + "','" + LoginModel.RegistrationID + "')";
                        com.CommandText = SQLQuery;
                        return com.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        } 
    }
}
