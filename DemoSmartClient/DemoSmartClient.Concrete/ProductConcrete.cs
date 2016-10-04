using DemoSmartClient.Interface;
using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Data;

namespace DemoSmartClient.Concrete
{
    public class ProductConcrete : IProduct
    {
        SQLiteDatabaseHelper db;
        public ProductConcrete()
        {
            db = new SQLiteDatabaseHelper();
        }
  
        /// <summary>
        /// Adding New Product
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        public bool AddProduct(Product Product)
        {
            try
            {
                bool result = false;

                Dictionary<String, String> data = new Dictionary<String, String>();
                data.Add("ProductNumber", Product.ProductNumber);
                data.Add("NAME", Product.Name);
                data.Add("Color", Product.Color);
                data.Add("ProductClass", Product.ProductClass);
                data.Add("Price", Convert.ToString(Product.Price));
                data.Add("Description", Product.Description);
                data.Add("CreatedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                data.Add("CLientIDToken", Product.CLientIDToken);
                result = db.Insert("ProductTB", data);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Getting Product Data from Database based on CLientIDToken
        /// </summary>
        /// <param name="CLientIDToken"></param>
        /// <returns></returns>
        public DataTable GetData(string CLientIDToken)
        {
            DataTable dsproduct = new DataTable();
            try
            {
                
                String query = "select ProductID,ProductNumber,NAME,Color,ProductClass,Price,Description,CreatedDate,CLientIDToken"
                + " from ProductTB where CLientIDToken ='" + CLientIDToken + "'";
                return dsproduct = db.GetDataTable(query);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dsproduct.Dispose();
            }
        }

        /// <summary>
        /// Deleting Product from Database
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public bool DeleteProduct(int ProductID)
        {
            try
            {
                return db.Delete("ProductTB", String.Format("ProductID = {0}", ProductID));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
