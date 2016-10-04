using Dapper;
using SmartWebApp.Interface;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartWebApp.Concrete
{
    public class LoginConcrete : ILogin
    {
        public LoginViewResponse ValidateLoginUser(string Username, string Password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@Username", Username);
                    param.Add("@Password", Password);
                    var result = con.Query<LoginViewResponse>("ValidateLoginUser", param, null, false, 0, CommandType.StoredProcedure).SingleOrDefault();
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        return new LoginViewResponse();
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
