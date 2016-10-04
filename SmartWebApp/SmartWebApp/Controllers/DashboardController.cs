using SmartWebApp.BL;
using SmartWebApp.Concrete;
using SmartWebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartWebApp.Controllers
{
    [AuthenticateUser]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        ProductBL ProductBL;
        public DashboardController()
        {
            ProductBL = new ProductBL(new ProductConcrete());
        }

        [HttpGet]
        public ActionResult Home()
        {
            try
            {
                var data = ProductBL.ListProduct();
                return View(data);
            }
            catch (Exception)
            {             
                throw;
            }
        }
    }
}
