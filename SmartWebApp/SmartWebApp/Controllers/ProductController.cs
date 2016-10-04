using SmartWebApp.BL;
using SmartWebApp.Concrete;
using SmartWebApp.CryptoLib;
using SmartWebApp.Filters;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SmartWebApp.Controllers
{
    public class ProductController : ApiController
    {
        ProductBL ProductBL;

        string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
        string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();
       
        public ProductController()
        {
            ProductBL = new ProductBL(new ProductConcrete());
        }

     
        // POST api/product
        [AuthoriseAPI]
        public HttpResponseMessage Post(HttpRequestMessage Request)
        {
            try
            {
                var Responsedata = Request.Content.ReadAsStringAsync().Result;

                string data = EncryptionDecryptorTripleDES.Decryption(Responsedata, keyValue, IVValue);

                Product objpro = new JavaScriptSerializer().Deserialize<Product>(data);

                if (objpro.CLientIDToken != null)
                {
                    var result = ProductBL.InsertProduct(objpro);

                    if (result > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, objpro.ProductID);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Failed");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Failed");
                }
            }
            catch (Exception)
            {
               return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Failed");
            }
        }
    }
}
