using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Filters
{
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var boundNameByParameterName = controllerActionDescriptor.Parameters.ToDictionary(
                    p => p.Name,
                    p => p.BindingInfo.BinderModelName ?? p.Name);

                var argumentValueByParameterName = context.ActionArguments.ToDictionary(
                    a => boundNameByParameterName[a.Key],
                    a => a.Value);

                var invalidBoundParameters = controllerActionDescriptor.MethodInfo
                    .GetParameters()
                    .Select(p => new
                    {
                        Parameter = controllerActionDescriptor.Parameters.OfType<ControllerParameterDescriptor>().Single(p2 => p.Name == p2.Name),
                        HasValue = argumentValueByParameterName.TryGetValue(p.Name, out object value),
                        Value = value,
                        ValidationAttributes = p.GetCustomAttributes(false).OfType<ValidationAttribute>()
                    })
                    .Where(x =>
                        x.ValidationAttributes.Any() &&
                        (!x.HasValue || !IsValueValid(x.Value, x.ValidationAttributes)));

                foreach (var invalidBoundParameter in invalidBoundParameters)
                {
                    context.ModelState.AddModelError(
                        GetInvalidBindingModelErrorKey(invalidBoundParameter.Parameter),
                        "Value was invalid");
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        private bool IsValueValid(object value, IEnumerable<ValidationAttribute> validationAttributes) =>
            Validator.TryValidateValue(
                value,
                new ValidationContext(new object()),
                new List<ValidationResult>(),
                validationAttributes);

        private string GetInvalidBindingModelErrorKey(ControllerParameterDescriptor p) =>
            p.BindingInfo.BindingSource == BindingSource.Body ? string.Empty :
            p.BindingInfo.BindingSource == BindingSource.Query ? p.BindingInfo.BinderModelName ?? p.Name :
            string.Empty;
    }
}
