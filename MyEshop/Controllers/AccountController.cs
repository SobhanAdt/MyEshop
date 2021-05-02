using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModel;
using System.Web.Security;
using System.Xml.Schema;

namespace MyEshop.Controllers
{
    
    public class AccountController : Controller
    {
        MyEshop_DBEntities db=new MyEshop_DBEntities();
        // GET: Account
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public ActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u=>u.Email==register.Email.Trim().ToLower()))
                {
                    Users user=new Users()
                    {
                        Email = register.Email,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password,"MD5"),
                        ActiveCode = Guid.NewGuid().ToString(),
                        IsActive = false,
                        RegisterDate = DateTime.Now,
                        RoleID = 1,
                        UserName =register.UserName
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    //send Email
                    string body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivationEmail", user);
                    SendEmail.Send(user.Email,"ایمیل فعال سازی ",body);
                    //end send email

                    return View("SuccessRegister", user);
                }
                else
                {
                    ModelState.AddModelError("Email","ایمیل وارد شده تکراری است");
                }
            }
            return View(register);
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login,string ReturnUrl="/")
        {
            if (ModelState.IsValid)
            {
                string HashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password,"MD5");
                var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == HashPassword);
                if (user!=null)
                {
                    if (user.IsActive)
                    {
                       FormsAuthentication.SetAuthCookie(user.UserName,login.RememberMe);
                       return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email","کاربری با اطلاعات وارد شده یافت نشد");
                }
            }

            return View(login);
        }

        public ActionResult ActiveUser(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
            if (user==null)
            {
                return HttpNotFound();
            }

            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.UserName = user.UserName;
            
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [Route("ForgetPassword")]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [Route("ForgetPassword")]
        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordViewModel forget)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == forget.Email);
                if (user !=null)
                {
                    if (user.IsActive)
                    {
                        string bodyEmail =
                            PartialToStringClass.RenderPartialView("ManageEmail", "RecoveryPassword", user);
                        SendEmail.Send(user.Email,"بازیابی کلمه عبور",bodyEmail);
                        return View("SuccessForgetPasword", user);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری یافت نشد");
                }
            }
            {
                
            }
            return View();
        }

        public ActionResult RecoveryPassword(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoveryPassword(string id,RecoveryPasswordViewModel recovery)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
                if (user==null)
                {
                    return HttpNotFound();
                }
               

                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.SaveChanges();
                return Redirect("/Login?recovery=ture");
            }
            return View();
        }
    }
}