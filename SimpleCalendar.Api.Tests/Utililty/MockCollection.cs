using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public class MockCollection
    {
        public Mock<IUserIdContainer> UserId { get; set; } = new Mock<IUserIdContainer>();
    }
}
