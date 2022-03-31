using Helperland.Data;
using Helperland.Enums;
using Helperland.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Core
{
    public class CookieHelper : ActionFilterAttribute
    {
        private HelperlandContext _helperlandContext;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session.GetString("User");

            if(user == null)
            {
                string userCookie = filterContext.HttpContext.Request.Cookies["UserEmail"];

                if (userCookie != null)
                {
                    _helperlandContext = filterContext.HttpContext.RequestServices.GetService(typeof(HelperlandContext)) as HelperlandContext;
                    User _user = _helperlandContext.Users.Where(x => x.Email == userCookie).FirstOrDefault();

                    int userTypeId = Convert.ToInt32(_user.UserTypeId);

                    SessionUser sessionUser = new SessionUser
                    {
                        UserID = _user.UserId.ToString(),
                        UserName = _user.FirstName + " " + _user.LastName,
                        UserType = ((UserTypeEnum)userTypeId).ToString(),
                        Email = _user.Email.ToString()
                    };

                    filterContext.HttpContext.Session.SetString("User", JsonConvert.SerializeObject(sessionUser));

                    var actionName = filterContext.RouteData.Values["action"] as string;
                    var controllerName = filterContext.RouteData.Values["controller"] as string;

                    filterContext.Result = new RedirectToRouteResult(new { action = actionName, controller = controllerName });
                    return;

                    //if (sessionUser.UserType == UserTypeEnum.Admin.ToString())
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(new { action = "UserManagement", controller = "Admin" });
                    //    return;
                    //}
                    //else if (sessionUser.UserType == UserTypeEnum.Customer.ToString())
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(new { action = "ServiceHistory", controller = "Customer" });
                    //    return;
                    //}
                    //else if (sessionUser.UserType == UserTypeEnum.ServiceProvider.ToString())
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(new { action = "UpcomingService", controller = "ServiceProvider" });
                    //    return;
                    //}
                }
            }
        }
    }
}
