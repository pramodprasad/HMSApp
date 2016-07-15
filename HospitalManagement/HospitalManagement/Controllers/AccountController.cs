using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HospitalManagement.Models;
using HMS.Entity;

namespace HospitalManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly HMSDBEntities db = new HMSDBEntities();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.BranchID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.EmployeeTypeID = new SelectList(db.EmployeeTypes, "ID", "Description");
            ViewBag.TitleID = new SelectList(db.Titles, "ID", "Name");
            ViewBag.ShiftTypes = new SelectList(db.ShiftTypes, "ID","Name");
            ViewBag.States = new SelectList(db.States, "ID", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            model.EmpDetails.CreatedDate = DateTime.Now;
            if (model.EmpDetails.EmployeeType_ID != 2)
            {
                   if (ModelState.ContainsKey("Doctor.Qualification"))
                   {
                       ModelState["Doctor.Qualification"].Errors.Clear();
                   }
                   if (ModelState.ContainsKey("Doctor.SpecializationDetails"))
                   {
                       ModelState["Doctor.SpecializationDetails"].Errors.Clear();
                   }
                   if (ModelState.ContainsKey("Doctor.OtherDetails"))
                   {
                       ModelState["Doctor.OtherDetails"].Errors.Clear();
                   }
                   if (ModelState.ContainsKey("Doctor.RegistrationNo"))
                   {
                       ModelState["Doctor.RegistrationNo"].Errors.Clear();
                   }
                   if (ModelState.ContainsKey("Shift"))
                   {
                       ModelState["Shift"].Errors.Clear();
                   }
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.EmailAddress};

                    try
                    {
                        using (HMSDBEntities context = new HMSDBEntities())
                        {
                            EmployeeDetail newEmp = model.EmpDetails;
                                if (model.EmpDetails.EmployeeType_ID == 2)
                                {
                                    try
                                    {
                                        //ShiftType shift = context.ShiftTypes.Find(model.Shift);
                                        Doctor doctor = model.Doctor;
                                        doctor.CreatedOn = DateTime.Now;
                                        doctor.CreatedBy = 1;
                                        doctor.UpdatedBy = 1;
                                        doctor.UpdatedOn = DateTime.Now;
                                        doctor.ShiftTypes = new List<ShiftType>();
                                        //doctor.ShiftTypes.Add(shift);
                                        //shift.Doctors.Add(doctor);
                                                                              
                                        context.EmployeeDetails.Add(newEmp);
                                        newEmp.Doctors.Add(doctor);
                                        
                                        context.SaveChanges();
                                         
                                    }
                                    catch (Exception ex)
                                    {
                                        string exceptionmessage = ex.Message;
                                        ViewBag.BranchID = new SelectList(db.BranchDetails, "ID", "Name");
                                        ViewBag.EmployeeTypeID = new SelectList(db.EmployeeTypes, "ID", "Description");
                                        ViewBag.TitleID = new SelectList(db.Titles, "ID", "Name");
                                        ViewBag.ShiftTypes = new SelectList(db.ShiftTypes, "ID", "Name");
                                        ViewBag.States = new SelectList(db.States, "ID", "Name");
                                        return View(model);
                                     }

                                }
                                else
                                {
                                    context.EmployeeDetails.Add(newEmp);
                                    context.SaveChanges();
                                }
                                user.HMSEmpID = newEmp.ID;                       
                                user.BranchID = Convert.ToInt32(model.EmpDetails.BranchDetail_ID);
                                var result1 = await UserManager.CreateAsync(user, model.Password);
                                if (result1.Succeeded)
                                {
                                    return RedirectToAction("EmpDetails", "Account");
                                }
                                else
                                {
                                    context.EmployeeDetails.Remove(newEmp);
                                    context.SaveChanges();
                                    AddErrors(result1);
                                }
                            }

                        }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
             }

            // If we got this far, something failed, redisplay form
            ViewBag.BranchID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.EmployeeTypeID = new SelectList(db.EmployeeTypes, "ID", "Description");
            ViewBag.TitleID = new SelectList(db.Titles, "ID", "Name");
            ViewBag.ShiftTypes = new SelectList(db.ShiftTypes, "ID", "Name");
            ViewBag.States = new SelectList(db.States, "ID", "Name");
            return View(model);
        }

        //
        //GET: /Account/EmployeesDetails

        [AllowAnonymous]
        public ActionResult EmpDetails()
        {
            return View(db.EmployeeDetails.ToList());
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                var currentEmployee = db.EmployeeDetails.Where(user => user.ID == currentUser.HMSEmpID).FirstOrDefault();

                if (currentEmployee.Password == model.Password)
                {
                    ModelState.AddModelError("", "Provided password is same as current password.!");
                }
                else
                {
                    await UserManager.RemovePasswordAsync(currentUser.Id);
                    IdentityResult result = await UserManager.AddPasswordAsync(currentUser.Id, model.Password);
                    if (result.Succeeded)
                    {
                        currentEmployee.Password = model.Password;
                        db.Entry(currentEmployee).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            var customerId = UserManager.FindById(User.Identity.GetUserId()).HMSEmpID;
            EmployeeDetail employee = db.EmployeeDetails.Where(emp => emp.ID == customerId).FirstOrDefault();
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name");
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name");
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description");
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name");
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(EmployeeDetail model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" , "Home");
            }
            ViewBag.BranchDetail_ID = new SelectList(db.BranchDetails, "ID", "Name", model.BranchDetail_ID);
            ViewBag.City_ID = new SelectList(db.Cities, "ID", "name", model.City_ID);
            ViewBag.EmployeeType_ID = new SelectList(db.EmployeeTypes, "ID", "Description", model.EmployeeType_ID);
            ViewBag.Title_ID = new SelectList(db.Titles, "ID", "Name", model.Title_ID);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}