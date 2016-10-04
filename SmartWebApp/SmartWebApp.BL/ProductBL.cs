using SmartWebApp.Interface;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartWebApp.BL
{
    public class ProductBL
    {
        IProduct _IProduct;
        public ProductBL(IProduct IProduct)
        {
            _IProduct = IProduct;
        }

        public int InsertProduct(Product product)
        {
          return  _IProduct.InsertProduct(product);
        }
        public List<Product> ListProduct()
        {
            return _IProduct.ListProduct();
        }
    }
}
