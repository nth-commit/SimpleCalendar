using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Middleware.UserPreparation
{
    public interface IUserInfoService
    {
        Task<IEnumerable<Claim>> GetUserInfoAsync(HttpContext httpContext);
    }
}
