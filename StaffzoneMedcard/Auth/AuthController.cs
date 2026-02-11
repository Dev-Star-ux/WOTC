using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using StaffZoneMaster.Controllers;
using StaffZoneMaster.Helpers;
using StaffZoneMaster.Models;

namespace StaffZoneMaster.Auth
{

    [Authorize]
    public class AuthController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthController));

        private readonly AuthService _authService = new AuthService();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (base.User.Identity.IsAuthenticated)
            {
                _authService.Logout(this);
                return RedirectToAction("Login");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!base.ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _authService.Login(this, model.UserName, model.Password);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                base.ViewBag.ErrorMessage = ex.Message;
                LoggerHelper.Error(Log, ex);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
                _authService.Logout(this);
            }
            catch (Exception ex)
            {
                base.ViewBag.ErrorMessage = ex.Message;
                LoggerHelper.Error(Log, ex);
            }
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                base.ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (base.Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }

}
