using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCalendar.Api.Commands.Users
{
    public interface IGetUserCommand : ICommand<string>
    {
    }
}
