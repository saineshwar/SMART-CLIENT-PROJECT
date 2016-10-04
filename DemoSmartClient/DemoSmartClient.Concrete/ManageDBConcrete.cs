using DemoSmartClient.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DemoSmartClient.Concrete
{
    public class ManageDBConcrete : IManageDB
    {
        /// <summary>
        /// Creating SQLite Database
        /// </summary>
        public void CreateSqlLiteDatabase()
        {
            SQLiteConnection.CreateFile("SmartData.db3");
        }

        /// <summary>
        /// Changing password of Database
        /// </summary>
        public void Changepassword()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
            {
                con.Open();
                con.ChangePassword("@@#DEMOSMART#@@");
            }
        }

        /// <summary>
        /// Setting Database password
        /// </summary>
        public void Setpassword()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
            {
                con.SetPassword("@@#DEMOSMART#@@");
            }
        }

        /// <summary>
        /// Removing Database password
        /// </summary>
        public void Removepassword()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
            {
                con.SetPassword("@@#DEMOSMART#@@");
                con.Open();            
                con.ChangePassword("");
            }
        }

        /// <summary>
        /// Creating product Table in Database
        /// </summary>
        public void Createtbproduct()
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
                {

                    con.Open();
                    using (SQLiteCommand com = new SQLiteCommand(con))
                    {
                        string createTableQuery =
                           @"CREATE TABLE IF NOT EXISTS [ProductTB] (
                          [ProductID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [ProductNumber] NVARCHAR(2048)  NULL,
                          [Name] VARCHAR(2048)  NULL,
                          [Color] VARCHAR(2048)  NULL,
                          [ProductClass] VARCHAR(2048)  NULL,
                          [Price] Decimal(18,2)  NULL,
                          [Description] VARCHAR(2048)  NULL,
                          [CreatedDate] DateTime ,
                          [CLientIDToken] VARCHAR(2048) NULL
                          )";
                        com.CommandText = createTableQuery;
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creating Login Table in Database
        /// </summary>
        public void Createt_Login_Table()
        {
            using (SQLiteConnection con = new SQLiteConnection(ConfigurationManager.AppSettings["DBConnection"].ToString()))
            {
                con.Open();

                using (SQLiteCommand com = new SQLiteCommand(con))
                {
                    string createTableQuery =
                       @"CREATE TABLE IF NOT EXISTS [LoginTB] (
                          [UserID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Username] NVARCHAR(2048)  NULL,
                          [Password] VARCHAR(2048)  NULL,
                          [CLientIDToken] VARCHAR(2048)  NULL
                          )";
                    com.CommandText = createTableQuery;
                    com.ExecuteNonQuery();
                }
            }
        }
    }
}
