using DemoSmartClient.Interface;
using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DemoSmartClient.BL
{
    public class ProductBL
    {
        IProduct _IProduct;
        public ProductBL(IProduct IProduct)
        {
            _IProduct = IProduct;
        }

        public bool AddProduct(Product Product)
        {
            return _IProduct.AddProduct(Product);
        }

        public DataTable GetData(string CLientIDToken)
        {
            return _IProduct.GetData(CLientIDToken);
        }

        public bool DeleteProduct(int ProductID)
        {
            return _IProduct.DeleteProduct(ProductID);
        }
     
    }
}
