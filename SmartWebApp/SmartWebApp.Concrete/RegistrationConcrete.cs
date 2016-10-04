using SmartWebApp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWebApp.Model;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace SmartWebApp.Concrete
{
    public class RegistrationConcrete : IRegistration
    {
        public int CreateUser(Registration registration)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction sqltans = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", registration.RegistrationID);
                    param.Add("@FirstName", registration.FirstName);
                    param.Add("@Lastname", registration.Lastname);
                    param.Add("@Gender", registration.Gender);
                    param.Add("@Username", registration.Username);
                    param.Add("@Password", registration.Password);
                    param.Add("@Mobileno", registration.Mobileno);
                    param.Add("@EmailID", registration.EmailID);
                    var result = con.Execute("sprocClientRegistrationInsertUpdateSingleItem",
                          param,
                          sqltans,
                          0,
                          commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        sqltans.Commit();
                    }
                    else
                    {
                        sqltans.Rollback();
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool ValidateUser(string Username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@Username", Username);
                    var result = con.Query<int>("ValidateUser", param, null, false, 0, CommandType.StoredProcedure).SingleOrDefault();
                    if (result == 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
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
