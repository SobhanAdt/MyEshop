using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using DataLayer.ViewModel;
namespace MyEshop.Areas.UserPanel.Controllers
{
    public class AccountController : Controller
    {
        MyEshop_DBEntities db=new MyEshop_DBEntities();
        // GET: UserPanel/Account
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(CahngePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Single(u => u.UserName == User.Identity.Name);
                string oldpasswordhash =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
                if (user.Password==oldpasswordhash)
                {
                    string hashNewPassword =
                        FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
                    user.Password = hashNewPassword;
                    db.SaveChanges();
                    ViewBag.Success = true;
                }
                else
                {
                    ModelState.AddModelError("OldPassword","کلمه عبور فعلی درست نمی باشد");
                }
            }
            return View();

        }
    }
}