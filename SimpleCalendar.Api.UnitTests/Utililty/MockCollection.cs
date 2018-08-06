using Microsoft.AspNetCore.Http;
using Moq;
using SimpleCalendar.Api.Middleware.UserPreparation;
using SimpleCalendar.Api.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public class MockCollection
    {
        public Mock<IUserEmailContainer> UserEmail { get; set; } = new Mock<IUserEmailContainer>();

        public Mock<IUserInfoService> UserInfoService { get; set; } = new Mock<IUserInfoService>();

        public Mock<IDateTimeAccessor> DateTimeAccessor { get; set; } = new Mock<IDateTimeAccessor>();

        public MockCollection()
        {
            UserInfoService
                .Setup(x => x.GetUserInfoAsync(It.IsAny<HttpContext>()))
                .ReturnsAsync<HttpContext, IUserInfoService, IEnumerable<Claim>>(context =>
                    context.User.Claims);
        }
    }
}
