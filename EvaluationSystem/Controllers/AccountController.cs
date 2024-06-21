using EvaluationSystem.Data;
using EvaluationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvaluationSystem.Data.Entities;
using System.Web.Security;
using EvaluationSystem.Data.Interfaces;
using Newtonsoft.Json;

namespace EvaluationSystem.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountViewModel model, string returnUrl)
        
        {
            if (ModelState.IsValid)
            {
                var user = _accountRepository.Login(model.UserName, model.Password);
                if (user != null)
                {
                    Principal principal = new Principal();
                    principal.Id = user.Id;
                    principal.UserName = user.UserName;

                    string userData = JsonConvert.SerializeObject(principal);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1,
                                user.UserName,
                                DateTime.Now,
                                DateTime.Now.AddHours(12),
                                model.RememberMe,
                                userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("", "Toàn khoản hoặc mật khẩu không chính xác.");
            }
            return View(model);

            
        }
        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }    
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Login", "Account");
        }
    }
}