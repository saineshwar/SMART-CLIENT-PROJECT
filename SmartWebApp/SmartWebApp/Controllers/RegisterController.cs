using SmartWebApp.BL;
using SmartWebApp.Concrete;
using SmartWebApp.CryptoLib;
using SmartWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartWebApp.Controllers
{
    public class RegisterController : Controller
    {
        RegistrationBL _RegistrationBL;
        public RegisterController()
        {
            _RegistrationBL = new RegistrationBL(new RegistrationConcrete());
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(Registration Registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Registration.Password = EncryptandDecryptAES.Encrypt(Registration.Password);
                    _RegistrationBL.CreateUser(Registration);
                    TempData["Message"] = "Registration Done Successfully";
                    return RedirectToAction("NewUser", "Register");
                }
                else
                {
                    return View(Registration);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public JsonResult ValidateUser(string Username)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var result = _RegistrationBL.ValidateUser(Username);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
