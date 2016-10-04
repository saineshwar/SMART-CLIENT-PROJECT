using DemoSmartClient.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DemoSmartClient.Interface
{
    public interface IProduct
    {
        bool AddProduct(Product Product);
        DataTable GetData(string CLientIDToken);
        bool DeleteProduct(int ProductID);      
    }
}
