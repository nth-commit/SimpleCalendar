using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var result = claimsBySub[user.OriginatingSub].ToDictionary(
                kvp => kvp.Key,
                kvp => (object)kvp.Value);

            var hasCreatedEvent = await _coreDbContext.Events
                .Where(e => e.CreatedByEmail == email)
                .Where(e => !e.IsDeleted)
                .AnyAsync();

            result.Add("hasCreatedEvent", hasCreatedEvent);

            return new OkObjectResult(result);
        }
    }
}
