using Helperland.Core;
using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Helperland.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    [CookieHelper]
    public class AccountController : Controller
    {
        private readonly HelperlandContext _helperlandContext;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IAccountControllerRepository _accountControllerRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _Key = "MSIHELPERLAND";
        private User _user;

        public AccountController(HelperlandContext helperlandContext, IConfiguration configuration, IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider, IAccountControllerRepository accountControllerRepository)
        {
            this._helperlandContext = helperlandContext;
            this._configuration = configuration;
            this._hostingEnvironment = hostingEnvironment;
            this._dataProtectionProvider = dataProtectionProvider;
            this._accountControllerRepository = accountControllerRepository;
        }

        [HttpPost]
        public JsonResult Login([FromBody] LoginViewModel model)
        {
            _user = _accountControllerRepository.GetUserByEmailAndPassword(model.Email.ToString().Trim(), model.Password.ToString().Trim());

            if (_user != null && _user.IsApproved == true)
            {
                int userTypeId = Convert.ToInt32(_user.UserTypeId);

                SessionUser sessionUser = new SessionUser
                {
                    UserID = _user.UserId.ToString(),
                    UserName = _user.FirstName + " " + _user.LastName,
                    UserType = ((UserTypeEnum)userTypeId).ToString(),
                    Email = _user.Email
                };

                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(sessionUser));

                if (model.RememberMe == true)
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(1));
                    HttpContext.Response.Cookies.Append("UserEmail", model.Email, cookieOptions);
                }

                if (_user.UserTypeId == (int)UserTypeEnum.Admin)
                {
                    return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "ok", ErrorMessage = null, Url = "Admin/UserManagement" });
                    //return RedirectToAction("UserManagement", "Admin");
                }
                else if (_user.UserTypeId == (int)UserTypeEnum.ServiceProvider)
                {
                    return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "ok", ErrorMessage = null, Url = "ServiceProvider/UpcomingService" });
                    //return RedirectToAction("UpcomingService", "ServiceProvider");
                }
                else if (_user.UserTypeId == (int)UserTypeEnum.Customer)
                {
                    return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "ok", ErrorMessage = null, Url = "Customer/Dashboard" });
                    //return RedirectToAction("ServiceHistory", "Customer");
                }
            }
            else if (_user != null && _user.IsApproved == false)
            {
                return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "Error", ErrorMessage = "You have not yet approved by Admin" });
                //TempData["ErrorMessage"] = "You have not yet approved by Admin";
            }
            else
            {
                return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "Error", ErrorMessage = "Username or password is invalid" });
                //TempData["ErrorMessage"] = "Username or password is invalid";
            }

            return Json(new SingleEntity<LoginViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            _user = _accountControllerRepository.GetUserByEmail(model.Email.ToString().Trim());

            if (_user != null)
            {
                //Convert password to base64string
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_user.Password);
                //var oldPassword = System.Convert.ToBase64String(plainTextBytes);
                var oldPassword = BCrypt.Net.BCrypt.HashPassword(_user.Password);

                //create token
                string inputToken = model.Email + "_$_" + DateTime.Now.ToString() + "_$_" + oldPassword;

                //encrypt token
                var protector = _dataProtectionProvider.CreateProtector(_Key);
                string encryptToken = protector.Protect(inputToken);

                EmailModel emailModel = new EmailModel
                {
                    DisplayName = _user.FirstName + " " + _user.LastName,
                    To = model.Email,
                    Subject = "Helperland Reset Password Link.",
                    Body = "http://" + this.Request.Host.ToString() + "/Account/ResetPassword?token=" + encryptToken
                };

                MailHelper mailHelper = new MailHelper(_configuration, _hostingEnvironment);

                mailHelper.SendResetPasswordLink(emailModel);
            }
            else
            {
                return Json(new SingleEntity<ForgotPasswordViewModel> { Result = model, Status = "Error", ErrorMessage = "Please Enter Register email Address" });
            }
            return Json(new SingleEntity<ForgotPasswordViewModel> { Result = model, Status = "ok", ErrorMessage = null });
            //return View("~/Views/home/index.cshtml", model);
        }

        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                ViewBag.Messsage = "Password reset link is invalid.";
                return View();
            }
            else
            {
                checkResetPasswordToken(token);
            }

            return View();
        }

        private bool checkResetPasswordToken(string token)
        {
            string decrypt = "";
            try
            {
                var protector = _dataProtectionProvider.CreateProtector(_Key);
                decrypt = protector.Unprotect(token);
            }
            catch
            {
                ViewBag.Messsage = "Password reset link is invalid.";
                return false;
            }

            string[] resetPasswordToken = decrypt.Split("_$_");

            _user = _accountControllerRepository.GetUserByEmail(resetPasswordToken[0].ToString().Trim());

            DateTime tokenDate = Convert.ToDateTime(resetPasswordToken[1]).AddMinutes(30);
            DateTime currentDateTime = DateTime.Now;

            //var base64EncodedBytes = System.Convert.FromBase64String(resetPasswordToken[2]);
            //var oldPassword = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var oldPassword = resetPasswordToken[2];

            if (tokenDate < currentDateTime || !BCrypt.Net.BCrypt.Verify(_user.Password, oldPassword))
            {
                ViewBag.Messsage = "Link is expired";
                return false;
            }

            return true;
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!checkResetPasswordToken(model.Token))
                {
                    return View();
                }

                if (_user.Password == model.NewPassword)
                {
                    TempData["Message"] = "You used this password recently. Please choose a different one.";
                    return View(model);
                }

                _user.Password = model.NewPassword;
                _user.ModifiedBy = _user.UserId;
                _user.ModifiedDate = DateTime.Now;

                _accountControllerRepository.Update(_user);

                ViewBag.Messsage = "Success";

                return View();
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            if (Request.Cookies["UserEmail"] != null)
            {
                Response.Cookies.Delete("UserEmail");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegistration(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.MobileNumber,
                    UserTypeId = (int)UserTypeEnum.Customer,
                    CreatedDate = DateTime.Now,
                    IsApproved = true
                };

                _accountControllerRepository.Add(_user);

                TempData["SuccessMessage"] = "Register Successfully.";

                return RedirectToAction();
            }

            return View("Index", model);
        }

        public IActionResult BecomeAPro()
        {
            ViewBag.navbar = "transparentNavbar";
            ViewBag.hideFlag = true;
            return View();
        }

        //BecomeAPro post method
        [HttpPost]
        public IActionResult ServiceProviderRegistration(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.MobileNumber,
                    UserTypeId = (int)UserTypeEnum.ServiceProvider,
                    CreatedDate = DateTime.Now,
                    IsApproved = false,
                    UserProfilePicture = "/img/admin-panel/sp-avtar/avatar-hat.png"
                };

                _accountControllerRepository.Add(_user);

                TempData["SuccessMessage"] = "Register Successfully. You can login after admin can approved your request.";

                return RedirectToAction("BecomeAPro");
            }

            return View("BecomeAPro", model);
        }

        //For Create new account,    check email is already present in database or not
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _helperlandContext.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email '{email}' is already in use. Please use another email.");
            }
        }

    }
}
