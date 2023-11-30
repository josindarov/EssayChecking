using System;
using System.Linq.Expressions;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit.Sdk;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            
            this.userService = new UserService(
                storageBroker: storageBrokerMock.Object,
                loggingBroker:loggingBrokerMock.Object);
        }
        
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
			actualException => actualException.SameExceptionAs(expectedException);

        private static User CreateRandomUser() =>
            CreateUserFiller().Create();

        private static Filler<User> CreateUserFiller() =>
            new Filler<User>();


    }
}
