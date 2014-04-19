using PQM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PQM.Controllers
{
    public class UsersController : Controller
    {
        private project_infoEntities db = new project_infoEntities();

        //
        // GET: /Users/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            Users user = new Users();
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName,user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    if (this.Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Project");
                    }
                }
                this.ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu!");
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        // GET:
        public ActionResult ChangePassword()
        {
            ManagerUsers muser = new ManagerUsers();
            return View(muser);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ManagerUsers muser)
        {
            if (ModelState.IsValid)
            {
                if (muser.IsValid(User.Identity.Name, muser.OldPassword))
                {
                    var q = from m in db.users
                            where User.Identity.Name == m.email.ToString()
                            select m;
                    user userdata = db.users.First(i => i.email == User.Identity.Name);
                    userdata.password = muser.NewPassword;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Project");
                }
                else
                {
                    this.ModelState.AddModelError("", "Mật khẩu cũ không khớp!");
                }
            }
            return View(muser);
        }

        // GET:
        public ActionResult Create()
        {
            return View();
        }

        // POST:
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,email,password")] user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }
	}
}