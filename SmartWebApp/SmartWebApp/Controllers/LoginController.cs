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

    public class LoginController : Controller
    {
        //
        // GET: /Login/
        LoginBL LoginBL;
        public LoginController()
        {
            LoginBL = new LoginBL(new LoginConcrete());
        }

        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(LoginViewModel LoginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string password = EncryptandDecryptAES.Encrypt(LoginViewModel.Password);

                    var val = LoginBL.ValidateLoginUser(LoginViewModel.Username, password);
                    if (val.RegistrationID == null)
                    {
                        Session["UserToken"] = string.Empty;
                        TempData["Message"] = "Invalid Username and Password.";
                        return RedirectToAction("Users", "Login");
                    }
                    else
                    {
                        Session["UserToken"] = val.RegistrationID;
                        return RedirectToAction("Home", "Dashboard");
                    }
                }
                return View("Users", LoginViewModel);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Users", "Login");
        }

    }
}
