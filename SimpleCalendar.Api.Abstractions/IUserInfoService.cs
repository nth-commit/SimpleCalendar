using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api
{
    public interface IUserInfoService
    {
        Task<IEnumerable<Claim>> GetUserInfo(HttpContext httpContext);
    }
}
