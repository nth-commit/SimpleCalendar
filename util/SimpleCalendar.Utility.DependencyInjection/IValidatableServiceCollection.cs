using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.DependencyInjection
{
    public interface IValidatableServiceCollection : IServiceCollection
    {
        void AddRequirement(Type type);

        void ValidateRequirements();
    }
}
