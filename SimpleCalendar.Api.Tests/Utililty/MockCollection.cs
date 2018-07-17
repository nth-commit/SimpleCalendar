using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public class MockCollection
    {
        public Mock<IUserEmailContainer> UserEmail { get; set; } = new Mock<IUserEmailContainer>();
    }
}
