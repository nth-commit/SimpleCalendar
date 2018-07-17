using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands
{
    public interface ICommand
    {
        Task<IActionResult> InvokeAsync(ActionContext context);
    }

    public interface ICommand<T1>
    {
        Task<IActionResult> InvokeAsync(ActionContext context, T1 arg1);
    }

    public interface ICommand<T1, T2>
    {
        Task<IActionResult> InvokeAsync(ActionContext context, T1 arg1, T2 arg2);
    }

    public interface ICommand<T1, T2, T3>
    {
        Task<IActionResult> InvokeAsync(ActionContext context, T1 arg1, T2 arg2, T3 arg3);
    }
}
