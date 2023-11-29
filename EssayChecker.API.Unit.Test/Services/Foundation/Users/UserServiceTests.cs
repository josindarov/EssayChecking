using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.userService = new UserService(
                storageBroker: storageBrokerMock.Object);
        }

        private static User CreateRandomUser() =>
            CreateUserFiller().Create();

        private static Filler<User> CreateUserFiller() =>
            new Filler<User>();


    }
}
