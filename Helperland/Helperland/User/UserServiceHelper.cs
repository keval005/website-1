using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Helperland.User
{
    public class UserServiceHelper : IUserServiceHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserServiceHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string getUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
