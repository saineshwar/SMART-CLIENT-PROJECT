using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.Interface
{
    public interface IProduct
    {
        int InsertProduct(Product product);
        List<Product> ListProduct();
    }
}
