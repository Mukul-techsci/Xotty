using BussinessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Xotty.Controllers
{
   
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(enLogin model)
        {
            try
            {
              
                    blLogin bl = new blLogin(model);

                    var result = bl.LoginUser();

                    if (result != null && result.Count > 0)
                    {
                        if (result[0].Status == 1)
                        {
                            // Session Store
                            HttpContext.Session.SetString("UserEmail", result[0].Email ?? "");
                            HttpContext.Session.SetString("UserName", result[0].Name ?? "");
                            HttpContext.Session.SetInt32("UserId", result[0].Id);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", result[0].Message ?? "Login failed");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Email or Password");
                    }
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(enLogin model)
        {
            try
            {
                
                    // Confirm Password Check
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Password and Confirm Password do not match.");
                        return View(model);
                    }

                    blLogin bl = new blLogin(model);

                    List<enLogin> result = bl.RegisterUser();

                    if (result != null && result.Count > 0)
                    {
                        if (result[0].Status == 1)
                        {
                            TempData["Success"] = result[0].Message;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", result[0].Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something went wrong.");
                    }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }
        public IActionResult MyAddress()
        {

            enLogin model = new enLogin();
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            blLogin bl = new blLogin(model);

            var data = bl.GetUserAddress(userId.Value);

            return View(data);
        }
               

        [HttpPost]
        public JsonResult SaveAddress(enAddress enAddress)
        {
            enLogin model = new enLogin();
            enAddress.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            blLogin bl = new blLogin(model);

            var result = bl.SaveUserAddress(enAddress);

            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteAddress(int id)
        {
            enLogin model = new enLogin();
            blLogin bl = new blLogin(model);

            var result = bl.DeleteAddress(id);

            return Json(result);
        }

    }
}
