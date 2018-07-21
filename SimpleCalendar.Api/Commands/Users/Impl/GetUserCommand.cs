using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;

namespace SimpleCalendar.Api.Commands.Users.Impl
{
    public class GetUserCommand : IGetUserCommand
    {
        private readonly CoreDbContext _coreDbContext;

        public GetUserCommand(
            CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string email)
        {
            var user = await _coreDbContext.Users.FindAsync(email);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var claimsBySub = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(user.ClaimsBySubJson);
            return new OkObjectResult(claimsBySub[user.OriginatingSub]);
        }
    }
}
