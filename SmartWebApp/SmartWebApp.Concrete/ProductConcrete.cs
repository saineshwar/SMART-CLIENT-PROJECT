using Dapper;
using SmartWebApp.Interface;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SmartWebApp.Concrete
{
    public class ProductConcrete : IProduct
    {
        public int InsertProduct(Product product)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction sqltans = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@ProductID", product.ProductID);
                    param.Add("@ProductNumber", product.ProductNumber);
                    param.Add("@Name", product.Name);
                    param.Add("@Color", product.Color);
                    param.Add("@ProductClass", product.ProductClass);
                    param.Add("@Price", product.Price);
                    param.Add("@Description", product.Description);
                    param.Add("@CreatedDate", product.CreatedDate);
                    param.Add("@CLientIDToken", product.CLientIDToken);

                    var result = con.Execute("sprocProductTBInsertUpdateSingleItem",
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

        public List<Product> ListProduct()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString))
                {
                    con.Open();
                    return con.Query<Product>("sprocProductTBSelectList", null, null, false, 0, commandType: CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
