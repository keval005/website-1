using Helperland.Enums;
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
    public class SessionHelper : ActionFilterAttribute
    {
        private readonly UserTypeEnum _userType;

        public SessionHelper(UserTypeEnum userType)
        {
            this._userType = userType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Session.GetString("User");
            filterContext.HttpContext.Request.Path.ToString();
            if (user == null)
            {
                var actionName = filterContext.RouteData.Values["action"] as string;
                var controllerName = filterContext.RouteData.Values["controller"] as string;

                filterContext.Result = new RedirectToActionResult("Index", "Home", new { returnUrl = controllerName + "/" + actionName });
                return;
            }
            else
            {
                SessionUser sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
                if (sessionUser.UserType != _userType.ToString())
                {
                    filterContext.Result = new RedirectToRouteResult(new { action = "Index", controller = "Home" });
                    return;
                }
            }
        }
    }

    public class SessionUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
    }
}
